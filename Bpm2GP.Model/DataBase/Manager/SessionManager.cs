using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bpm2GP.Model.DataBase.Models;
using IniParser;
using IniParser.Model;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;

namespace Bpm2GP.Model.DataBase.Manager
{
    public class SessionManager
    {
        private static SessionManager _sessionManager = null;
        public static SessionManager Instance => _sessionManager ?? (_sessionManager = new SessionManager());
        public DbFactory DbFactory { get; set; }
        public Dictionary<string, ISessionFactory> SessionsFactories { get; set; }

        private SessionManager()
        {
            SessionsFactories = new Dictionary<string, ISessionFactory>();
            Conexao();
            DbFactory = new DbFactory();
        }

        public ISessionFactory LoadSessionManager(string persistenceUnit)
        {
            if (SessionsFactories.ContainsKey(persistenceUnit))
                return SessionsFactories[persistenceUnit];

            throw new NullReferenceException("Nenhum SessionManager registrada!");
        }

        private void Conexao()
        {
            try
            {
                var iniFile = LerIni();

                var server = iniFile["DbConfig"]["server"];
                var port = iniFile["DbConfig"]["port"];
                var dbName = iniFile["DbConfig"]["dbName"];
                var user = iniFile["DbConfig"]["user"];
                var psw = iniFile["DbConfig"]["psw"];

                var stringConexao = "Persist Security Info=False;server=" + server + ";port=" + port + ";database=" +
                                    dbName + ";uid=" + user + ";pwd=" + psw;

                var mySql = new MySqlConnection(stringConexao);
                try
                {
                    mySql.Open();
                }
                catch
                {
                    CriarSchemaBanco(server, port, dbName, psw, user);
                }
                finally
                {
                    if (mySql.State == ConnectionState.Open)
                    {
                        mySql.Close();
                    }
                }

                ConfigurarNHibernate(stringConexao);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel conectar", ex);
            }
        }

        private IniData LerIni()
        {
            try
            {
                var diretorio = System.Environment.CurrentDirectory;
                var arquivo = diretorio + "/Config/bpm2game_config.ini";
                if (HttpContext.Current != null)
                {
                    arquivo = HttpContext.Current.Server.MapPath("/Config/bpm2game_config.ini").Replace("\\", "/");
                }

                if (!System.IO.File.Exists(arquivo))
                {
                    throw new Exception("O arquivo de configuração não existe no diretório.");
                }

                var parser = new FileIniDataParser();

                return parser.ReadFile(arquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível ler o arquivo de configuração.", ex);
            }
        }

        private void CriarSchemaBanco(string server, string port, string dbName, string psw, string user)
        {
            try
            {
                var stringConexao = "server=" + server + ";user=" + user + ";port=" + port + ";password=" + psw + ";";
                var mySql = new MySqlConnection(stringConexao);
                var cmd = mySql.CreateCommand();

                mySql.Open();
                cmd.CommandText = "CREATE DATABASE IF NOT EXISTS `" + dbName + "`;";
                cmd.ExecuteNonQuery();
                mySql.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi criar o banco de dados.", ex);
            }
        }

        private void ConfigurarNHibernate(String stringConexao)
        {
            try
            {
                var config = new Configuration();

                ConfigureLog4Net();

                //Configuração do NH com o MySQL
                config.DataBaseIntegration(i =>
                {
                    //Dialeto do Banco
                    i.Dialect<NHibernate.Dialect.MySQLDialect>();
                    //Conexao string
                    i.ConnectionString = stringConexao;
                    //Drive de conexão com o banco
                    i.Driver<NHibernate.Driver.MySqlDataDriver>();
                    // Provedor de conexão do MySQL 
                    i.ConnectionProvider<NHibernate.Connection.DriverConnectionProvider>();
                    // GERA LOG DOS SQL EXECUTADOS NO CONSOLE
                    i.LogSqlInConsole = true;
                    // DESCOMENTAR CASO QUEIRA VISUALIZAR O LOG DE SQL FORMATADO NO CONSOLE
                    i.LogFormattedSql = true;
                    // CRIA O SCHEMA DO BANCO DE DADOS SEMPRE QUE A CONFIGURATION FOR UTILIZADA
                    i.SchemaAction = SchemaAutoAction.Update;
                });

                //Realiza o mapeamento das classes
                var maps = this.Mapeamento();
                config.AddMapping(maps);

                //Verifico se a aplicação é Desktop ou Web
                if (HttpContext.Current == null)
                {
                    config.CurrentSessionContext<ThreadStaticSessionContext>();
                }
                else
                {
                    config.CurrentSessionContext<WebSessionContext>();
                }

                var sessionFactory = config.BuildSessionFactory();
                SessionsFactories.Add("dbfactory", sessionFactory);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível configurar NH", ex);
            }
        }

        private HbmMapping Mapeamento()
        {
            try
            {
                var mapper = new ModelMapper();

                mapper.AddMappings(
                    Assembly.GetAssembly(typeof(DesignerMap)).GetTypes()
                );

                return mapper.CompileMappingForAllExplicitlyAddedEntities();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o mapeamento do modelo.", ex);
            }
        }

        public static void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();

            /***
            Exemplo de configuração do log para nhibernate no arquivo app.config
            -------------------------------------------------------------------
              <configSections>
                <section name="log4net"
                  type="log4net.Config.Log4NetConfigurationSectionHandler, log4net"/>
              </configSections>
              <log4net>
                <appender name="NHLog" type="log4net.Appender.RollingFileAppender, log4net" >
                  <param name="File" value="NHLog.txt" />
                  <param name="AppendToFile" value="true" />
                  <param name="maximumFileSize" value="200KB" />
                  <param name="maxSizeRollBackups" value="1" />
                  <layout type="log4net.Layout.PatternLayout, log4net">
                    <conversionPattern
                    value="%date{yyyy.MM.dd hh:mm:ss} %-5level [%thread] - %message%newline" />
                  </layout>
                </appender>
                <!-- levels: ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF -->
                <root>
                  <level value="INFO" />
                  <appender-ref ref="NHLog" />
                </root>
                <logger name="NHBase.SQL">
                  <level value="ALL" />
                  <appender-ref ref="NHLog" />
                </logger>
              </log4net>
            ***/
        }
    }
}
