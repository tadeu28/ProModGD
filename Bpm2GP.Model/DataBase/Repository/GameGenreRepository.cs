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
    public class GameGenreRepository : RepositoryBase<GameGenre>
    {
        public GameGenreRepository (ISession session) : base(session) { }

        public List<GameGenre> FindAllGenresByDesigner(Designer designer, bool isInactive)
        {
            return this.Session.Query<GameGenre>().Where(w => w.Inactive == isInactive && 
                                                             (w.IsConstant ||
                                                              w.Designer == designer))
                                                         .ToList();
        }
    }
}
