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
        public ProjectGddRepository(ISession session) : base(session) { }

        public ProjectGdd FindFirstByProjectId(Guid Id)
        {
            return this.Session.Query<ProjectGdd>().FirstOrDefault(f => f.Project.Id == Id);
        }

    }
}
