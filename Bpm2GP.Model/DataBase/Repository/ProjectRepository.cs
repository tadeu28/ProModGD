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
        
        public List<Project> FindByDesigner(Guid idDesigner)
        {
            return this.Session.Query<Project>().Where(f => f.DesignTeam.Designer.Id == idDesigner).ToList();
        }
    }
}
