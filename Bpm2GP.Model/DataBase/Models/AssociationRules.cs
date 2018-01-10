using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public enum AssociationRuleOperator
    {
        None,
        Exists,
        Equals,
        NotEquals,
        Contains,
        Starts,
        Finishes,
        HaveSomeContent,
        MoreThan,
        LessThan,
        Before,
        After,
        NotExists
    }

    public class AssociationRules
    {
        public virtual Guid Id { get; set; }
        public virtual AssociationType Type { get; set; }
        public virtual String Field { get; set; }
        public virtual String Rule { get; set; }
        public virtual AssociationRuleOperator Operator { get; set; }
        public virtual AssociationConfElements AssociationElement { get; set; }
    }

    public class AssociationRulesMap : ClassMapping<AssociationRules>
    {
        public AssociationRulesMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Field);
            Property(x => x.Operator);
            Property(x => x.Rule);

            ManyToOne(x => x.Type, m =>
            {
                m.Column("idType");
                //m.Class(typeof(AssociationType));
                //m.Fetch(FetchKind.Join);
            });
            ManyToOne(x => x.AssociationElement, m =>
            {
                m.Column("idAssocElement");
            });
        }
    }

}
