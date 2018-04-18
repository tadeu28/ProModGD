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
        public List<ProjectSolicitation> FindByDesigner(Guid idDesigner)
        {
            try
            {
                return this.Session.Query<ProjectSolicitation>().Where(f => f.Designer.Id == idDesigner).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<ProjectSolicitation> FindByProject(Guid idProject)
        {
            try
            {
                return this.Session.Query<ProjectSolicitation>().Where(f => f.Project.Id == idProject).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<ProjectSolicitation> FindByProjectAndDesigner(Guid idProject, Guid idDesigner)
        {
            try
            {
                return this.Session.Query<ProjectSolicitation>().Where(f => f.Project.Id == idProject && f.Designer.Id == idDesigner).ToList();
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
