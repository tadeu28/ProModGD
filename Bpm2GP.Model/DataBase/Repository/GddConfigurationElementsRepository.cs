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
    public class GddConfigurationElementsRepository : RepositoryBase<GddConfigurationElements>
    {
        public GddConfigurationElementsRepository(ISession session) : base(session) { }

        public List<GddConfigurationElements> FindAllElementsByGddId(Guid Id)
        {
            return this.Session.Query<GddConfigurationElements>().Where(w => w.Id == Id).ToList();
        }
    }
}
