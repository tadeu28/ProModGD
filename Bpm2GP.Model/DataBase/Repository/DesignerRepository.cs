using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Models;
using NHibernate;
using NHibernate.Linq;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class DesignerRepository : RepositoryBase<Designer>
    {
        public Designer FindByUserId(Guid id)
        {
            try
            {
                return this.Session.Query<Designer>().FirstOrDefault(w => w.User.Id == id);
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
