using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Manager;
using NHibernate;
using NHibernate.Context;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Proxy;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class RepositoryBase<T> where T : class
    {
        public IList<T> FindAll()
        {
            try
            {
                return Session.CreateCriteria(typeof(T)).List<T>();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public T FirstById(Guid id)
        {
            try
            {
                return Session.Get<T>(id);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public T FirstOrDefault()
        {
            try
            {
                return this.Session.Query<T>().FirstOrDefault();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public virtual T SaveOrUpdate(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.SaveOrUpdate(entity);

                transacao.Commit();

                return entity;

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public virtual T Save(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Save(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Update(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível editar " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public void Delete(T entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public void DeleteAll(List<T> entity)
        {
            try
            {
                var transacao = Session.BeginTransaction();

                Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        protected ISession Session
        {
            get
            {
                try
                {
                    var sessionFactory = SessionManager.Instance.LoadSessionManager("dbfactory");

                    if (CurrentSessionContext.HasBind(sessionFactory))
                    {
                        if (sessionFactory.GetCurrentSession().IsOpen)
                        {
                            return sessionFactory.GetCurrentSession();
                        }
                    }

                    var session = sessionFactory.OpenSession();
                    session.FlushMode = FlushMode.Commit;

                    CurrentSessionContext.Bind(session);

                    return session;
                }
                catch (Exception ex)
                {
                    throw new Exception("Não foi possível criar a Sessão.", ex);
                }
            }
        }

        public void Clear()
        {
            Session?.Clear();
        }
    }
}
