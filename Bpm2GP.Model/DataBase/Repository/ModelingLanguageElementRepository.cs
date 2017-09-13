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
    public class ModelingLanguageElementRepository : RepositoryBase<ModelingLanguageElement>
    {
        public ModelingLanguageElementRepository(ISession session) : base(session) { }

        public List<ModelingLanguageElement> FindAllElementsByLanguageId(Guid languageId)
        {
            return this.Session.Query<ModelingLanguageElement>().Where(w => w.Language.Id == languageId).ToList();
        }
    }
}
