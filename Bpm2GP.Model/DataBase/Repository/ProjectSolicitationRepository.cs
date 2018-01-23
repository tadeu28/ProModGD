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
    public class ProjectSolicitationRepository : RepositoryBase<ProjectSolicitation>
    {
        public ProjectSolicitationRepository(ISession session) : base(session) { }

        public List<ProjectSolicitation> FindByDesigner(Guid idDesigner)
        {
            return this.Session.Query<ProjectSolicitation>().Where(f => f.Designer.Id == idDesigner).ToList();
        }

        public List<ProjectSolicitation> FindByProject(Guid idProject)
        {
            return this.Session.Query<ProjectSolicitation>().Where(f => f.Project.Id == idProject).ToList();
        }

        public List<ProjectSolicitation> FindByProjectAndDesigner(Guid idProject, Guid idDesigner)
        {
            return this.Session.Query<ProjectSolicitation>().Where(f => f.Project.Id == idProject && f.Designer.Id == idDesigner).ToList();
        }
    }
}
