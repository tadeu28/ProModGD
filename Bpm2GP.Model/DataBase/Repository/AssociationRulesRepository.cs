using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Models;
using NHibernate;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class AssociationRulesRepository : RepositoryBase<AssociationRules>
    {
        public AssociationRulesRepository(ISession session) : base(session) { }
    }
}
