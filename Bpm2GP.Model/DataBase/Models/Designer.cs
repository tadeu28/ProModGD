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
        public virtual String ShortBio { get; set; }
        public virtual User User { get; set; }
        public virtual IList<Project>  Projects { get; set; }
        public virtual IList<ModelingLanguage> Languages { get; set; }
        public virtual IList<GameGenre> GameGenres { get; set; }

        public Designer()
        {
            this.Projects = new List<Project>();
            this.Languages = new List<ModelingLanguage>();
            this.GameGenres = new List<GameGenre>();
        }
    }

    public class DesignerMap : ClassMapping<Designer>
    {
        public DesignerMap()
        {
            Table("designer");

            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Guid);
            });

            Property(x => x.Name);
            Property(x => x.ShortBio);

            OneToOne(x => x.User, map =>
            {
                map.PropertyReference(typeof (User).GetProperty("Designer"));
                map.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.Projects, map =>
            {
                map.Cascade(Cascade.None);
                map.Lazy(CollectionLazy.NoLazy);
                map.Key(k => k.Column("idDesigner"));
                //map.Table("designers");
            },
            o => o.ManyToMany(p => p.Column("idProject")));

            Bag<ModelingLanguage>(x => x.Languages, m =>
            {
                m.Key(k => k.Column("idDesigner"));
                m.Inverse(true);
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.NoLazy);
            }, o => o.OneToMany());

            Bag<GameGenre>(x => x.GameGenres, m =>
            {
                m.Key(k => k.Column("idDesigner"));
                m.Inverse(true);
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.NoLazy);
            }, o => o.OneToMany());
        }
    }
}
