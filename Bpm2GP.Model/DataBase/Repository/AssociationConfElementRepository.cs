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
        public AssociationConfElementRepository(ISession session) : base(session) { }

        public List<AssociationConfElements> FindAllElementsByAssociantionAndLanguageElementId(Guid langElementId, Guid idAssociation)
        {
            return this.Session.Query<AssociationConfElements>().Where(w => w.ProcessElement.Id == langElementId && w.Association.Id == idAssociation).ToList();
        }

        public List<AssociationConfElements> FindAllElementsByAssociantionAndGameElementId(Guid gameElementId, Guid idAssociation)
        {
            return this.Session.Query<AssociationConfElements>().Where(w => w.GameGenreElement.Id == gameElementId && w.Association.Id == idAssociation).ToList();
        }

        public List<AssociationConfElements> FindAllElementsByAssociantion(Guid idAssociation)
        {
            return this.Session.Query<AssociationConfElements>().Where(w => w.Association.Id == idAssociation).ToList();
        }

        public List<AssociationConfElements> FindAllElementsByElementeMetamodel(String metamodel)
        {
            return this.Session.Query<AssociationConfElements>().Where(w => w.ProcessElement.Metamodel == metamodel).ToList();
        }
    }
}
