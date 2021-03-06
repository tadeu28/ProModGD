﻿using System;
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
        public List<ModelingLanguageElement> FindAllElementsByLanguageId(Guid languageId, bool isInactive)
        {
            try
            {
                return this.Session.Query<ModelingLanguageElement>().Where(w => w.Language.Id == languageId && w.Inactive == isInactive).ToList();
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
