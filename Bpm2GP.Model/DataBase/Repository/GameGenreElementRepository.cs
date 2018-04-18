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
    
    public class GameGenreElementRepository : RepositoryBase<GameGenreElement>
    {
        public List<GameGenreElement> FindAllElementsByGenreId(Guid genreId, bool alsoInactive)
        {
            try
            {
                return this.Session.Query<GameGenreElement>().Where(w => w.GameGenre.Id == genreId && w.Inactive == alsoInactive).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<GameGenreElement> FindAllElementsByListId(Guid[] ids)
        {
            try
            {
                return this.Session.Query<GameGenreElement>().Where(w => ids.Contains(w.Id)).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<GameGenreElement> FindAllElementsByGenreNotInListId(Guid idGenre, Guid[] ids)
        {
            try
            {
                return this.Session.Query<GameGenreElement>().Where(w => w.GameGenre.Id == idGenre && !ids.Contains(w.Id)).ToList();
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
