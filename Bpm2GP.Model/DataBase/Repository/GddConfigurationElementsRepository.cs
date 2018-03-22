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
            return this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == Id).ToList();
        }

        public List<GddConfigurationElements> FindAllChildren(Guid Id)
        {
            return this.Session.Query<GddConfigurationElements>().Where(w => w.ParentElement.Id == Id).OrderBy(o => o.PresentationOrder).ToList();
        }

        public GddConfigurationElements GetElementsByGddIdAndOrder(Guid Id, int order)
        {
            return this.Session.Query<GddConfigurationElements>().FirstOrDefault(w => w.GddConfig.Id == Id && w.PresentationOrder == order);
        }

        public GddConfigurationElements GetPreviousElementInOrder(GddConfigurationElements element)
        {
            return this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == element.GddConfig.Id 
                                                                          && w.PresentationOrder < element.PresentationOrder)
                                                                 .OrderByDescending(o => o.PresentationOrder).First();
        }

        public GddConfigurationElements GetNextElementInOrder(GddConfigurationElements element)
        {
            return this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == element.GddConfig.Id
                                                                          && w.PresentationOrder > element.PresentationOrder)
                                                                 .OrderBy(o => o.PresentationOrder).First();
        }

        public int GetMaxOrder(Guid Id)
        {
            var elements = this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == Id);

            return elements.Any() ? elements.Max(m => m.PresentationOrder) : 0;
        }
    }
}
