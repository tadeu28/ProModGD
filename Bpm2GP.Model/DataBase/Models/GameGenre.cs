using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class GameGenre
    {
        public virtual Guid Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual DateTime RegisterDate { get; set; }
        public virtual Boolean IsConstant { get; set; }
        public virtual bool Inactive { get; set; }
        public virtual Designer Designer { get; set; }
        public virtual IList<GameGenreElement>  Elements { get; set; }
        public virtual IList<Project> Projects { get; set; }
        public virtual IList<AssociationConf> Associations { get; set; }

        public GameGenre()
        {
            this.Elements = new List<GameGenreElement>();
            this.Associations = new List<AssociationConf>();
        }
    }

    public class GameGenreMap : ClassMapping<GameGenre>
    {
        public GameGenreMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Name);
            Property(x => x.Description);
            Property(x => x.RegisterDate);
            Property(x => x.IsConstant);
            Property(x => x.Inactive);

            ManyToOne(x => x.Designer, m =>
            {
                m.Column("idDesigner");
                m.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.Elements, m =>
            {
                m.Cascade(Cascade.DeleteOrphans);
                m.Inverse(true);
                m.Lazy(CollectionLazy.NoLazy);
                m.Key(k => k.Column("IdGenre"));
            }, o => o.OneToMany());

            Bag(x => x.Associations, m =>
            {
                m.Key(k => k.Column("idLanguage"));
                m.Inverse(true);
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.Lazy);
            }, o => o.OneToMany());

            Bag(x => x.Projects, m =>
            {
                m.Key(k => k.Column("idGameGenre"));
                m.Inverse(true);
                m.Cascade(Cascade.DeleteOrphans);
                m.Lazy(CollectionLazy.Lazy);
            }, o => o.OneToMany());
        }
    }
}
