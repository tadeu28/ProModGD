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
    public class GameDesignMappingElementsRepository : RepositoryBase<GameDesignMappingElements>
    {

        public List<GameDesignMappingElements> FindFirstByModelId(String id)
        {
            try
            {
                return this.Session.Query<GameDesignMappingElements>().Where(w => w.ModelElementId.ToLower() == id.ToLower()).ToList();
            }
            finally
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
        }

        public List<GameDesignMappingElements> FindFirstByModelIdAndProjectId(String id, String projectId)
        {
            try
            {
                return this.Session.Query<GameDesignMappingElements>().Where(w => w.ModelElementId.ToLower() == id.ToLower() &&
                                                                         w.DesignMapping.Project.Id.ToString() == projectId).ToList();
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
