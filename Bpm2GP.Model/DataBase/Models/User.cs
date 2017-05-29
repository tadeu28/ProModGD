using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System.ComponentModel.DataAnnotations;
using System.Management.Instrumentation;

namespace Bpm2GP.Model.DataBase.Models
{
    public class User
    {
        public virtual Guid Id { get; set; }
        [Required(ErrorMessage = "User name is required.")]
        public virtual string UserName { get; set; }
        [Required(ErrorMessage = "E-mail is required.")]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password is different of confirmation.")]
        [DataType(DataType.Password)]
        public virtual string PswConfirm { get; set; }
        public virtual Designer Designer { get; set; }
    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Guid);
            });

            Property(x => x.UserName);
            Property(x => x.Email);
            Property(x => x.Password);

            ManyToOne(customer => customer.Designer, map =>
            {
                map.Cascade(Cascade.All);
                map.Lazy(LazyRelation.Proxy);
                map.Column("IdDesigner");
            });
        }
    }
}
