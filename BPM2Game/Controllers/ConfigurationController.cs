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
    public class ConfigurationController : Controller
    {
        // GET: Configuration
        public ActionResult ProcessModelConfiguration()
        {
            var designer = LoginUtils.User.Designer;
            var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer);

            return View(languages);
        }

        public PartialViewResult SaveLanguage(ModelingLanguage language)
        {
            try
            {
                var designer = LoginUtils.User.Designer;

                language.Inactive = false;
                language.IsConstant = false;
                language.Designer = designer;
                language.RegisterDate = DateTime.Now;
                DbFactory.Instance.ModelingLanguageRepository.Save(language);

                var languages = new List<ModelingLanguage> {language};

                return PartialView("_TblLanguages", languages);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveLanguage"));
            }
        }

        public PartialViewResult RealoadAllLanguages()
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer);

                return PartialView("_TblLanguages", languages);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RealoadAllLanguages"));
            }
        }

        public PartialViewResult RemoveLanguage(Guid id)
        {
            try
            {
                var designer = LoginUtils.User.Designer;

                var lang = DbFactory.Instance.ModelingLanguageRepository.FindFirstById(id);

                DbFactory.Instance.ModelingLanguageRepository.Delete(lang);

                var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer);

                return PartialView("_TblLanguages", languages);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RemoveLanguage"));
            }
        }

        public PartialViewResult LanguageElements(Guid id)
        {
            try
            {
                var elements = DbFactory.Instance.ModelingLanguageElementRepository.FindAllElementsByLanguageId(id);

                ViewBag.IdLanguage = id;
                return PartialView("_TblElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "LanguageElements"));
            }
        }
    }
}