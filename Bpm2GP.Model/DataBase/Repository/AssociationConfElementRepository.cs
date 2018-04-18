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
    public class AssociationConfElementRepository : RepositoryBase<AssociationConfElements>
    {
        public List<AssociationConfElements> FindAllElementsByAssociantionAndLanguageElementId(Guid langElementId, Guid idAssociation)
        {
            try
            {
                return this.Session.Query<AssociationConfElements>().Where(w => w.ProcessElement.Id == langElementId && w.Association.Id == idAssociation).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<AssociationConfElements> FindAllElementsByAssociantionAndGameElementId(Guid gameElementId, Guid idAssociation)
        {
            try
            {
                return this.Session.Query<AssociationConfElements>().Where(w => w.GameGenreElement.Id == gameElementId && w.Association.Id == idAssociation).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<AssociationConfElements> FindAllElementsByAssociantion(Guid idAssociation)
        {
            try
            {
                return this.Session.Query<AssociationConfElements>().Where(w => w.Association.Id == idAssociation).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<AssociationConfElements> FindAllElementsByElementeMetamodel(String metamodel)
        {
            try
            {
                return this.Session.Query<AssociationConfElements>().Where(w => w.ProcessElement.Metamodel == metamodel).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<AssociationConfElements> FindAllElementsByElements(List<ModelingLanguageElement> elements)
        {
            try
            {
                return this.Session.Query<AssociationConfElements>().Where(w => elements.Contains(w.ProcessElement)).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public AssociationConfElements FindByElementMetamodel(String metamodel)
        {
            try
            {
                return this.Session.Query<AssociationConfElements>().FirstOrDefault(w => w.ProcessElement.Metamodel == metamodel);
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
