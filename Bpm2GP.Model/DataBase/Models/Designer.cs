using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class Designer
    {
        public virtual Guid Id { get; set; }
        public virtual String Name { get; set; }
        public virtual User User { get; set; }
    }

    public class DesignerMap : ClassMapping<Designer>
    {
        public DesignerMap()
        {
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Guid);
            });

            Property(x => x.Name);
            
            OneToOne(x => x.User, map =>
            {
                map.PropertyReference(typeof (User).GetProperty("Designer"));
                map.Lazy(LazyRelation.Proxy);
            });
        }
    }
}
