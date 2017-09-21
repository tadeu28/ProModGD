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
        public GameGenreElementRepository(ISession session) : base(session) { }

        public List<GameGenreElement> FindAllElementsByGenreId(Guid genreId)
        {
            return this.Session.Query<GameGenreElement>().Where(w => w.GameGenre.Id == genreId).ToList();
        }

        public List<GameGenreElement> FindAllElementsByListId(Guid[] ids)
        {
            return this.Session.Query<GameGenreElement>().Where(w => ids.Contains(w.Id)).ToList();
        }

        public List<GameGenreElement> FindAllElementsByGenreNotInListId(Guid idGenre, Guid[] ids)
        {
            return this.Session.Query<GameGenreElement>().Where(w =>  w.GameGenre.Id == idGenre && !ids.Contains(w.Id)).ToList();
        }
    }
}
