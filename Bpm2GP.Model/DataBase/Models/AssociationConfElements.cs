using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class AssociationConfElements
    {
        public virtual Guid Id { get; set; }
        public virtual ModelingLanguageElement ProcessElement { get; set; }
        public virtual GameGenreElement GameGenreElement { get; set; }
        public virtual AssociationConf Association { get; set; }
        public virtual Boolean Inactive { get; set; }
        public virtual IList<AssociationRules> Ruleses { get; set; }

        public AssociationConfElements()
        {
            this.Ruleses = new List<AssociationRules>();
        }
    }

    public class AssociationConfElementsMap : ClassMapping<AssociationConfElements>
    {
        public AssociationConfElementsMap()
        {
            Table("associationconfelements");

            Id(x => x.Id, m => m.Generator(Generators.Guid));
            Property(x => x.Inactive);
            
            ManyToOne(x => x.ProcessElement, m =>
            {
                m.Column("idLanguageElement");
                m.Lazy(LazyRelation.NoLazy);
            });
            ManyToOne(x => x.GameGenreElement, m =>
            {
                m.Column("idGenreElement");
                m.Lazy(LazyRelation.NoLazy);
            });
            ManyToOne(x => x.Association, m =>
            {
                m.Column("idAssociation");
                m.Lazy(LazyRelation.NoLazy);
            });

            Bag(x => x.Ruleses, m =>
            {
                m.Lazy(CollectionLazy.NoLazy);
                m.Key(k => k.Column("idAssocElement"));
                m.Cascade(Cascade.All);
                m.Inverse(true);
            }, o => o.OneToMany());
        }
    }
}
