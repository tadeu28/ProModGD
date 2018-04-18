using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Models;
using NHibernate;
using NHibernate.Linq;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class AssociationConfRepository : RepositoryBase<AssociationConf>
    {
        public List<AssociationConf> FindAllElementsByGenreAndLanguage(Guid genreId, Guid languageId, bool isInactive)
        {
            try
            {
                return this.Session.Query<AssociationConf>().Where(w => w.Genre.Id == genreId && w.Language.Id == languageId && w.Inactive == isInactive).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public AssociationConf FirstById(Guid Id, bool isInactive)
        {
            try
            {
                return this.Session.Query<AssociationConf>().FirstOrDefault(w => w.Id == Id && w.Inactive == isInactive);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<AssociationConf> FindAllElementsByGenre(Guid genreId, bool isInactive)
        {
            try
            {
                return this.Session.Query<AssociationConf>().Where(w => w.Genre.Id == genreId && w.Inactive == isInactive).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }
    }
}
