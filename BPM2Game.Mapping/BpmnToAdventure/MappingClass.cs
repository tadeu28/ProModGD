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
                        ProcessMapping(assocElements, bpmnElements);
                    }
                }
            });

            MappingList = MappingList;
        }

        private void ProcessMapping(List<AssociationConfElements> elements, List<Element> bpmnElements)
        {
            foreach (var element in elements)
            {
                var rules = element.Ruleses.ToList();

                if (element.ProcessElement.ParentElement != null)
                {
                    var parentAssocElement = DbFactory.Instance
                        .AssociationConfElementRepository
                        .FindByElementMetamodel(element.ProcessElement.ParentElement.Metamodel);

                    if (parentAssocElement != null)
                    {
                        rules.AddRange(parentAssocElement.Ruleses);
                    }
                }

                ProcessRelationRules(rules, element, bpmnElements);
            }
        }

        private void ProcessRelationRules(List<AssociationRules> rules, AssociationConfElements element, List<Element> bpmnElements)
        {
            foreach (var bpmnElement in bpmnElements)
            {
                if (element.ProcessElement.Metamodel.ToLower().Contains("association"))
                {
                    ProcessMessageAssociations(element, bpmnElement);
                }

                if (rules != null && rules.Any())
                {
                    var qtdAcceptedRules = 0;
                    foreach (var rule in rules)
                    {
                        if (rule.Operator == AssociationRuleOperator.HaveSomeContent)
                        {
                            qtdAcceptedRules += ProcessRuleHaveSomeContent(rule, element, bpmnElement);
                        }
                        else if (rule.Operator == AssociationRuleOperator.Exists)
                        {
                            qtdAcceptedRules += ProcessExists(rule, element, bpmnElement);
                        }
                        else if (rule.Operator == AssociationRuleOperator.None)
                        {
                            return;
                        }
                    }

                    if (qtdAcceptedRules == rules.Count)
                    {
                        ProcessDefaultRelation(element, bpmnElement);
                    }
                }
                else
                {
                    ProcessDefaultRelation(element, bpmnElement);
                }
            }
        }

        private int ProcessRuleHaveSomeContent(AssociationRules rule, AssociationConfElements element, Element bpmnElements)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElements.Attributes.Where(attr => attr.Key.ToLower().Trim() == rule.Field.ToLower().Trim()).Any(attr => attr.Value != ""))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (bpmnElements.Attributes.ContainsKey("documentation"))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElements.Elements.Where(e => e.Key.ToLower().Trim() == rule.Field.ToLower().Trim()).Any(a => a.Value != null && a.Value.Count > 0))
                            {
                                return 1;
                            }
                            break;
                        }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the ProcessRuleHaveSomeContent: " + element.ProcessElement.Name, ex));
                return 0;
            }
        }

        private int ProcessExists(AssociationRules rule, AssociationConfElements element, Element bpmnElements)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElements.Attributes.Any(a => a.Key.ToLower() == rule.Field.ToLower().Trim()))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (bpmnElements.Attributes.Any(a => a.Key.ToLower() == "documentation"))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElements.Elements.Any(a => a.Key.ToLower() == rule.Field.ToLower().Trim()))
                            {
                                return 1;
                            }
                            break;
                        }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the ProcessRuleHaveSomeContent: " + element.ProcessElement.Name, ex));
                return 0;
            }
        }

        private void ProcessMessageAssociations(AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                if (bpmnElement.Attributes["id"] != "")
                {
                    if (element.ProcessElement.Metamodel.ToLower().Contains("input"))
                    {
                        var sourceRefId = bpmnElement.Elements["sourceRef"].FirstOrDefault();
                        var souceElement = FindBpmnElementById(sourceRefId.Attributes["Value"], "id");

                        Element target = null;
                        var els = GetElementsWithElementChild(bpmnElement.TypeName);
                        foreach (var e in els)
                        {
                            if (e.Elements.ContainsKey("dataInputAssociation") && e.Elements["dataInputAssociation"] != null)
                            {
                                if (e.Elements["dataInputAssociation"].Exists(x => x.Attributes["id"] == bpmnElement.Attributes["id"]))
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
                                Descricao = (souceElement.Attributes["name"].Trim() == "" ? souceElement.Attributes["id"] : souceElement.Attributes["name"]) + " <---> "
                                            + (target.Attributes["name"].Trim() == "" ? target.Attributes["id"] : target.Attributes["name"]),
                                DesignMapping = DesignMapping,
                                GameGenreElement = element.GameGenreElement,
                                ModelElementId = bpmnElement.Attributes["id"],
                                IsManual = false
                            };

                            MappingList.Add(ge);
                        }
                    }
                    else
                    {
                        var els = GetElementsWithElementChild(bpmnElement.TypeName);
                        Element source = null;
                        foreach (var e in els)
                        {
                            if (e.Elements.ContainsKey("dataOutputAssociation") && e.Elements["dataOutputAssociation"] != null)
                            {
                                if (e.Elements["dataOutputAssociation"].Exists(x => x.Attributes["id"] == bpmnElement.Attributes["id"]))
                                {
                                    source = e;
                                    break;
                                }
                            }
                        }

                        var targetList = bpmnElement.Elements["targetRef"];
                        foreach (var tg in targetList)
                        {
                            var target = FindBpmnElementById(tg.Attributes["Value"], "id");

                            if ((target != null) && (source != null))
                            {
                                var ge = new GameDesignMappingElements()
                                {
                                    AssociateElement = element,
                                    Descricao = (source.Attributes["name"].Trim() == "" ? source.Attributes["id"] : source.Attributes["name"]) + " <---> "
                                            + (target.Attributes["name"].Trim() == "" ? target.Attributes["id"] : target.Attributes["name"]),
                                    DesignMapping = DesignMapping,
                                    GameGenreElement = element.GameGenreElement,
                                    ModelElementId = bpmnElement.Attributes["id"],
                                    IsManual = false
                                };

                                MappingList.Add(ge);
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

        //private void ProcessTitle(AssociationConfElements element, Element bpmnElement)
        //{
        //    try
        //    {
        //        var ge = new GameDesignMappingElements()
        //        {
        //            AssociateElement = element,
        //            Descricao = (bpmnElement.Attributes["name"].Trim() == "" ? bpmnElement.Attributes["id"] : bpmnElement.Attributes["name"]),
        //            DesignMapping = DesignMapping,
        //            GameGenreElement = element.GameGenreElement,
        //            ModelElementId = bpmnElement.Attributes["id"],
        //            IsManual = false
        //        };

        //        MappingList.Add(ge);
        //    }
        //    catch (Exception ex)
        //    {
        //        Errors.Add(new Exception("Can't possible to Process the Rule Relation " + element.ProcessElement.Name + ".", ex));
        //    }
        //}

        private void ProcessDefaultRelation(AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                var ge = new GameDesignMappingElements()
                {
                    AssociateElement = element,
                    Descricao = (bpmnElement.Attributes["name"] == "" ? bpmnElement.Attributes["id"] : bpmnElement.Attributes["name"]),
                    DesignMapping = DesignMapping,
                    GameGenreElement = element.GameGenreElement,
                    ModelElementId = bpmnElement.Attributes["id"],
                    IsManual = false
                };

                MappingList.Add(ge);
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to Process the Relation " + element.ProcessElement.Name + ".", ex));
            }
        }

        private Element FindBpmnElementById(String id, String idName)
        {
            try
            {
                return Model.Elements.FirstOrDefault(w => w.Attributes.ContainsKey(idName) && w.Attributes[idName] == id);
            }
            catch (Exception ex)
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
    }
}
