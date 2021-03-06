﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Bpm2GP.Model.DataBase.Repository
{
    public class DesignMappingRepository : RepositoryBase<DesignMapping>
    {

        public DesignMapping FindFirstByProjectId(Guid id)
        {
            try
            {
                return this.Session.Query<DesignMapping>().FirstOrDefault(f => f.Project.Id == id);
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
