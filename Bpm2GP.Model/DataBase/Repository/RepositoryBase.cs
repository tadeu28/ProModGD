using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class RepositoryBase<T> where T : class
    {
        public ISession Session;

        public RepositoryBase(ISession session)
        {
            this.Session = session;
        }

        public virtual IList<T> FindAll()
        {
            return this.Session.CreateCriteria(typeof(T)).List<T>();
        }

        public T FindFirstOrDefault()
        {
            return this.Session.Query<T>().FirstOrDefault();
        }

        public virtual T Save(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.SaveOrUpdate(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                var transacao = this.Session.BeginTransaction();

                this.Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Delete(T entity, int id)
        {
            try
            {
                this.Session.CreateQuery(String.Format("delete from {0} where id = {1}", typeof(T).Name, id)).ExecuteUpdate();

                this.Session.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Clear()
        {
            if(this.Session != null)
                this.Session.Clear();
        }

    }
}
