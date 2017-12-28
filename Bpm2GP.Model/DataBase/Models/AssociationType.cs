using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class AssociationType
    {
        public virtual Int32 Id { get; set; }
        public virtual String Description { get; set; }
    }

    public class AssociationTypeMap : ClassMapping<AssociationType>
    {
        public AssociationTypeMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Description);
        }
    }
}
