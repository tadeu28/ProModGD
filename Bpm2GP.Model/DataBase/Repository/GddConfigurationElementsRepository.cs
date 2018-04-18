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
        public List<GddConfigurationElements> FindAllElementsByGddId(Guid Id)
        {
            try
            {
                return this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == Id).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<GddConfigurationElements> FindAllChildren(Guid Id)
        {
            try
            {
                return this.Session.Query<GddConfigurationElements>().Where(w => w.ParentElement.Id == Id).OrderBy(o => o.PresentationOrder).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public GddConfigurationElements GetElementsByGddIdAndOrder(Guid Id, int order)
        {
            try
            {
                return this.Session.Query<GddConfigurationElements>().FirstOrDefault(w => w.GddConfig.Id == Id && w.PresentationOrder == order);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public GddConfigurationElements GetPreviousElementInOrder(GddConfigurationElements element)
        {
            try
            {
                return this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == element.GddConfig.Id
                                                                          && w.PresentationOrder < element.PresentationOrder)
                                                                 .OrderByDescending(o => o.PresentationOrder).First();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public GddConfigurationElements GetNextElementInOrder(GddConfigurationElements element)
        {
            try
            {
                return this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == element.GddConfig.Id
                                                                          && w.PresentationOrder > element.PresentationOrder)
                                                                 .OrderBy(o => o.PresentationOrder).First();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public int GetMaxOrder(Guid Id)
        {
            try
            {
                var elements = this.Session.Query<GddConfigurationElements>().Where(w => w.GddConfig.Id == Id);

                return elements.Any() ? elements.Max(m => m.PresentationOrder) : 0;
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
