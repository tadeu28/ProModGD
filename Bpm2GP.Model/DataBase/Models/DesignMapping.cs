using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode.Impl;

namespace Bpm2GP.Model.DataBase.Models
{
    public class DesignMapping
    {
        public virtual Guid Id { get; set; }
        public virtual Project Project { get; set; }
        public virtual AssociationConf AssociationConf { get; set; }
        public virtual ModelingLanguage Language { get; set; }
        public virtual GameGenre Genre { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual Decimal ModelScore { get; set; }
        public virtual IList<GameDesignMappingElements> GameDesignMappingElements { get; set; }
        public virtual IList<DesignMappingScores>  GameMappingScores { get; set; }
        public virtual IList<DesignMappingErrors> DesignMappingErrors { get; set; }

        public DesignMapping()
        {
            this.GameDesignMappingElements = new List<GameDesignMappingElements>();
            this.GameMappingScores = new List<DesignMappingScores>();
            this.DesignMappingErrors = new List<DesignMappingErrors>();
        }
    }

    public class DesignMappingMap : ClassMapping<DesignMapping>
    {
        public DesignMappingMap()
        {
            Table("designmapping");

            Id(x=> x.Id, map => map.Generator(Generators.Guid));

            Property(x => x.CreationDate);
            Property(x => x.ModelScore);

            ManyToOne(x => x.Project, m =>
            {
                m.Column("IdProject");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.AssociationConf, m =>
            {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("IdAssociationConf");
            });

            Bag(x => x.GameDesignMappingElements, m =>
            {
                m.Cascade(Cascade.All);
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
                m.Key(k => k.Column("idDesignMapping"));
            }, o => o.OneToMany());

            Bag(x => x.GameMappingScores, m =>
            {
                m.Cascade(Cascade.All);
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
                m.Key(k => k.Column("idDesignMapping"));
            }, o => o.OneToMany());

            Bag(x => x.DesignMappingErrors, m =>
            {
                m.Cascade(Cascade.All);
                m.Lazy(CollectionLazy.NoLazy);
                m.Inverse(true);
                m.Key(k => k.Column("idDesignMapping"));
            }, o => o.OneToMany());

        }
    }
}
