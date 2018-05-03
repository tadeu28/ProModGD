using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Adapter;
using Bpm2GP.Model.DataBase.Manager;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;
using BPM2Game.Mapping.Bpmn;
using BPM2Game.Models.Gdd;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Newtonsoft.Json;
using Rotativa.Core;
using Rotativa.MVC;

namespace BPM2Game.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        public static DbFactory DbFactory = SessionManager.Instance.DbFactory;

        // GET: Project
        public ActionResult Projects()
        {
            try
            {
                var projects = DbFactory.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

                var genres = DbFactory.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.Designer.Id, false);

                ViewBag.Genres = genres;

                return View(projects);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Project", "Projects"));
            }
        }

        [HttpPost]
        public PartialViewResult SaveNewProject(Project project, Guid IdGameGenre)
        {
            try
            {
                var genre = DbFactory.GameGenreRepository.FirstById(IdGameGenre);

                project.Owner = LoginUtils.Designer;
                project.StartDate = DateTime.Now;
                project.LastUpdate = DateTime.Now;
                project.GameGenre = genre;
                project.Designers.Add(project.Owner);

                DbFactory.ProjectRepository.Save(project);

                var projects = DbFactory.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

                var genres = DbFactory.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.Designer.Id, false);
                ViewData["Genres"] = genres;
                return PartialView("_TblProjects", projects);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "SaveNewProject"));
            }
        }

        [HttpPost]
        public PartialViewResult DeleteProjects(string[] prjs)
        {
            try
            {
                var projects = DbFactory.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

                if (prjs != null)
                {
                    projects.ForEach(f =>
                    {
                        if (prjs.ToList().Exists(e => e.Equals(f.Id.ToString())))
                        {
                            f.Inactive = true;
                            DbFactory.ProjectRepository.Update(f);
                        }
                    });

                    projects = DbFactory.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();
                }

                var genres = DbFactory.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.Designer.Id, false);
                ViewData["Genres"] = genres;
                return PartialView("_TblProjects", projects);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "DeleteProjects"));
            }
        }

        public ActionResult ProcessModelling(Guid id)
        {
            try {
                ViewBag.Edit = false;
                var project = DbFactory.ProjectRepository.FirstById(id);
                if (project.BpmnModelPath != "")
                {
                    ViewBag.Edit = true;
                }
                ViewBag.ProjectId = id;
                ViewBag.BpmnFilePath = Request.Url.Authority + "/files/bpmn/" + project.Id + ".txt";

                return View(project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "ProcessModelling"));
            }
        }

        public ActionResult ViewModelling(Guid id)
        {
            var project = DbFactory.ProjectRepository.FirstById(id);

            //var strBpmnModel = Encoding.UTF8.GetString(project.BpmnModel);

            ViewBag.ProjectId = id;
            ViewBag.BpmnFilePath = Request.Url.Authority + "/files/bpmn/" + project.Id +".txt";
            return View();
        }

        [HttpPost]
        public JsonResult SalvarXML(string id)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var project = DbFactory.ProjectRepository.FirstById(guidId);

                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    if (file != null)
                    {
                        var uploadPath = Server.MapPath("~/files/bpmn");
                        string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(file.FileName));
                        file.SaveAs(caminhoArquivo);

                        //project.BpmnModel = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                        project.BpmnModelPath = caminhoArquivo;
                        DbFactory.ProjectRepository.Update(project);
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult Project(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);

                if (LoginUtils.User == null)
                {
                    return View("ProcjectAccessSolicitation", project);
                }else if (project.Designers.All(a => a.Id != LoginUtils.Designer.Id))
                {
                    return View("ProcjectAccessSolicitation", project);
                }

                return View(project);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Project", "Project"));
            }
        }
        
        public PartialViewResult ProjectInformation(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);
                return PartialView("_ProjectInformation", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "ProjectInformation"));
            }
        }

        public PartialViewResult GameConfiguration(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);
                var associations =
                    DbFactory.AssociationConfRepository.FindAllElementsByGenre(project.GameGenre.Id, false);

                ViewData["Associations"] = associations;
                return PartialView("_GameConfiguration", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "GameConfiguration"));
            }
        }

        public PartialViewResult ProcessInformation(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);
                ViewData["BpmnModelPath"] = Request.Url.Authority + "/files/bpmn/" + project.Id + ".txt";

                return PartialView("_ProcessInformation", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "ProcessInformation"));
            }
        }

        public JsonResult AllMappings(Guid id)
        {
            try
            {
                var maps = DbFactory.AssociationConfRepository.FindAllElementsByGenre(id, false);

                var adapters = new List<GenericAdapter>();
                adapters.Add(new GenericAdapter());
                maps.ForEach(f =>
                {
                    adapters.Add(new GenericAdapter()
                    {
                        Id = f.Id,
                        Name = f.Name
                    });
                });

                var jsonStr = JsonConvert.SerializeObject(adapters);

                return Json(jsonStr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error : " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ExcluirMapeamento(Guid id)
        {
            try
            {
                var map = DbFactory.DesignMappingRepository.FindFirstByProjectId(id);

                DbFactory.DesignMappingRepository.Delete(map);

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error:" + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintMappping(Guid id)
        {
            try
            {
                var map = DbFactory.DesignMappingRepository.FindFirstByProjectId(id);
                
                var pdf = new ViewAsPdf()
                {
                    ViewName = "_TblMappingElements",
                    Model = new List<DesignMapping>() { map },
                    RotativaOptions = new DriverOptions()
                    {
                        PageSize = Rotativa.Core.Options.Size.A4,
                        PageOrientation = Rotativa.Core.Options.Orientation.Portrait
                    }
                };

                return pdf;
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Project", "ProcessInformation"));
            }
        }

        public PartialViewResult UpdateProject(Project project)
        {
            try
            {
                var proj = DbFactory.ProjectRepository.FirstById(project.Id);
                proj.Title = project.Title;
                proj.Description = project.Description;
                proj.LastUpdate = DateTime.Now;

                proj = DbFactory.ProjectRepository.Update(proj);

                return PartialView("_ProjectInformationPanel", proj);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "UpdateProject"));
            }
        }

        public ActionResult RemoveSolicitation(Guid id)
        {
            try
            {
                var sol = DbFactory.ProjectSolicitationRepository.FirstById(id);

                DbFactory.ProjectSolicitationRepository.Delete(sol);

                return RedirectToAction("UserProfile", "User");
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "RemoveSolicitation"));
            }
        }

        public PartialViewResult CreateMapping(Guid idProject, Guid idAssociations)
        {
            try
            {
                var association = DbFactory.AssociationConfRepository.FirstById(idAssociations, false);
                var project = DbFactory.ProjectRepository.FirstById(idProject);

                var designMapping = new DesignMapping()
                {
                    AssociationConf = association,
                    CreationDate = DateTime.Now,
                    Project = project,
                    Language = association.Language,
                    Genre = association.Genre
                };
                
                var mappingCore = new BpmnMapEngineClass();
                mappingCore.StartMapping(designMapping);

                designMapping.ModelScore = mappingCore.ModelMappedScore;
                designMapping.GameDesignMappingElements = mappingCore.MappingList;
                designMapping.GameMappingScores = mappingCore.MappingScores;
                designMapping.DesignMappingErrors = mappingCore.Errors;

                designMapping = DbFactory.DesignMappingRepository.Save(designMapping);
                
                return PartialView("_TblMappingElements", new List<DesignMapping>() { designMapping });
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "CreateMapping"));
            }
        }

        public PartialViewResult AddMappingElement(Guid id, Guid idDesignMap)
        {
            try
            {
                var associatedElement = DbFactory.AssociationConfElementRepository.FirstById(id);
                var designMap = DbFactory.DesignMappingRepository.FirstById(idDesignMap);

                var mapElement = new GameDesignMappingElements()
                {
                    AssociateElement = associatedElement,
                    DesignMapping = designMap,
                    GameGenreElement = associatedElement.GameGenreElement
                };

                return PartialView("_AddMappingElement", mapElement);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "AddMappingElement"));
            }
        }

        public JsonResult AskParticipation(Guid id, Guid idProject)
        {
            try
            {
                var designer = DbFactory.DesignerRepository.FirstById(id);
                var project = DbFactory.ProjectRepository.FirstById(idProject);

                var solicitation = new ProjectSolicitation()
                {
                    Date = DateTime.Now,
                    Designer = designer,
                    Project = project
                };

                DbFactory.ProjectSolicitationRepository.Save(solicitation);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult SaveMapElement(GameDesignMappingElements mapElement, Guid idAssociationElement, Guid idDesignMap)
        {
            try
            {
                var associatedElement = DbFactory.AssociationConfElementRepository.FirstById(idAssociationElement);
                var designMap = DbFactory.DesignMappingRepository.FirstById(idDesignMap);
                
                mapElement.AssociateElement = associatedElement;
                mapElement.DesignMapping = designMap;
                mapElement.GameGenreElement = associatedElement.GameGenreElement;
                mapElement.IsManual = true;

                mapElement = DbFactory.GameDesignMappingElementsRepository.Save(mapElement);

                designMap = DbFactory.DesignMappingRepository.FirstById(idDesignMap);

                return PartialView("_TblMappingElements", new List<DesignMapping>() { designMap });
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "SaveMapElement"));
            }
        }

        public PartialViewResult RemoveMappingElement(Guid id)
        {
            try
            {
                var mapElement = DbFactory.GameDesignMappingElementsRepository.FirstById(id);
                var idDesignMap = mapElement.DesignMapping.Id;

                DbFactory.GameDesignMappingElementsRepository.Delete(mapElement);

                var designMap = DbFactory.DesignMappingRepository.FirstById(idDesignMap);

                return PartialView("_TblMappingElements", new List<DesignMapping>() { designMap });
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "SaveMapElement"));
            }
        }

        public PartialViewResult ShowElementInfo(String id, String projectId)
        {
            try
            {
                var elements = DbFactory.GameDesignMappingElementsRepository.FindFirstByModelIdAndProjectId(id, projectId);

                ViewBag.idModel = id;
                return PartialView("_ElementInfomationPane", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "ShowElementInfo"));
            }
        }

        public PartialViewResult RemoveSolicitationAjax(Guid id)
        {
            try
            {
                var sol = DbFactory.ProjectSolicitationRepository.FirstById(id);
                var idProject = sol.Project.Id;
                
                DbFactory.ProjectSolicitationRepository.Delete(sol);

                var project = DbFactory.ProjectRepository.FirstById(idProject);

                return PartialView("_ProjectDesigners", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "RemoveSolicitationAjax"));
            }
        }

        public PartialViewResult AcceptSolicitation(Guid id)
        {
            try
            {
                var sol = DbFactory.ProjectSolicitationRepository.FirstById(id);
                var idProject = sol.Project.Id;
                var project = DbFactory.ProjectRepository.FirstById(idProject);
                
                project.Designers.Add(sol.Designer);

                project = DbFactory.ProjectRepository.Update(project);

                var solicitations = DbFactory.ProjectSolicitationRepository.FindByProjectAndDesigner(project.Id, sol.Designer.Id);

                foreach (var solicitation in solicitations)
                {
                    DbFactory.ProjectSolicitationRepository.Delete(solicitation);
                }
                
                return PartialView("_ProjectDesigners", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "AcceptSolicitation"));
            }
        }

        public PartialViewResult RemoveDesigner(Guid id, Guid idProject)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(idProject);

                var designer = project.Designers.FirstOrDefault(f => f.Id == id);
                if (designer != null)
                {
                    project.Designers.Remove(designer);
                    project = DbFactory.ProjectRepository.Update(project);
                }
                
                return PartialView("_ProjectDesigners", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "RemoveDesigner"));
            }
        }

        public PartialViewResult GameDesignDocument(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);
                if(project.ProjectGdd != null && project.ProjectGdd.GddContent != null)
                    project.ProjectGdd.GddAsHtml = Encoding.UTF8.GetString(project.ProjectGdd.GddContent);

                ViewBag.Gdd = project.ProjectGdd;
                
                return PartialView("_GameDesignDocument", project);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "GameDesignDocument"));
            }
        }

        public PartialViewResult CreateGdd(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);

                var gdd = new ProjectGdd()
                {
                    CreationDate = DateTime.Now,
                    Project = project,
                    DesignerName = LoginUtils.Designer.Name,
                    BasedOnMapping = false
                };

                gdd = DbFactory.ProjectGddRepository.Save(gdd);

                return PartialView("_ShowGdd", gdd);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "CreateGdd"));
            }
        }

        public PartialViewResult DeleteGdd(Guid id)
        {
            try
            {
                var gdd = DbFactory.ProjectGddRepository.FirstById(id);

                if (gdd != null)
                {
                    DbFactory.ProjectGddRepository.Delete(gdd);
                }

                return PartialView("_ShowGdd", new ProjectGdd());
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "DeleteGdd"));
            }
        }

        [HttpPost]
        public JsonResult SaveGddContentSection(Guid id)
        {
            try
            {
                foreach (string fileName in Request.Files)
                {
                    var file = Request.Files[fileName];
                    if (file != null)
                    {
                        var reader = new StreamReader(file.InputStream);
                        var text = reader.ReadToEnd();

                        Stream s = new MemoryStream(Encoding.UTF8.GetBytes(text ?? ""));
                        var txtBlob = new BinaryReader(s).ReadBytes(file.ContentLength);
                        
                        var gdd = DbFactory.ProjectGddRepository.FirstById(id);
                        gdd.GddContent = txtBlob;

                        DbFactory.ProjectGddRepository.Update(gdd);
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult ShowGddBasedOnMapping(Guid id)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(id);

                var gddConfigs = DbFactory.GddConfigurationRepository.FindAllByGameGenre(project.GameGenre, false);

                var configs = new SelectList(gddConfigs, "Id", "Title");
                ViewBag.GddConfig = configs;
                ViewBag.IdProject = project.Id;

                return PartialView("_SelectGddConf", configs);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "CreateGddBasedOnMapping"));
            }
        }

        public PartialViewResult CreateGddBasedOnMapping(Guid idGddConfig, Guid IdProject)
        {
            try
            {
                var project = DbFactory.ProjectRepository.FirstById(IdProject);
                var gddConf = DbFactory.GddConfigurationRepository.FirstById(idGddConfig);

                var gdd = GddMappingEngine.ProcessGddBasedOnMapping(project, gddConf, true);

                gdd = DbFactory.ProjectGddRepository.FirstById(gdd.Id);

                if (gdd != null && gdd.GddContent != null)
                    gdd.GddAsHtml = Encoding.UTF8.GetString(gdd.GddContent);

                return PartialView("_ShowGdd", gdd);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "CreateGddBasedOnMapping"));
            }
        }
    }
}