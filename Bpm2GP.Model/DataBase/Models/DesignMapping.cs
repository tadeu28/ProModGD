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
        public virtual IList<GameDesignMappingElements> GameDesignMappingElements { get; set; }
        public virtual IList<DesignMappingScores>  GameMappingScores { get; set; }

        public DesignMapping()
        {
            this.GameDesignMappingElements = new List<GameDesignMappingElements>();
            this.GameMappingScores = new List<DesignMappingScores>();
        }
    }

    public class DesignMappingMap : ClassMapping<DesignMapping>
    {
        public DesignMappingMap()
        {
            Id(x=> x.Id, map => map.Generator(Generators.Guid));

            ManyToOne(x => x.Project, m =>
            {
                m.Column("IdProject");
            });

            ManyToOne(x => x.AssociationConf, m =>
            {
                m.Column("IdAssociationConf");
            });

            Bag(x => x.GameDesignMappingElements, m =>
            {
                m.Cascade(Cascade.All);
                m.Lazy(CollectionLazy.Lazy);
                m.Inverse(true);
                m.Key(k => k.Column("idDesignMapping"));
            }, o => o.OneToMany());

            Bag(x => x.GameMappingScores, m =>
            {
                m.Cascade(Cascade.All);
                m.Lazy(CollectionLazy.Lazy);
                m.Inverse(true);
                m.Key(k => k.Column("idDesignMapping"));
            }, o => o.OneToMany());

            Property(x => x.CreationDate);
        }
    }
}
