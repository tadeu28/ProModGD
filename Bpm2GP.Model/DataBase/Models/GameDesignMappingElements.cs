using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Bpm2GP.Model.DataBase.Models
{
    public class GameDesignMappingElements
    {
        public virtual Guid Id { get; set; }
        [Required(ErrorMessage = "Description field is required.")]
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
            Table("gamedesignmappingelements");

            Id(x => x.Id, m => m.Generator(Generators.Guid));

            Property(x => x.Descricao, m =>
            {
                m.Type(NHibernateUtil.StringClob);
                m.Column(c => c.SqlType("LONGTEXT"));
            });
            Property(x => x.IsManual);
            Property(x => x.ModelElementId);

            ManyToOne(x => x.GameGenreElement, m =>
            {
                m.Column("idGenreElement");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.AssociateElement, m =>
            {
                m.Column("idAssocElement");
                m.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.DesignMapping, m =>
            {
                m.Column("idDesignMapping");
                m.Lazy(LazyRelation.NoLazy);
            });
        }
    }
}
