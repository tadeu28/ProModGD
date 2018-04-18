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
    public class GddConfigurationRepository : RepositoryBase<GddConfiguration>
    {

        public List<GddConfiguration> FindAllGenresByDesigner(Designer designer, bool isInactive)
        {
            try
            {
                return this.Session.Query<GddConfiguration>().Where(w => w.Inactive == isInactive &&
                                                             (w.IsConstant ||
                                                              w.Designer == designer))
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

        public List<GddConfiguration> FindAllByGameGenre(GameGenre gameGenre, bool isInactive)
        {
            try
            {
                return this.Session.Query<GddConfiguration>().Where(w => w.Inactive == isInactive &&
                                                                     w.GameGenre.Id == gameGenre.Id)
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
