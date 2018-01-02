using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class GameDesignMappingElements
    {
        public virtual Guid Id { get; set; }
        public virtual String Descricao { get; set; }
        public virtual DesignMapping DesignMapping { get; set; }
        public virtual AssociationConfElements AssociateElement { get; set; }
        public virtual GameGenreElement GameGenreElement { get; set; }
        public virtual String ModelElementId { get; set; }
        public virtual bool IsManual { get; set; }
    }

    public class GameDesignMappingElementsMap : ClassMapping<GameDesignMappingElements>
    {
        public GameDesignMappingElementsMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Descricao);
            Property(x => x.IsManual);
            Property(x => x.ModelElementId);

            ManyToOne(x => x.GameGenreElement, m =>
            {
                m.Column("idGenreElement");
            });

            ManyToOne(x => x.AssociateElement, m =>
            {
                m.Column("idAssocElement");
            });

            ManyToOne(x => x.DesignMapping, m =>
            {
                m.Column("idDesignMapping");
            });
        }
    }
}
