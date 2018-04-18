using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class DesignMappingScores
    {
        public virtual Guid Id { get; set; }
        public virtual String GameGenreElement { get; set; }
        public virtual Guid GameGenreElementId { get; set; }
        public virtual String ModelElement { get; set; }
        public virtual Guid ModelElementId { get; set; }
        public virtual Decimal ExpectedElements { get; set; }
        public virtual Decimal MappedElements { get; set; }
        public virtual DesignMapping DesignMapping { get; set; }
        public virtual Decimal Score => (ExpectedElements > 0 ? (MappedElements / ExpectedElements) : 1);
    }

    public class DesignMappingScoresMap : ClassMapping<DesignMappingScores>
    {
        public DesignMappingScoresMap()
        {
            Table("designmappingscores");

            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.GameGenreElement);
            Property(x => x.GameGenreElementId);
            Property(x => x.ModelElement);
            Property(x => x.ModelElementId);
            Property(x => x.ExpectedElements);
            Property(x => x.MappedElements);

            ManyToOne(x => x.DesignMapping, m =>
            {
                m.Column("idDesignMapping");
                m.Lazy(LazyRelation.NoLazy);
            });
        }
    }
}
