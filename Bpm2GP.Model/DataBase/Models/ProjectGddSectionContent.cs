using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class ProjectGddSectionContent 
    {
        public virtual Guid Id { get; set; }
        public virtual String Content { get; set; }
        public virtual String GameGenreTitle { get; set; }
        public virtual Boolean Automatic { get; set; }
        public virtual ProjectGddSection Section { get; set; }

    }

    public class ProjectGddSectionContentMap : ClassMapping<ProjectGddSectionContent>
    {
        public ProjectGddSectionContentMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.GameGenreTitle);
            Property(x => x.Automatic);

            Property(x => x.Content, m =>
            {
                m.Type(NHibernateUtil.StringClob);
                m.Column(c => c.SqlType("LONGTEXT"));
            });

            ManyToOne(x => x.Section, m =>
            {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("idGddSection");
            });
        }
    }
}
