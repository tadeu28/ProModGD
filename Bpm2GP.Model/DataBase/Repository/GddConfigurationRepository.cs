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
        public GddConfigurationRepository(ISession session) : base(session) { }

        public List<GddConfiguration> FindAllGenresByDesigner(Designer designer, bool isInactive)
        {
            return this.Session.Query<GddConfiguration>().Where(w => w.Inactive == isInactive &&
                                                             (w.IsConstant ||
                                                              w.Designer == designer))
                                                         .ToList();
        }
    }
}
