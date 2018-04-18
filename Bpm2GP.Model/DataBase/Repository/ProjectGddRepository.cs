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
    public class ProjectGddRepository : RepositoryBase<ProjectGdd>
    {
        public ProjectGdd FindFirstByProjectId(Guid Id)
        {
            try
            {
                return this.Session.Query<ProjectGdd>().FirstOrDefault(f => f.Project.Id == Id);
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
