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
    public class ProjectRepository : RepositoryBase<Project>
    {
        public ProjectRepository(ISession session) : base(session) { }

        public Project FindFirstById(Guid id)
        {
            return this.Session.Query<Project>().FirstOrDefault(f => f.Id == id);
        }
    }
}
