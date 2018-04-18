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
    public class ProjectGddSectionRepository : RepositoryBase<ProjectGddSection>
    {
        

        public ProjectGddSection FindFirstByProjectId(Guid Id)
        {
            try
            {
                return this.Session.Query<ProjectGddSection>().FirstOrDefault(f => f.ProjectGdd.Id == Id);
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public IList<ProjectGddSection> FindAllByProjectId(Guid Id)
        {
            try
            {
                return this.Session.Query<ProjectGddSection>().Where(f => f.ProjectGdd.Id == Id).OrderBy(o => o.Title).ToList();
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
