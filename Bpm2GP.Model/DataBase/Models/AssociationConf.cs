using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class AssociationConf
    {
        public virtual Guid Id { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime DtCreation { get; set; }
        public virtual ModelingLanguage Language { get; set; }
        public virtual GameGenre Genre { get; set; }
        public virtual bool IsConstant { get; set; }

        public virtual IList<AssociationConfElements> ElementsAssociated { get; set; }

        public virtual String ToShow
        {
            get
            {
                if (Name.Trim() != null)
                {
                    return Name + " [" + Language.Name + " - " + Genre.Name + "]";
                }

                return "";
            }
        }

        public AssociationConf()
        {
            ElementsAssociated = new List<AssociationConfElements>();
        }

    }

    public class AssociationConfMap : ClassMapping<AssociationConf>
    {
        public AssociationConfMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Name);
            Property(x => x.DtCreation);
            Property(x => x.IsConstant);

            ManyToOne(x => x.Language, m =>
            {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("idLanguage");
            });
            ManyToOne(x => x.Genre, m => {
                m.Lazy(LazyRelation.NoLazy);
                m.Column("idGenre");
            });

            Bag(x => x.ElementsAssociated, m =>
            {
                m.Lazy(CollectionLazy.NoLazy);
                m.Key(k => k.Column("idAssociation"));
                m.Inverse(true);
                m.Cascade(Cascade.All);
            }, o => o.OneToMany());
        }
    }
}
