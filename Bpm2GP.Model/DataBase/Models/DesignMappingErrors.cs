using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class DesignMappingErrors
    {
        public virtual Guid Id { get; set; }
        public virtual Exception Error { get; set; }
        public virtual DesignMapping DesignMapping { get; set; }
    }

    public class DesignMappingErrorsMap : ClassMapping<DesignMappingErrors>
    {
        public DesignMappingErrorsMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Error);

            ManyToOne(x => x.DesignMapping, m =>
            {
                m.Column("idDesignMapping");
            });
        }
    }
}
