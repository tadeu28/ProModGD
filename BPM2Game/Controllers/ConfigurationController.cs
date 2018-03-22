using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Models;
using Bpm2GP.Model.Utils;

namespace BPM2Game.Controllers
{
    [OutputCache(NoStore = true, Duration = 0)]
    [Authorize]
    public class ConfigurationController : Controller
    {
        // GET: Configuration
        public ActionResult ProcessModelConfiguration()
        {
            var designer = LoginUtils.User.Designer;
            var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer, false);

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

        public PartialViewResult CopyLanguage(ModelingLanguage language, Guid idLanguage)
        {
            try
            {
                var languageToCopy = DbFactory.Instance.ModelingLanguageRepository.FindFirstById(idLanguage);

                var designer = LoginUtils.User.Designer;
                language.Inactive = false;
                language.IsConstant = false;
                language.Designer = designer;
                language.RegisterDate = DateTime.Now;
                language.Version = languageToCopy.Version;
                language.Description += "\n [Copied from: "+ languageToCopy.Name + "]";

                language = DbFactory.Instance.ModelingLanguageRepository.Save(language);

                var tempElements = new List<ModelingLanguageElement>();
                foreach (var elementToCopy in languageToCopy.Elements.OrderBy(o => o.ParentElement != null))
                {
                    ModelingLanguageElement parentElement = null;
                    if(elementToCopy.ParentElement != null)
                        parentElement = tempElements.FirstOrDefault(f => f.Metamodel == elementToCopy.ParentElement.Metamodel);

                    var element = new ModelingLanguageElement()
                    {
                        Name = elementToCopy.Name,
                        Description = elementToCopy.Description,
                        Metamodel = elementToCopy.Metamodel,
                        Language = language,
                        ParentElement = parentElement
                    };

                    element = DbFactory.Instance.ModelingLanguageElementRepository.Save(element);

                    tempElements.Add(element);
                }

                var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer, false);

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
                var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer, false);

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

                lang.Inactive = true;

                DbFactory.Instance.ModelingLanguageRepository.Update(lang);

                var languages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer, false);

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
                var elements = DbFactory.Instance.ModelingLanguageElementRepository.FindAllElementsByLanguageId(id, false)
                    .OrderBy(o => o.Name).ToList();
                
