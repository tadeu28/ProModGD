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
    public class GddConfigurationElements
    {
        public virtual Guid Id { get; set; }
        public virtual String Title { get; set; }
        public virtual String Description { get; set; }
        public virtual int PresentationOrder { get; set; }
        public virtual GddConfiguration GddConfig { get; set; }
        public virtual GddConfigurationElements ParentElement { get; set; }
        public virtual IList<GddConfigurationElements> ChildElements { get; set; }
        public virtual IList<GameGenreElement> GameGenreElements { get; set; }

        public GddConfigurationElements()
        {
            GameGenreElements = new List<GameGenreElement>();
        }
    }

    public class GddConfigurationElementsMap : ClassMapping<GddConfigurationElements>
    {
        public GddConfigurationElementsMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Title);
            Property(x => x.Description, m =>
            {
                m.Type(NHibernateUtil.StringClob);
                m.Column(c => c.SqlType("LONGTEXT"));
            });
            Property(x => x.PresentationOrder);

            ManyToOne(x => x.GddConfig, m =>
            {
                m.Column("idGddConfig");
                m.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.GameGenreElements, map =>
            {
                map.Cascade(Cascade.None);
                map.Lazy(CollectionLazy.NoLazy);
                map.Key(k => k.Column("idGddElement"));
            },
            o => o.ManyToMany(p => p.Column("idGameGenre")));

            ManyToOne(x => x.ParentElement, m =>
            {
                m.Column("idGddElement");
                m.Lazy(LazyRelation.Proxy);
            });

            Bag(x => x.ChildElements, m =>
            {
                m.Cascade(Cascade.DeleteOrphans);
                m.Inverse(true);
                m.Lazy(CollectionLazy.Lazy);
                m.Key(k => k.Column("idGddElement"));
            }, o => o.OneToMany());
        }
    }
}
