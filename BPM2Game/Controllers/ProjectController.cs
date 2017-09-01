using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;

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

        public ActionResult Project(int id)
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult SaveNewProject(Project project)
        {
            try
            {
                project.Owner = LoginUtils.User.Designer;
                project.StartDate = DateTime.Now;
                project.LastUpdate = DateTime.Now;
                project.DesignTeam = new DesignTeam()
                {
                   Designer = project.Owner,
                   Project = project
                };

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
                            DbFactory.Instance.ProjectRepository.Save(f);
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
                return View();
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

                        project.BpmnModel = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);
                        project.BpmnModelPath = caminhoArquivo;
                        DbFactory.Instance.ProjectRepository.Save(project);
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}