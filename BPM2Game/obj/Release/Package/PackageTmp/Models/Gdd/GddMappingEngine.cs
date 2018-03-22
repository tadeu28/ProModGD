using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;

namespace BPM2Game.Models.Gdd
{
    public class GddMappingEngine
    {
        private static Project _project;
        private static GddConfiguration _gddConf;

        public static ProjectGdd ProcessGddBasedOnMapping(Project project, GddConfiguration gddConf)
        {
            try
            {
                _project = project;
                _gddConf = gddConf;

                //Cria o GDD
                var gdd = new ProjectGdd()
                {
                    CreationDate = DateTime.Now,
                    Project = project,
                    DesignerName = LoginUtils.User.Designer.Name,
                    BasedOnMapping = true
                };

                gdd = DbFactory.Instance.ProjectGddRepository.Save(gdd);

                var gddConfigSections =
                    DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(_gddConf.Id).Where(w => w.ParentElement == null).OrderBy(o => o.PresentationOrder).ToList();
                foreach (var element in gddConfigSections)
                {
                    CreateGddChapterAndSections(gdd, null, element);    
                }

                return gdd;
            }
            catch (Exception ex)
            {
                throw new Exception("It haven't been possible to generate the game design document.", ex);
            }
        }

        private static void CreateGddChapterAndSections(ProjectGdd gdd, ProjectGddSection parenteSection, GddConfigurationElements gddConfigElement)
        {
            try
            {
                var gddSection = new ProjectGddSection()
                {
                    ParentSection = parenteSection,
                    ProjectGdd = gdd, 
                    DtHoraCadastro = DateTime.Now,
                    Title = gddConfigElement.Title
                };

                gddSection = DbFactory.Instance.ProjectGddSectionRepository.Save(gddSection);

                CreateGddSectionContent(gddSection, gddConfigElement);

                var childElements =
                    DbFactory.Instance.GddConfigurationElementsRepository.FindAllChildren(gddConfigElement.Id);

                if (childElements != null && childElements.Count > 0)
                {
                    foreach (var element in childElements)
                    {
                        CreateGddChapterAndSections(gdd, gddSection, element);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("It haven't been possible to generate the chapters and sections of game design document.", ex);
            }
        }

        private static void CreateGddSectionContent(ProjectGddSection section, GddConfigurationElements confElement)
        {
            try
            {
                var mapping = DbFactory.Instance.DesignMappingRepository.FindFirstByProjectId(_project.Id);
                if (mapping != null)
                {
                    foreach (var genreElement in confElement.GameGenreElements)
                    {
                        var mappingElements =
                            mapping.GameDesignMappingElements.Where(w => w.GameGenreElement.Id == genreElement.Id)
                                .ToList();
                        foreach (var mappingContent in mappingElements)
                        {
                            var content = mappingContent.Descricao + 
                                            (!String.IsNullOrEmpty(mappingContent.ModelElementId) 
                                            ? "\n\n{small}*Relative to process object: " + mappingContent.ModelElementId + "{/small}"
                                            : "");

                            var sectionContent = new ProjectGddSectionContent()
                            {
                                Automatic = true,
                                Content = content,
                                GameGenreTitle = genreElement.Name,
                                Section = section
                            };

                            DbFactory.Instance.ProjectGddContentSectionRepository.Save(sectionContent);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("It haven't been possible to generate the section content of game design document.", ex);
            }
        }
    }
}