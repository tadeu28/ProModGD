using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class ProjectSolicitation
    {
        public virtual Guid Id { get; set; }
        public virtual Project Project { get; set; }
        public virtual Designer Designer { get; set; }
        public virtual DateTime Date { get; set; }
    }
    public class ProjectSolicitationMap : ClassMapping<ProjectSolicitation>
    {
        public ProjectSolicitationMap()
        {
            Id(x =>x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Date);

            ManyToOne(x => x.Designer, m => m.Column("idDesigner"));
            ManyToOne(x => x.Project, m => m.Column("idProject"));
        }
    }
}

