using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class GddConfiguration
    {
        public virtual Guid Id { get; set; }
        public virtual String Title { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime RegistrationDate { get; set; }
        public virtual Boolean IsConstant { get; set; }
        public virtual bool Inactive { get; set; }
        public virtual Designer Designer { get; set; }
        public virtual GameGenre GameGenre { get; set; }
        public virtual IList<GddConfigurationElements> GddElements { get; set; }

        public GddConfiguration()
        {
            this.GddElements = new List<GddConfigurationElements>();
        }
    }

    public class GddConfigurationMap : ClassMapping<GddConfiguration>
    {
        public GddConfigurationMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Title);
            Property(x => x.RegistrationDate);
            Property(x => x.Inactive);
            Property(x => x.IsConstant);

            ManyToOne(x => x.Designer, m =>
            {
                m.Column("idDesigner");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.GameGenre, m =>
            {
                m.Column("idGameGenre");
                m.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.GddElements, m =>
            {
                m.Key(k => k.Column("idGddConfig"));
                m.Cascade(Cascade.DeleteOrphans);
                m.Inverse(true);
                m.Lazy(CollectionLazy.Lazy);
            }, o => o.OneToMany());
        }
    }
}
