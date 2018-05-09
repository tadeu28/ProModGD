using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class Project
    {
        public virtual Guid Id { get; set; }
        public virtual String Title { get; set; }
        public virtual String Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public virtual DateTime LastUpdate { get; set; }
        public virtual String BpmnModelPath { get; set; }
        public virtual byte[] BpmnModel { get; set; }
        public virtual bool Inactive { get; set; }
        public virtual Designer Owner { get; set; }
        public virtual GameGenre GameGenre { get; set; }
        public virtual ProjectGdd ProjectGdd { get; set; }
        public virtual IList<DesignMapping> DesignMappings { get; set; }
        public virtual IList<Designer> Designers { get; set; }
        public virtual IList<ProjectFile> Files { get; set; }

        public Project()
        {
            this.Designers = new List<Designer>();
            DesignMappings = new List<DesignMapping>();
            Files = new List<ProjectFile>();
        }
    }

    public class ProjectMap : ClassMapping<Project>
    {
        public ProjectMap()
        {
            Table("project");

            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Guid);
            });

            Property(x=> x.Title);
            Property(x => x.Description, m =>
            {
                m.Type(NHibernateUtil.StringClob);
                m.Column(c => c.SqlType("LONGTEXT"));
            });
            Property(x => x.StartDate);
            Property(x => x.LastUpdate);
            Property(x => x.Inactive);
            Property(x => x.BpmnModelPath);
            Property(x => x.BpmnModel);

            Bag(x => x.DesignMappings, map =>
            {
                map.Cascade(Cascade.DeleteOrphans);
                map.Lazy(CollectionLazy.NoLazy);
                map.Key(k => k.Column("IdProject"));
            },
            o => o.OneToMany());

            ManyToOne(x => x.Owner, m =>
            {
                m.Column("idDesigner");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.GameGenre, m =>
            {
                m.Column("idGameGenre");
                m.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.Designers, map =>
            {
                map.Cascade(Cascade.None);
                map.Lazy(CollectionLazy.NoLazy);
                map.Key(k => k.Column("idProject"));
                //map.Table("designers");
            }, 
            o => o.ManyToMany(p => p.Column("idDesigner")));

            
            Bag(x => x.Files, map =>
            {
                map.Cascade(Cascade.DeleteOrphans);
                map.Lazy(CollectionLazy.NoLazy);
                map.Key(k => k.Column("idProject"));
            },
            o => o.OneToMany());

            OneToOne(x => x.ProjectGdd, m =>
            {
                m.PropertyReference(typeof(ProjectGdd).GetProperty("Project"));
                m.Lazy(LazyRelation.NoLazy);
            });
        }   
    }
}
