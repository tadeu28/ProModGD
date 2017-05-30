using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}