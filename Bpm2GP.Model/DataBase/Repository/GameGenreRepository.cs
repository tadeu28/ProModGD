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
        public List<GameGenre> FindAllGenresByDesigner(Guid idDesigner, bool isInactive)
        {
            try
            {
                return this.Session.Query<GameGenre>().Where(w => w.Inactive == isInactive &&
                                                             (w.IsConstant ||
                                                              w.Designer.Id == idDesigner))
                                                         .ToList();
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