                ViewBag.Language = DbFactory.Instance.ModelingLanguageRepository.FindFirstById(id);
                return PartialView("_TblElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "LanguageElements"));
            }
        }

        public PartialViewResult SaveElement(ModelingLanguageElement element, Guid IdLanguage, Guid IdBaseElement)
        {
            try
            {
                var language = DbFactory.Instance.ModelingLanguageRepository.FindFirstById(IdLanguage);

                var baseElement = DbFactory.Instance.ModelingLanguageElementRepository.FindFirstById(IdBaseElement);
                
                element.Language = language;
                if (baseElement != null)
                {
                    element.ParentElement = baseElement;
                }

                DbFactory.Instance.ModelingLanguageElementRepository.Save(element);

                var elements = DbFactory.Instance.ModelingLanguageElementRepository.FindAllElementsByLanguageId(IdLanguage, false)
                    .OrderBy(o => o.Name).ToList();
                                
                ViewBag.Language = language;
                return PartialView("_TblElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveElement"));
            }
        }

        public PartialViewResult RemoveElement(Guid id)
        {
            try
            {
                var element = DbFactory.Instance.ModelingLanguageElementRepository.FindFirstById(id);
                var idLanguage = Guid.Parse(element.Language.Id.ToString());

                element.Inactive = true;
                DbFactory.Instance.ModelingLanguageElementRepository.Update(element);

                var language = DbFactory.Instance.ModelingLanguageRepository.FindFirstById(idLanguage);
                var elements = DbFactory.Instance.ModelingLanguageElementRepository.FindAllElementsByLanguageId(idLanguage, false)
                    .OrderBy(o => o.Name).ToList();

                ViewBag.Language = language;
                return PartialView("_TblElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RemoveElement"));
            }
        }

        public ActionResult GameGenreConfiguration()
        {
            var designer = LoginUtils.User.Designer;
            var genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(designer, false);

            return View(genres);
        }

        public PartialViewResult ReloadAllGenres()
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(designer, false);

                return PartialView("_TblGenres", genres);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "ReloadAllGenres"));
            }
        }

        public PartialViewResult ReloadAllGdds()
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var gdds = DbFactory.Instance.GddConfigurationRepository.FindAllGenresByDesigner(designer, false);

                return PartialView("_TblGdds", gdds);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "ReloadAllGenres"));
            }
        }

        public PartialViewResult SaveGenre(GameGenre genre)
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                
                genre.IsConstant = false;
                genre.Designer = designer;
                genre.RegisterDate = DateTime.Now;
                DbFactory.Instance.GameGenreRepository.Save(genre);

                var genres = new List<GameGenre> { genre };

                return PartialView("_TblGenres", genres);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveGenre"));
            }
        }

        public PartialViewResult RemoveGameGenre(Guid id)
        {
            try
            {
                var designer = LoginUtils.User.Designer;

                var genre = DbFactory.Instance.GameGenreRepository.FindFirstById(id);

                DbFactory.Instance.GameGenreRepository.Update(genre);

                var genres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(designer, false);

                return PartialView("_TblGenres", genres);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RemoveGameGenre"));
            }
        }

        public PartialViewResult GenreElements(Guid id)
        {
            try
            {
                var elements = DbFactory.Instance.GameGenreElementRepository.FindAllElementsByGenreId(id)
                    .OrderBy(o => o.Name).ToList();

                ViewBag.Genre = DbFactory.Instance.GameGenreRepository.FindFirstById(id);
                return PartialView("_TblGenreElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "GenreElements"));
            }
        }

        public PartialViewResult SaveGameGenreElement(GameGenreElement element, Guid IdGenre)
        {
            try
            {
                var genre = DbFactory.Instance.GameGenreRepository.FindFirstById(IdGenre);
                
                element.GameGenre = genre;
                DbFactory.Instance.GameGenreElementRepository.Save(element);

                var elements = DbFactory.Instance.GameGenreElementRepository.FindAllElementsByGenreId(IdGenre)
                    .OrderBy(o => o.Name).ToList();

                ViewBag.Genre = genre;
                return PartialView("_TblGenreElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveGameGenreElement"));
            }
        }

        public PartialViewResult RemoveGenreElement(Guid id)
        {
            try
            {
                var element = DbFactory.Instance.GameGenreElementRepository.FindFirstById(id);
                var IdGenre = Guid.Parse(element.GameGenre.Id.ToString());

                element.Inactive = true;
                DbFactory.Instance.GameGenreElementRepository.Update(element);

                var genre = DbFactory.Instance.GameGenreRepository.FindFirstById(IdGenre);
                var elements = DbFactory.Instance.GameGenreElementRepository.FindAllElementsByGenreId(IdGenre)
                    .OrderBy(o => o.Name).ToList();

                ViewBag.Genre = genre;
                return PartialView("_TblGenreElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RemoveGenreElement"));
            }
        }

        public ActionResult ElementsAssociation()
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var processLanguages = DbFactory.Instance.ModelingLanguageRepository.FindAllLanguagesByDesigner(designer, false);
                var gameGenres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(designer, false);
                
                ViewBag.Languages = processLanguages;
                ViewBag.Genres = gameGenres;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Configuration", "ElementsAssociation"));
            }
        }

        public PartialViewResult ElementsSelect(Guid idAssociation)
        {
            try
            {
                var assoc = DbFactory.Instance.AssociationConfRepository.FindFirstById(idAssociation);

                return PartialView("_ElementsAssociationTables", assoc);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "ElementsSelect"));
            }
        }

        public PartialViewResult SaveElementAssociation(Guid idAssociation, Guid idProcElement, Guid[] cklGenreElements)
        {
            try
            {
                var assoc = DbFactory.Instance.AssociationConfRepository.FindFirstById(idAssociation);

                if (cklGenreElements != null)
                {
                    var procElement = DbFactory.Instance.ModelingLanguageElementRepository.FindFirstById(idProcElement);
                    var elements =
                        DbFactory.Instance.GameGenreElementRepository.FindAllElementsByListId(cklGenreElements);


                    foreach (var element in elements)
                    {
                        var assocElement = new AssociationConfElements()
                        {
                            Association = assoc,
                            GameGenreElement = element,
                            ProcessElement = procElement
                        };

                        DbFactory.Instance.AssociationConfElementRepository.Save(assocElement);
                    }
                }

                assoc = DbFactory.Instance.AssociationConfRepository.FindFirstById(idAssociation);
                return PartialView("_ElementsAssociationTables", assoc);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveElementAssociation"));
            }
        }

        public PartialViewResult LoadAssociations(Guid idLanguage, Guid idGenre)
        {
            try
            {
                var associations =
                    DbFactory.Instance.AssociationConfRepository.FindAllElementsByGenreAndLanguage(idGenre, idLanguage, false);

                ViewBag.IdGenre = idGenre;
                ViewBag.IdLanguage = idLanguage;
                return PartialView("_ViewAssociations", associations);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "LoadAssociations"));
            }
        }

        public PartialViewResult UnAssociateElement(Guid Id)
        {
            try
            {
                var assocElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(Id);
                var idAssoc = assocElement.Association.Id;

                assocElement.Inactive = true;

                DbFactory.Instance.AssociationConfElementRepository.Update(assocElement);

                var assoc = DbFactory.Instance.AssociationConfRepository.FindFirstById(idAssoc);
                return PartialView("_ElementsAssociationTables", assoc);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "UnAssociateElement"));
            }
        }

        public PartialViewResult SaveAssociation(Guid idGenre, Guid idLanguage, string edtAssociationName)
        {
            try
            {
                var genre = DbFactory.Instance.GameGenreRepository.FindFirstById(idGenre);
                var language = DbFactory.Instance.ModelingLanguageRepository.FindFirstById(idLanguage);
                
                var assoc = new AssociationConf()
                {
                    Name = edtAssociationName,
                    DtCreation = DateTime.Now,
                    Genre = genre,
                    Language = language
                };

                DbFactory.Instance.AssociationConfRepository.Save(assoc);

                var associations =
                    DbFactory.Instance.AssociationConfRepository.FindAllElementsByGenreAndLanguage(idGenre, idLanguage, false);

                ViewBag.IdGenre = idGenre;
                ViewBag.IdLanguage = idLanguage;
                return PartialView("_ViewAssociations", associations);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveAssociation"));
            }
        }

        public PartialViewResult DeleteAssociation(Guid id)
        {
            try
            {
                var assoc = DbFactory.Instance.AssociationConfRepository.FindFirstById(id);
                ViewBag.IdGenre = assoc.Genre.Id;
                ViewBag.IdLanguage = assoc.Language.Id;

                if (!assoc.IsConstant)
                {
                    assoc.Inactive = true;
                    DbFactory.Instance.AssociationConfRepository.Update(assoc);
                }

                var associations =
                    DbFactory.Instance.AssociationConfRepository.FindAllElementsByGenreAndLanguage(ViewBag.IdGenre, ViewBag.IdLanguage, false);
                
                return PartialView("_ViewAssociations", associations);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "DeleteAssociation"));
            }
        }

        public PartialViewResult ShowRuleDialog(Guid id)
        {
            try
            {
                var assocElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(id);
                var ruleType = DbFactory.Instance.AssociationTypeRepository.FindAll();

                ViewBag.RuleTypes = new SelectList(ruleType, "Id", "Description");
                return PartialView("_RuleDialog", assocElement);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "ShowRuleDialog"));
            }
        }

        public PartialViewResult DeleteRule(Guid id)
        {
            try
            {
                var ruleObj = DbFactory.Instance.AssociationRulesRepository.FindFirstById(id);
                var idAssoc = ruleObj.AssociationElement.Id;

                DbFactory.Instance.AssociationRulesRepository.Delete(ruleObj);

                var ruleTypes = DbFactory.Instance.AssociationTypeRepository.FindAll();
                var assocElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(idAssoc);

                ViewData["RuleType"] = new SelectList(ruleTypes, "Id", "Description");
                return PartialView("_AddAssociationRule", assocElement);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "DeleteRule"));
            }
        }

        public PartialViewResult SaveRule(Guid idAssociation, Int32 idType, String rule, String field, AssociationRuleOperator op)
        {
            try
            {
                var ruleTypes = DbFactory.Instance.AssociationTypeRepository.FindAll();
                var assocElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(idAssociation);
                var ruleType = ruleTypes.FirstOrDefault(f => f.Id == idType);

                var ruleObj = new AssociationRules()
                {
                    AssociationElement = assocElement,
                    Rule = rule,
                    Field = field,
                    Operator = op,
                    Type = ruleType
                };

                DbFactory.Instance.AssociationRulesRepository.Save(ruleObj);

                assocElement = DbFactory.Instance.AssociationConfElementRepository.FindFirstById(idAssociation);
                ViewData["RuleType"] = new SelectList(ruleTypes, "Id", "Description");
                return PartialView("_AddAssociationRule", assocElement);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "ShowRuleDialog"));
            }
        }

        public PartialViewResult ShowElementsAssociationDialog(Guid id, Guid idLanguageElement)
        {
            try
            {
                var assoc = DbFactory.Instance.AssociationConfRepository.FindFirstById(id);
                var languageElement = DbFactory.Instance.ModelingLanguageElementRepository.FindFirstById(idLanguageElement);

                var idGameElements = new List<Guid>();
                var assocElements = DbFactory.Instance.AssociationConfElementRepository.FindAllElementsByAssociantionAndLanguageElementId(idLanguageElement, id);
                foreach (var ae in assocElements)
                {
                    idGameElements.Add(ae.GameGenreElement.Id); 
                }

                var elementsInassociated =
                    DbFactory.Instance.GameGenreElementRepository.FindAllElementsByGenreNotInListId(assoc.Genre.Id, idGameElements.ToArray())
                    .OrderBy(o => o.Name).ToList();
                
                ViewBag.Association = assoc;
                ViewBag.LanguageElement = languageElement;

                return PartialView("_SaveElementAssociation", elementsInassociated);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "ShowElementsAssociationDialog"));
            }
        }

        public ActionResult GDDConfiguration()
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var gddsConfig = DbFactory.Instance.GddConfigurationRepository.FindAllGenresByDesigner(designer, false);

                return View(gddsConfig);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Configuration", "GDDConfiguration"));
            }
        }

        public PartialViewResult AddNewGdd()
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var gameGenres = DbFactory.Instance.GameGenreRepository.FindAllGenresByDesigner(designer, false);

                ViewData["GameGenre"] = new SelectList(gameGenres, "Id", "Name");

                return PartialView("_AddGdd", new GddConfiguration());
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "AddNewGdd"));
            }
        }

        public PartialViewResult SaveGdd(GddConfiguration gdd, Guid idGameGenre)
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var gameGenre = DbFactory.Instance.GameGenreRepository.FindFirstById(idGameGenre);

                gdd.Designer = designer;
                gdd.GameGenre = gameGenre;
                gdd.Inactive = false;
                gdd.IsConstant = false;
                gdd.RegistrationDate = DateTime.Now;

                DbFactory.Instance.GddConfigurationRepository.Save(gdd);

                var gdds = DbFactory.Instance.GddConfigurationRepository.FindAllGenresByDesigner(designer, false);

                return PartialView("_TblGdds", gdds);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "SaveGdd"));
            }
        }

        public PartialViewResult GddElements(Guid id)
        {
            try
            {
                var elements = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(id)
                    .OrderBy(o => o.PresentationOrder).ToList();

                ViewBag.IdGdd = id;
                return PartialView("_TblGddElements", elements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "GddElements"));
            }
        }

        public PartialViewResult AddGddSection(Guid id)
        {
            try
            {
                var Gdd = DbFactory.Instance.GddConfigurationRepository.FindFirstById(id);
                var GddSections = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(id).OrderBy(o => o.PresentationOrder).ToList();

                GddSections.Insert(0, new GddConfigurationElements()
                {
                    Title = "[None] - In case of Chapter"
                });

                var selectGDDSections = new SelectList(GddSections, "Id", "Title");

                var gameGenreElements = Gdd.GameGenre.Elements.OrderBy(o => o.Name).ToList();

                ViewBag.Gdd = Gdd;
                ViewBag.SelectGDDSections = selectGDDSections;
                ViewBag.GameGenreElements = gameGenreElements;
                return PartialView("_AddGddSection", new GddConfigurationElements());
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "AddGddSection"));
            }
        }

        public PartialViewResult SaveGddSection(Guid IdGdd, Guid IdGddElement, Guid IdSection, GddConfigurationElements gddElement, Guid[] cklGenreElements)
        {
            try
            {
                var Gdd = DbFactory.Instance.GddConfigurationRepository.FindFirstById(IdGdd);

                var parent = DbFactory.Instance.GddConfigurationElementsRepository.FindFirstById(IdSection);
                if (parent != null)
                {
                    gddElement.ParentElement = parent;
                }

                var maxOrder = DbFactory.Instance.GddConfigurationElementsRepository.GetMaxOrder(IdGdd);
                maxOrder++;
                gddElement.PresentationOrder = maxOrder;

                if (IdGddElement != gddElement.Id)
                {
                    var gddElemetDb = DbFactory.Instance.GddConfigurationElementsRepository.FindFirstById(IdGddElement);

                    gddElemetDb.Title = gddElement.Title;
                    gddElemetDb.Description = gddElement.Description;
                    gddElemetDb.ParentElement = gddElement.ParentElement;
                    gddElement = gddElemetDb;
                }
                
                if (cklGenreElements != null)
                {
                    var elements = DbFactory.Instance.GameGenreElementRepository.FindAllElementsByListId(cklGenreElements);

                    foreach (var element in elements)
                    {
                        gddElement.GameGenreElements.Add(element);
                    }
                }

                gddElement.GddConfig = Gdd;
                DbFactory.Instance.GddConfigurationElementsRepository.SaveOrUpdate(gddElement);

                var gddElements = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(Gdd.Id).OrderBy(o => o.PresentationOrder).ToList();

                ViewBag.IdGdd = Gdd.Id;
                return PartialView("_TblGddElements", gddElements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "AddGddSection"));
            }
        }

        public PartialViewResult UnGddGameElement(Guid Id, Guid idGddElement)
        {
            try
            {
                var gddElement = DbFactory.Instance.GddConfigurationElementsRepository.FindFirstById(idGddElement);
                var gameElement = gddElement.GameGenreElements.FirstOrDefault(f => f.Id == Id);
                if (gameElement != null)
                {
                    gddElement.GameGenreElements.Remove(gameElement);
                    DbFactory.Instance.GddConfigurationElementsRepository.Update(gddElement);
                }

                var gddElements = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(gddElement.GddConfig.Id).OrderBy(o => o.PresentationOrder).ToList();

                ViewBag.IdGdd = gddElement.GddConfig.Id;
                return PartialView("_TblGddElements", gddElements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "UnGddGameElement"));
            }
        }

        public PartialViewResult RemoveGddElement(Guid Id)
        {
            try
            {
                var gddElement = DbFactory.Instance.GddConfigurationElementsRepository.FindFirstById(Id);
                var idGdd = gddElement.GddConfig.Id;
                DbFactory.Instance.GddConfigurationElementsRepository.Delete(gddElement);

                var gddElements = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(idGdd).OrderBy(o => o.PresentationOrder).ToList();

                ViewBag.IdGdd = idGdd;
                return PartialView("_TblGddElements", gddElements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RemoveGddElement"));
            }
        }

        public PartialViewResult OrderGddElement(Guid Id, int order, String op)
        {
            try
            {
                var gddElement = DbFactory.Instance.GddConfigurationElementsRepository.FindFirstById(Id);
                
                var gddElementChange = op == "d" ? 
                    DbFactory.Instance.GddConfigurationElementsRepository.GetNextElementInOrder(gddElement) : 
                    DbFactory.Instance.GddConfigurationElementsRepository.GetPreviousElementInOrder(gddElement);

                if (gddElementChange != null)
                {
                    var orderAux = gddElementChange.PresentationOrder;
                    gddElementChange.PresentationOrder = gddElement.PresentationOrder;
                    gddElement.PresentationOrder = orderAux;

                    DbFactory.Instance.GddConfigurationElementsRepository.Update(gddElementChange);
                    DbFactory.Instance.GddConfigurationElementsRepository.Update(gddElement);
                }

                var gddElements = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(gddElement.GddConfig.Id)
                    .OrderBy(o => o.PresentationOrder).ToList();

                ViewBag.IdGdd = gddElement.GddConfig.Id;
                return PartialView("_TblGddElements", gddElements);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "OrderGddElement"));
            }
        }

        public PartialViewResult UpdateGddElement(Guid id)
        {
            try
            {
                var gddElement = DbFactory.Instance.GddConfigurationElementsRepository.FindFirstById(id);
                var GddSections = DbFactory.Instance.GddConfigurationElementsRepository.FindAllElementsByGddId(gddElement.GddConfig.Id).OrderBy(o => o.PresentationOrder).ToList();
                GddSections.Remove(gddElement);

                GddSections.Insert(0, new GddConfigurationElements()
                {
                    Title = "[None] - In case of Chapter"
                });

                var selectedSection = GddSections.FirstOrDefault(f => f.Id == gddElement.ParentElement.Id);
                var selectGDDSections = new SelectList(GddSections, "Id", "Title", gddElement.ParentElement.Id);

                var gameGenreElements = gddElement.GddConfig.GameGenre.Elements.Where(ge => gddElement.GameGenreElements.FirstOrDefault(a => a.Id == ge.Id) == null)
                    .OrderBy(o => o.Name).ToList();

                ViewBag.Gdd = gddElement.GddConfig;
                ViewBag.SelectGDDSections = selectGDDSections;
                ViewBag.GameGenreElements = gameGenreElements;
                return PartialView("_AddGddSection", gddElement);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "UpdateGddElement"));
            }
        }

        public PartialViewResult RemoveGdd(Guid Id)
        {
            try
            {
                var designer = LoginUtils.User.Designer;
                var gdd = DbFactory.Instance.GddConfigurationRepository.FindFirstById(Id);
                gdd.Inactive = true;

                DbFactory.Instance.GddConfigurationRepository.Update(gdd);

                var gdds = DbFactory.Instance.GddConfigurationRepository.FindAllGenresByDesigner(designer, false);

                return PartialView("_TblGdds", gdds);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Configuration", "RemoveGdd"));
            }
        }
    }
}