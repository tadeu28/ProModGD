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
            Id(x => x.Id, m => m.Generator(Generators.Guid));
            
            ManyToOne(x => x.ProcessElement, m => m.Column("idLanguageElement"));
            ManyToOne(x => x.GameGenreElement, m => m.Column("idGenreElement"));
            ManyToOne(x => x.Association, m => m.Column("idAssociation"));

            Bag(x => x.Ruleses, m =>
            {
                m.Key(k => k.Column("idAssocElement"));
                m.Cascade(Cascade.All);
                m.Lazy(CollectionLazy.Lazy);
                m.Inverse(true);
            }, o => o.OneToMany());
        }
    }
}
