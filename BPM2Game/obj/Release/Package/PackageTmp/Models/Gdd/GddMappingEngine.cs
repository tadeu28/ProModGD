using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Manager;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;

namespace BPM2Game.Models.Gdd
{
    public class GddMappingEngine
    {
        public static DbFactory DbFactory = SessionManager.Instance.DbFactory;

        private static Project _project;
        private static GddConfiguration _gddConf;

        public static String Html { get; set; }

        public static ProjectGdd ProcessGddBasedOnMapping(Project project, GddConfiguration gddConf, bool isHtml)
        {
            try
            {
                _project = project;
                _gddConf = gddConf;
                Html = "";
                
                //Cria o GDD
                var gdd = new ProjectGdd()
                {
                    CreationDate = DateTime.Now,
                    Project = project,
                    DesignerName = LoginUtils.Designer.Name,
                    BasedOnMapping = true
                };

                gdd = DbFactory.ProjectGddRepository.Save(gdd);

                var headerHtml =
                    $"<h1 style='text-align: center;'>{gdd.Project.Title}</h1>" +
                    "<h3 style='text-align: center;'>(Game Design Document)</h3>" +
                    $"<h4 style='text-align: center;'>Created by: {gdd.DesignerName} in {DateTime.Now.ToShortDateString()} </h4>" +
                    $"<h6 style='text-align: center; color: silver;'>Game Id: {gdd.Project.Id}</h6>" +
                    "<h6 style='text-align: center; color: silver;'>Obs.: This document have been created based on game mapping performed to this project.</h6><br/>";

                var level = 0;
                var gddConfigSections =
                    DbFactory.GddConfigurationElementsRepository.FindAllElementsByGddId(_gddConf.Id).Where(w => w.ParentElement == null).OrderBy(o => o.PresentationOrder).ToList();
                if (gddConfigSections.Count > 0)
                {
                    Html = "<p style='text-align: justify;'><ol>";

                    foreach (var element in gddConfigSections)
                    {
                        CreateGddChapterAndSections(element, level);
                    }

                    Html = Html + "</ol></p>";
                }
                
                Html = headerHtml + Html;

                gdd.GddContent = Encoding.UTF8.GetBytes(Html ?? "");
                gdd = DbFactory.ProjectGddRepository.Update(gdd);

                return gdd;
            }
            catch (Exception ex)
            {
                throw new Exception("It haven't been possible to generate the game design document.", ex);
            }
        }

        private static void CreateGddChapterAndSections(GddConfigurationElements gddConfigElement, int level)
        {
            try
            {
                var chapter =$"<li><h3>{gddConfigElement.Title}</h3></li>";
                Html = Html + chapter;

                CreateGddSectionContent(gddConfigElement);

                var childElements =
                    DbFactory.GddConfigurationElementsRepository.FindAllChildren(gddConfigElement.Id);

                if (childElements != null && childElements.Count > 0)
                {
                    level++;
                    Html = Html + "<ol>";
                    foreach (var element in childElements)
                    {
                        CreateGddChapterAndSections(element, level);
                    }
                    Html = Html + "</ol>";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("It haven't been possible to generate the chapters and sections of game design document.", ex);
            }
        }

        private static void CreateGddSectionContent(GddConfigurationElements confElement)
        {
            try
            {
                var mapping = DbFactory.DesignMappingRepository.FindFirstByProjectId(_project.Id);
                if (mapping != null)
                {
                    foreach (var genreElement in confElement.GameGenreElements)
                    {
                        var mappingElements =
                            mapping.GameDesignMappingElements.Where(w => w.GameGenreElement.Id == genreElement.Id)
                                .ToList();

                        if (mappingElements.Count > 0)
                        {
                            Html = Html + "<ul>";
                            foreach (var mappingContent in mappingElements)
                            {
                                var content =
                                    "<li>" +
                                    $"<p>{mappingContent.Descricao}"+
                                    (!String.IsNullOrEmpty(mappingContent.ModelElementId)
                                        ? " <small style='color: silver;'>(*Relative to process object: " + mappingContent.ModelElementId + ")</small>"
                                        : "")+
                                    "</p></li>";

                                Html = Html + content;

                            }
                            Html = Html + "</ul>";
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