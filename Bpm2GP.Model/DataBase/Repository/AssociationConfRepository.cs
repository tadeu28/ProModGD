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
    public class AssociationConfRepository : RepositoryBase<AssociationConf>
    {
        public AssociationConfRepository(ISession session) : base(session)
        {
        }

        public List<AssociationConf> FindAllElementsByGenreAndLanguage(Guid genreId, Guid languageId)
        {
            return this.Session.Query<AssociationConf>().Where(w => w.Genre.Id == genreId && w.Language.Id == languageId).ToList();
        }
    }
}
