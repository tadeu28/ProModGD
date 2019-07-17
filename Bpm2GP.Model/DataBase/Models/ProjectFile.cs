using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class ProjectFile
    {
        public virtual Guid Id { get; set; }
        [Required(ErrorMessage = "Title field is required.")]
        public virtual String Title { get; set; }
        [Required(ErrorMessage = "Description field is required.")]
        public virtual String Description { get; set; }
        [Required(ErrorMessage = "File field is required.")]
        public virtual String FileName { get; set; }
        public virtual Project Project { get; set; }
        public virtual DateTime DtPersistence { get; set; }
    }

    public class ProjectFileMap : ClassMapping<ProjectFile>
    {
        public ProjectFileMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Title);
            Property(x => x.Description, m =>
            {
                m.Type(NHibernateUtil.StringClob);
                m.Column(c => c.SqlType("LONGTEXT"));
            });

            Property(x => x.DtPersistence);
            Property(x => x.FileName);

            ManyToOne(x => x.Project, m =>
            {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("idProject");
            });
        }
    }
}
