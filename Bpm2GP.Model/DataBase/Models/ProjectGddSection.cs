using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class ProjectGddSection
    {
        public virtual Guid Id { get; set; }
        public virtual String Title { get; set; }
        public virtual ProjectGdd ProjectGdd { get; set; }
        public virtual ProjectGddSection ParentSection { get; set; }
        public virtual DateTime DtHoraCadastro { get; set; }
        public virtual IList<ProjectGddSection> ChildrenSections { get; set; }
        public virtual IList<ProjectGddSectionContent> Contents { get; set; }

        public ProjectGddSection()
        {
            ChildrenSections = new List<ProjectGddSection>();
            Contents = new List<ProjectGddSectionContent>();
        }
    }

    public class ProjectGddSectionMap : ClassMapping<ProjectGddSection>
    {
        public ProjectGddSectionMap()
        {
            Table("projectgddsection");

            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Title);
            Property(x => x.DtHoraCadastro);

            ManyToOne(x => x.ParentSection, m =>
            {
                m.Column("idGddSection");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.ProjectGdd, m =>
            {
                m.Column("idProjectGdd");
                m.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.ChildrenSections, m =>
            {
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
                m.Key(k => k.Column("idGddSection"));
            }, 
            r => r.OneToMany());

            Bag(x => x.Contents, m =>
            {
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
                m.Key(k => k.Column("idGddSection"));
            },
            r => r.OneToMany());
        }
    }
}

