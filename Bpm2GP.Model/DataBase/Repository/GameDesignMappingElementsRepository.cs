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
        public GameDesignMappingElementsRepository(ISession session) : base(session)
        {
        }

        public List<GameDesignMappingElements> FindFirstByModelId(String id)
        {
            return this.Session.Query<GameDesignMappingElements>().Where(w => w.ModelElementId.ToLower() == id.ToLower()).ToList();
        }
    }
}
