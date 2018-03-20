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
        public ProjectGddSectionRepository(ISession session) : base(session) { }

        public ProjectGddSection FindFirstByProjectId(Guid Id)
        {
            return this.Session.Query<ProjectGddSection>().FirstOrDefault(f => f.ProjectGdd.Id == Id);
        }

        public IList<ProjectGddSection> FindAllByProjectId(Guid Id)
        {
            return this.Session.Query<ProjectGddSection>().Where(f => f.ProjectGdd.Id == Id).OrderBy(o => o.Title).ToList();
        }
    }
}
