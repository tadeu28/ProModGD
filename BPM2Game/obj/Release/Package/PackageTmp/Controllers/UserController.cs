using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;

namespace BPM2Game.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [AllowAnonymous]
        // GET: User
        public ActionResult SignIn(User user)
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult UserCompare(string UserName)
        {
            try
            {
                var user =
                    DbFactory.Instance.UserRepository.FindAll()
                        .FirstOrDefault(f => String.Equals(f.UserName, UserName, StringComparison.CurrentCultureIgnoreCase));

                if(user == null)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SaveNewUser(User user)
        {
            try
            {
                user.Designer = new Designer()
                {
                    Name = user.UserName
                };
                DbFactory.Instance.UserRepository.Save(user);

                LoginUtils.Logar(user);

                user.Designer.User = user;

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "User", "SaveNewUser"));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn(User u)
        {
            try
            {
                LoginUtils.Logar(u);

                var url = Request.Url.AbsoluteUri.Replace("User/LogIn", "Home/Index");

                return Json(new { success = true, Message = url });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = "" + ex.Message});
            }
        }

        public ActionResult LogOff()
        {
            LoginUtils.Deslogar();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            try
            {
                var designer = LoginUtils.User.Designer;

                return View(designer);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "User", "UserProfile"));
            }
        }

        public PartialViewResult SaveProfile(Designer designer)
        {
            try
            {
                LoginUtils.User.Designer = DbFactory.Instance.DesignerRepository.Save(designer);

                return PartialView("_DesignerProfile", LoginUtils.User.Designer);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "User", "SaveProfile"));
            }
        }
    }
}