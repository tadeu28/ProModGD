using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class Version
    {
        public virtual Guid Id { get; set; }
        public virtual Int32 VersionNumber { get; set; }
    }

    public class VersionMap : ClassMapping<Version>
    {
        public VersionMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));
            Property(x => x.VersionNumber);
        }
    }
}
