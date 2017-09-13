using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class ModelingLanguageElement
    {
        public virtual Guid Id { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual String Metamodel { get; set; }
        public virtual ModelingLanguageElement ParentElement { get; set; }
        public virtual IList<ModelingLanguageElement> ChildElements { get; set; }
        public virtual ModelingLanguage Language { get; set; }

        public ModelingLanguageElement()
        {
            this.ChildElements = new List<ModelingLanguageElement>();
        }
    }

    public class ModelingLanguageElementMap : ClassMapping<ModelingLanguageElement>
    {
        public ModelingLanguageElementMap()
        {
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Guid);
            });

            Property(x => x.Name);
            Property(x => x.Description);
            Property(x => x.Metamodel);

            ManyToOne(x => x.Language, m =>
            {
                m.Column("idLanguage");
                m.Lazy(LazyRelation.Proxy);
                m.Cascade(Cascade.All);
            });

            ManyToOne(x => x.ParentElement, m =>
            {
                m.Column("idElement");
                m.Lazy(LazyRelation.Proxy);
                m.Cascade(Cascade.All);
            });

            Bag(x => x.ChildElements, m =>
            {
                m.Cascade(Cascade.All);
                m.Inverse(true);
                m.Lazy(CollectionLazy.Lazy);
                m.Key(k => k.Column("idElement"));
            }, o => o.OneToMany());
        }
    }
}
