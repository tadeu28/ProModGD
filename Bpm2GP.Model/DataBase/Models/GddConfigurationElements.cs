using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class GddConfigurationElements
    {
        public virtual Guid Id { get; set; }
        public virtual String Title { get; set; }
        public virtual GddConfiguration GddConfig { get; set; }
        public virtual GameGenreElement GameGenreElement { get; set; }
    }

    public class GddConfigurationElementsMap : ClassMapping<GddConfigurationElements>
    {
        public GddConfigurationElementsMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Title);

            ManyToOne(x => x.GddConfig, m =>
            {
                m.Column("idGddConfig");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.GameGenreElement, m => m.Column("idGenreElement"));
        }
    }
}
