using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bpm2GP.Model.DataBase;
using Bpm2GP.Model.DataBase.Models;
using BPMN;

namespace BPM2Game.Mapping.BpmnToAdventure
{
    public class MappingClass
    {
        public Model Model { get; set; }
        public DesignMapping DesignMapping { get; set; }
        public List<GameDesignMappingElements> MappingList { get; set; }
        public List<Exception> Errors { get; set; }

        public void StartMapping(DesignMapping designMapping)
        {
            Errors = new List<Exception>();
            MappingList = new List<GameDesignMappingElements>();

            DesignMapping = designMapping;
            Model = Model.Read(designMapping.Project.BpmnModelPath);

            var bpmnStoredElements = DbFactory.Instance.ModelingLanguageElementRepository
                .FindAllElementsByLanguageId(designMapping.Language.Id).OrderBy(o => o.Metamodel).ToList();

            bpmnStoredElements.ForEach(f =>
            {
                var assocElements = DbFactory.Instance
                                        .AssociationConfElementRepository
                                        .FindAllElementsByElementeMetamodel(f.Metamodel)
                                        .OrderBy(o => o.ProcessElement.Name)
                                        .ToList();

                var word = f.Metamodel?.ToLower().Replace("bpmn:", "");
                if (word != null)
                {
                    var bpmnElements = Model.Elements.Where(w => w.TypeName.ToLower() == word).ToList();

                    if (bpmnElements.Count > 0)
                    {
                        ProcessMapping(word, assocElements, bpmnElements);
                    }
                }
            });

            MappingList = MappingList;
        }

        private void ProcessMapping(String word, List<AssociationConfElements> elements, List<Element> bpmnElements)
        {
            foreach (var element in elements)
            {
                if (element.ProcessElement.Metamodel.ToLower().Contains("bpmn:process"))
                {
                    ProcessTitle(element, bpmnElements);
                    //ProcessInstances
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("association"))
                {
                    ProcessMessageAssociations(element, bpmnElements);
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("flow"))
                {

                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("endevent"))
                {
                    //ProcessEndEvents(element, bpmnElements);
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("intermediate"))
                {

                }
                else
                {
                    ProcessDefaultRelation(element, bpmnElements);
                }
            }
        }

