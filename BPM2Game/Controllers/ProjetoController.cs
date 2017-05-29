using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BPM2Game.Controllers
{
    [Authorize]
    public class ProjetoController : Controller
    {
        // GET: Projeto
        public ActionResult Index(int id)
        {
            return View();
        }
    }
}