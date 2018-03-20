using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class ProjectGdd
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual String DesignerName { get; set; }
        public virtual Project Project { get; set; }
        public virtual IList<ProjectGddSection> Sections { get; set; }

        public ProjectGdd()
        {
            Sections = new List<ProjectGddSection>();
        }
    }

    public class ProjectGddMap : ClassMapping<ProjectGdd>
    {
        public ProjectGddMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.CreationDate);
            Property(x => x.DesignerName);

            ManyToOne(x => x.Project, m =>
            {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("idProject");
            });
            
            Bag(x => x.Sections, m =>
            {
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
                m.Key(k => k.Column("idProjectGdd"));
            },
            r => r.OneToMany());
        }
    }
}
