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
    public class ProjectGddContentSectionRepository : RepositoryBase<ProjectGddSectionContent>
    {
        public ProjectGddContentSectionRepository(ISession session) : base(session) { }
    }
}
