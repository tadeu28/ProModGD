using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Models;
using NHibernate;
using NHibernate.Linq;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class UserRepository : RepositoryBase<User>
    {
        public User Login(string senha, string usuario)
        {
            try
            {
                return this.Session.Query<User>().FirstOrDefault(w => w.Password == senha &&
                                                                      (w.Email == usuario || w.UserName == usuario));
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }
    }
}
