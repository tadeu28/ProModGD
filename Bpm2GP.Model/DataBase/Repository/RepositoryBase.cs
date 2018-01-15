using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Proxy;

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

        public T FindFirstById(Guid id)
        {
            return this.Session.CreateCriteria<T>()
                        .Add(Restrictions.Eq("Id", id))
                        .SetMaxResults(1)
                        .List<T>()
                        .FirstOrDefault();
        }

        public T FindFirstOrDefault()
        {
            return this.Session.Query<T>().FirstOrDefault();
        }

        public virtual T SaveOrUpdate(T entity)
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

        public virtual T Save(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.Save(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.Update(entity);

                transacao.Commit();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível editar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                this.Session.Clear();

                var transacao = this.Session.BeginTransaction();

                this.Session.Delete(entity);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível excluir " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

        public void Delete(T entity, string id)
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

        public virtual T UnProxy(T entity)
        {
            try
            {
                return (T) this.Session.GetSessionImplementation().PersistenceContext.Unproxy(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível salvar " + typeof(T) + "\nErro:" + ex.Message);
            }
        }

    }
}