        private void ProcessMessageAssociations(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    if (el.Attributes["id"] != "")
                    {
                        if (element.ProcessElement.Metamodel.ToLower().Contains("input"))
                        {
                            var sourceRefId = el.Elements["sourceRef"].FirstOrDefault();
                            var souceElement = FindBpmnElementById(sourceRefId.Attributes["Value"], "id");

                            Element target = null;
                            var els = GetElementsWithElementChild(el.TypeName);
                            foreach (var e in els)
                            {
                                if (e.Elements.ContainsKey("dataInputAssociation") && e.Elements["dataInputAssociation"] != null)
                                {
                                    if (e.Elements["dataInputAssociation"].Exists(x => x.Attributes["id"] == el.Attributes["id"]))
                                    {
                                        target = e;
                                        break;
                                    }
                                }
                            }

                            if ((souceElement != null) && (target != null))
                            {
                                var ge = new GameDesignMappingElements()
                                {
                                    AssociateElement = element,
                                    Descricao = souceElement.Attributes["name"] + " <---> " + target.Attributes["name"],
                                    DesignMapping = DesignMapping,
                                    GameGenreElement = element.GameGenreElement,
                                    IsManual = false
                                };

                                MappingList.Add(ge);
                            }
                        }
                        else
                        {
                            var els = GetElementsWithElementChild(el.TypeName);
                            Element source = null;
                            foreach (var e in els)
                            {
                                if (e.Elements.ContainsKey("dataOutputAssociation") && e.Elements["dataOutputAssociation"] != null)
                                {
                                    if (e.Elements["dataOutputAssociation"].Exists(x => x.Attributes["id"] == el.Attributes["id"]))
                                    {
                                        source = e;
                                        break;
                                    }
                                }
                            }

                            var targetList = el.Elements["targetRef"];
                            foreach (var tg in targetList)
                            {
                                var target = FindBpmnElementById(tg.Attributes["Value"], "id");

                                if ((target != null) && (source != null))
                                {
                                    var ge = new GameDesignMappingElements()
                                    {
                                        AssociateElement = element,
                                        Descricao = source.Attributes["name"] + " <---> " + target.Attributes["name"],
                                        DesignMapping = DesignMapping,
                                        GameGenreElement = element.GameGenreElement,
                                        IsManual = false
                                    };

                                    MappingList.Add(ge);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Can't possible to process the Associations.", ex);
            }
        }

        private void ProcessTitle(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    var bpmnEl = FindBpmnElementById(el.Attributes["id"], "processRef");
                    if (bpmnEl != null)
                    {
                        var ge = new GameDesignMappingElements()
                        {
                            AssociateElement = element,
                            Descricao = bpmnEl.Attributes["name"],
                            DesignMapping = DesignMapping,
                            GameGenreElement = element.GameGenreElement,
                            IsManual = false
                        };

                        MappingList.Add(ge);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the Process.", ex));
            }
        }

        private void ProcessEvents(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    var ge = new GameDesignMappingElements()
                    {
                        AssociateElement = element,
                        Descricao = el.Attributes["name"],
                        DesignMapping = DesignMapping,
                        GameGenreElement = element.GameGenreElement,
                        IsManual = false
                    };

                    MappingList.Add(ge);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the title.", ex));
            }
        }

        private void ProcessEndEvents(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    if ((el.Elements.ContainsKey("errorEventDefinition")) && (el.Elements["errorEventDefinition"] != null))
                    {
                        var ge = new GameDesignMappingElements()
                        {
                            AssociateElement = element,
                            Descricao = el.Attributes["name"],
                            DesignMapping = DesignMapping,
                            GameGenreElement = element.GameGenreElement,
                            IsManual = false
                        };

                        MappingList.Add(ge);
                    }
                    
                    else
                    {
                        var ge = new GameDesignMappingElements()
                        {
                            AssociateElement = element,
                            Descricao = el.Attributes["name"],
                            DesignMapping = DesignMapping,
                            GameGenreElement = element.GameGenreElement,
                            IsManual = false
                        };

                        MappingList.Add(ge);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the EndEvent.", ex));
            }
        }

        private void ProcessDefaultRelation(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    if (el.Attributes.ContainsKey("processRef"))
                        continue;

                    var ge = new GameDesignMappingElements()
                    {
                        AssociateElement = element,
                        Descricao = el.Attributes["name"],
                        DesignMapping = DesignMapping,
                        GameGenreElement = element.GameGenreElement,
                        IsManual = false
                    };

                    MappingList.Add(ge);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to Process the Relation "+ element.ProcessElement.Name +".", ex));
            }
        }

        private void Resources(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    var ge = new GameDesignMappingElements()
                    {
                        AssociateElement = element,
                        Descricao = el.Attributes["name"],
                        DesignMapping = DesignMapping,
                        GameGenreElement = element.GameGenreElement,
                        IsManual = false
                    };

                    MappingList.Add(ge);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to ProcessCharacterOrPlayers.", ex));
            }
        }

        private void ProcessCharacterOrPlayers(AssociationConfElements element, List<Element> bpmnElements)
        {
            try
            {
                foreach (var el in bpmnElements)
                {
                    if(el.Attributes.ContainsKey("processRef"))
                        continue;

                    var ge = new GameDesignMappingElements()
                    {
                        AssociateElement = element,
                        Descricao = el.Attributes["name"],
                        DesignMapping = DesignMapping,
                        GameGenreElement = element.GameGenreElement,
                        IsManual = false
                    };

                    MappingList.Add(ge);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to ProcessCharacterOrPlayers.", ex));
            }
        }

        private Element FindBpmnElementById(String id, String idName)
        {
            try
            {
                return Model.Elements.FirstOrDefault(w => w.Attributes.ContainsKey(idName) && w.Attributes[idName] == id);
            }
            catch(Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }

        private List<Element> GetElementsWithElementChild(String name)
        {
            try
            {
                return Model.Elements.Where(w => w.Elements.ContainsKey(name)).ToList();
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }
        private Element GetElementsWithElementChildId(String name, string id)
        {
            try
            {
                var elements = GetElementsWithElementChild(name).ToList();
                return elements.FirstOrDefault(f => f.Attributes.ContainsKey("id") && f.Attributes["id"] == id);
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }
    }
}
