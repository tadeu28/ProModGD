using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.DataBase.Repository;
using IniParser;
using IniParser.Model;
using MySql.Data.MySqlClient;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Mapping.ByCode;

namespace Bpm2GP.Model.DataBase
{
    public class DbFactory
    {
        public UserRepository UserRepository { get; set; }
        public DesignerRepository DesignerRepository { get; set; }
        public ProjectRepository ProjectRepository { get; set; }
        public ModelingLanguageRepository ModelingLanguageRepository { get; set; }
        public ModelingLanguageElementRepository ModelingLanguageElementRepository { get; set; }
        public GameGenreRepository GameGenreRepository { get; set; }
        public GameGenreElementRepository GameGenreElementRepository { get; set; }
        public AssociationConfRepository AssociationConfRepository { get; set; }
        public AssociationConfElementRepository AssociationConfElementRepository { get; set; }
        public DesignMappingRepository DesignMappingRepository { get; set; }
        public GameDesignMappingElementsRepository GameDesignMappingElementsRepository { get; set; }
        public AssociationTypeRepository AssociationTypeRepository { get; set; }
        public AssociationRulesRepository AssociationRulesRepository { get; set; }
        public ProjectSolicitationRepository ProjectSolicitationRepository { get; set; }
        public GddConfigurationRepository GddConfigurationRepository { get; set; }
        public GddConfigurationElementsRepository GddConfigurationElementsRepository { get; set; }
        public ProjectGddRepository ProjectGddRepository { get; set; }
        public ProjectGddSectionRepository ProjectGddSectionRepository { get; set; }
        public ProjectGddContentSectionRepository ProjectGddContentSectionRepository { get; set; }
        public ProjectFileRepository ProjectFileRepository { get; set; }

        public VersionRepository VersionRepository { get; set; }

        public DbFactory()
        {
            UserRepository = new UserRepository();
            DesignerRepository = new DesignerRepository();
            ProjectRepository = new ProjectRepository();
            ModelingLanguageRepository = new ModelingLanguageRepository();
            ModelingLanguageElementRepository = new ModelingLanguageElementRepository();
            GameGenreRepository = new GameGenreRepository();
            GameGenreElementRepository = new GameGenreElementRepository();
            AssociationConfRepository = new AssociationConfRepository();
            AssociationConfElementRepository = new AssociationConfElementRepository();
            DesignMappingRepository = new DesignMappingRepository();
            GameDesignMappingElementsRepository = new GameDesignMappingElementsRepository();
            AssociationTypeRepository = new AssociationTypeRepository();
            AssociationRulesRepository = new AssociationRulesRepository();
            ProjectSolicitationRepository = new ProjectSolicitationRepository();
            GddConfigurationRepository = new GddConfigurationRepository();
            GddConfigurationElementsRepository = new GddConfigurationElementsRepository();
            ProjectGddRepository = new ProjectGddRepository();
            ProjectGddSectionRepository = new ProjectGddSectionRepository();
            ProjectGddContentSectionRepository = new ProjectGddContentSectionRepository();
            VersionRepository = new VersionRepository();
            ProjectFileRepository = new ProjectFileRepository();
        }
    }
}
