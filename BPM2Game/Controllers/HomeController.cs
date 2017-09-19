using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.Utils;

namespace BPM2Game.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (LoginUtils.User == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Projects", "Project");
            }
        }
        
        
    }
}