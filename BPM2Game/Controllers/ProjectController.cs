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
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;
using BPM2Game.Mapping.Bpmn;
using Newtonsoft.Json;
using Rotativa.Core;
using Rotativa.MVC;

namespace BPM2Game.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Projects()
        {
            try
            {
                var projects = DbFactory.Instance.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

                ViewBag.Genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.User.Designer, false);

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
                var genre = DbFactory.Instance.GameGenreRepository.FindFirstById(IdGameGenre);

                project.Owner = LoginUtils.User.Designer;
                project.StartDate = DateTime.Now;
                project.LastUpdate = DateTime.Now;
                project.GameGenre = genre;
                project.Designers.Add(project.Owner);

                DbFactory.Instance.ProjectRepository.Save(project);

                var projects = DbFactory.Instance.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

                var genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.User.Designer, false);
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
                var projects = DbFactory.Instance.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

                if (prjs != null)
                {
                    projects.ForEach(f =>
                    {
                        if (prjs.ToList().Exists(e => e.Equals(f.Id.ToString())))
                        {
                            f.Inactive = true;
                            DbFactory.Instance.ProjectRepository.Update(f);
                        }
                    });

                    projects = DbFactory.Instance.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();
                }

                var genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.User.Designer, false);
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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);
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
            var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);

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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(guidId);

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
                        DbFactory.Instance.ProjectRepository.Update(project);
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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);

                if (LoginUtils.User == null)
                {
                    return View("ProcjectAccessSolicitation", project);
                }else if (project.Designers.All(a => a.Id != LoginUtils.User.Designer.Id))
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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);
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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);
                var associations =
                    DbFactory.Instance.AssociationConfRepository.FindAllElementsByGenre(project.GameGenre.Id, false);

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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);
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
                var maps = DbFactory.Instance.AssociationConfRepository.FindAllElementsByGenre(id, false);

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
                var map = DbFactory.Instance.DesignMappingRepository.FindFirstByProjectId(id);

                DbFactory.Instance.DesignMappingRepository.Delete(map);

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
                var map = DbFactory.Instance.DesignMappingRepository.FindFirstByProjectId(id);
                
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
                var proj = DbFactory.Instance.ProjectRepository.FindFirstById(project.Id);
                proj.Title = project.Title;
                proj.Description = project.Description;
                proj.LastUpdate = DateTime.Now;

                proj = DbFactory.Instance.ProjectRepository.Update(proj);

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
                var sol = DbFactory.Instance.ProjectSolicitationRepository.FindFirstById(id);

                DbFactory.Instance.ProjectSolicitationRepository.Delete(sol);

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
                var association = DbFactory.Instance.AssociationConfRepository.FindFirstById(idAssociations);
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(idProject);

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

                designMapping = DbFactory.Instance.DesignMappingRepository.Save(designMapping);
                
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
                var associatedElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(id);
                var designMap = DbFactory.Instance.DesignMappingRepository.FindFirstById(idDesignMap);

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
                var designer = DbFactory.Instance.DesignerRepository.FindFirstById(id);
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(idProject);

                var solicitation = new ProjectSolicitation()
                {
                    Date = DateTime.Now,
                    Designer = designer,
                    Project = project
                };

                DbFactory.Instance.ProjectSolicitationRepository.Save(solicitation);

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
                var associatedElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(idAssociationElement);
                var designMap = DbFactory.Instance.DesignMappingRepository.FindFirstById(idDesignMap);
                
                mapElement.AssociateElement = associatedElement;
                mapElement.DesignMapping = designMap;
                mapElement.GameGenreElement = associatedElement.GameGenreElement;
                mapElement.IsManual = true;

                mapElement = DbFactory.Instance.GameDesignMappingElementsRepository.Save(mapElement);

                designMap = DbFactory.Instance.DesignMappingRepository.FindFirstById(idDesignMap);

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
                var mapElement = DbFactory.Instance.GameDesignMappingElementsRepository.FindFirstById(id);
                var idDesignMap = mapElement.DesignMapping.Id;

                DbFactory.Instance.GameDesignMappingElementsRepository.Delete(mapElement);

                var designMap = DbFactory.Instance.DesignMappingRepository.FindFirstById(idDesignMap);

                return PartialView("_TblMappingElements", new List<DesignMapping>() { designMap });
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "SaveMapElement"));
            }
        }

        public PartialViewResult ShowElementInfo(String id)
        {
            try
            {
                var elements = DbFactory.Instance.GameDesignMappingElementsRepository.FindFirstByModelId(id);

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
                var sol = DbFactory.Instance.ProjectSolicitationRepository.FindFirstById(id);
                var idProject = sol.Project.Id;
                
                DbFactory.Instance.ProjectSolicitationRepository.Delete(sol);

                var project = DbFactory.Instance.ProjectRepository.FindFirstById(idProject);

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
                var sol = DbFactory.Instance.ProjectSolicitationRepository.FindFirstById(id);
                var idProject = sol.Project.Id;
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(idProject);
                
                project.Designers.Add(sol.Designer);

                project = DbFactory.Instance.ProjectRepository.Update(project);

                var solicitations = DbFactory.Instance.ProjectSolicitationRepository.FindByProjectAndDesigner(project.Id, sol.Designer.Id);

                foreach (var solicitation in solicitations)
                {
                    DbFactory.Instance.ProjectSolicitationRepository.Delete(solicitation);
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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(idProject);

                var designer = project.Designers.FirstOrDefault(f => f.Id == id);
                if (designer != null)
                {
                    project.Designers.Remove(designer);
                    project = DbFactory.Instance.ProjectRepository.Update(project);
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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);

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
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);

                var gdd = new ProjectGdd()
                {
                    CreationDate = DateTime.Now,
                    Project = project,
                    DesignerName = LoginUtils.User.Designer.Name
                };

                gdd = DbFactory.Instance.ProjectGddRepository.Save(gdd);

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
                var gdd = DbFactory.Instance.ProjectGddRepository.FindFirstById(id);

                if (gdd != null)
                {
                    DbFactory.Instance.ProjectGddRepository.Delete(gdd);
                }

                return PartialView("_ShowGdd", new ProjectGdd());
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "DeleteGdd"));
            }
        }

        public PartialViewResult NewGddSection(Guid id)
        {
            try
            {
                var gdd = DbFactory.Instance.ProjectGddRepository.FindFirstById(id);

                var gddSections = DbFactory.Instance.ProjectGddSectionRepository.FindAllByProjectId(id).OrderBy(o => o.DtHoraCadastro).ToList();
                gddSections.Insert(0, new ProjectGddSection()
                {
                    Title = "",

                });

                ViewBag.Sections = new SelectList(gddSections, "Id", "Title");
                return PartialView("_AddGddSection", new ProjectGddSection()
                {
                    ProjectGdd = gdd
                });
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "NewGddSection"));
            }
        }


        public PartialViewResult SaveGddSection(ProjectGddSection section, Guid idGdd, Guid idSection)
        {
            try
            {
                var gdd = DbFactory.Instance.ProjectGddRepository.FindFirstById(idGdd);
                var parentSection = DbFactory.Instance.ProjectGddSectionRepository.FindFirstById(idSection);

                section.ProjectGdd = gdd;
                section.ParentSection = parentSection;
                section.DtHoraCadastro = DateTime.Now;

                DbFactory.Instance.ProjectGddSectionRepository.Save(section);

                gdd = DbFactory.Instance.ProjectGddRepository.FindFirstById(idGdd);

                return PartialView("_ShowGdd", gdd);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "SaveGddSection"));
            }
        }
        
    }
}