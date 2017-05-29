using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Models;
using NHibernate;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ISession session) : base(session) { }
    }
}
