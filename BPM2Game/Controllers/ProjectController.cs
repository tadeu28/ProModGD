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
using BPM2Game.Mapping.BpmnToAdventure;
using Newtonsoft.Json;

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

                return View(projects);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Project", "Projects"));
            }
        }

        [HttpPost]
        public PartialViewResult SaveNewProject(Project project)
        {
            try
            {
                project.Owner = LoginUtils.User.Designer;
                project.StartDate = DateTime.Now;
                project.LastUpdate = DateTime.Now;
                project.Designers.Add(project.Owner);

                DbFactory.Instance.ProjectRepository.Save(project);

                var projects = DbFactory.Instance.ProjectRepository.FindAll().Where(w => !w.Inactive).OrderByDescending(o => o.LastUpdate).ToList();

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

        [Authorize]
        public ActionResult Project(Guid id)
        {
            try
            {
                var project = DbFactory.Instance.ProjectRepository.FindFirstById(id);
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
                var genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(LoginUtils.User.Designer);

                ViewData["Genres"] = genres;
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
                var maps = DbFactory.Instance.AssociationConfRepository.FindAllElementsByGenre(id);

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

        public PartialViewResult ShowElementInfo(String id)
        {
            try
            {
                var elements = DbFactory.Instance.GameDesignMappingElementsRepository.FindFirstByModelId(id);
                
                return PartialView("_ElementInfomationPane", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Project", "ShowElementInfo"));
            }

        }
    }
}