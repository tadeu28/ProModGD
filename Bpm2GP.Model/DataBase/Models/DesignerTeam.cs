using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class DesignTeam
    {
        public virtual Guid Id { get; set; }
        public virtual Designer Designer { get; set; }
        public virtual Project Project { get; set; }
    }

    public class DesignTeamMap : ClassMapping<DesignTeam>
    {
        public DesignTeamMap()
        {
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Guid);
            });

            ManyToOne<Designer>(x => x.Designer, m =>
            {
                m.Column("IdDesigner");
                m.Lazy(LazyRelation.NoLazy);
                m.Cascade(Cascade.All);
            });

            ManyToOne<Project>(x => x.Project, m =>
            {
                m.Column("IdProject");
                m.Lazy(LazyRelation.NoLazy);
                m.Cascade(Cascade.All);
            });
        }
    }
}
