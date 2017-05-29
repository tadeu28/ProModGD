using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BPM2Game.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogIn()
        {
            //Verificar autenticação aqui
            FormsAuthentication.SetAuthCookie("tadeuclasse", true);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ModeloProcesso()
        {
            return View();
        }
    }
}