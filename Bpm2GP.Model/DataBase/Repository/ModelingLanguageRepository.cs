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
    public class ModelingLanguageRepository : RepositoryBase<ModelingLanguage>
    {
        public ModelingLanguageRepository(ISession session) : base(session) { }
        
        public List<ModelingLanguage> FindAllLanguagesByDesigner(Designer designer)
        {
            return this.Session.Query<ModelingLanguage>().Where(w => w.IsConstant ||
                                                                     w.Designer == designer)
                                                         .ToList();
        }

        public List<ModelingLanguage> FindAllLanguagesByDesignerAndOrderDesc(Designer designer)
        {
            return this.Session.Query<ModelingLanguage>().Where(w => w.IsConstant ||
                                                                     w.Designer == designer)
                                                         .OrderByDescending(o => o.RegisterDate)
                                                         .ToList();
        }
    }
}
