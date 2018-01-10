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
    public class BpmnMapEngineClass
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
                    ProcessMessageAssociations(rules, element, bpmnElement);
                    continue;
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("messageflow"))
                {
                    ProcessBpmnProcessMessageFlow(element, bpmnElement);
                    continue;
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("process"))
                {
                    ProcessBpmnProcessElement(rules, element, bpmnElement);
                    continue;
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("textannotation"))
                {
                    ProcessBpmnProcessTextAnnotation(rules, element, bpmnElement);
                    continue;
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("gateway"))
                {
                    ProcessBpmnProcessGateways(rules, element, bpmnElement);
                    continue;
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("intermediate"))
                {
                    ProcessBpmnProcessIntermidiateEvents(rules, element, bpmnElement);
                    continue;
                }
                else if (element.ProcessElement.Metamodel.ToLower().Contains("sequenceflow"))
                {
                    ProcessBpmnProcessSequenceFlow(element, bpmnElement);
                    continue;
                }

                if (rules != null && rules.Any(a => a.Type.Id == 6))
                {
                    //GetProcessFlows
                }else if (rules != null && rules.Any())
                {
                    var qtdAcceptedRules = ProcessRules(rules, element, bpmnElement);

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

        private int ProcessRules(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
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
                else if (rule.Operator == AssociationRuleOperator.Equals)
                {
                    qtdAcceptedRules += ProcessEquals(rule, element, bpmnElement);
                }
                else if (rule.Operator == AssociationRuleOperator.NotEquals)
                {
                    qtdAcceptedRules += ProcessNotEquals(rule, element, bpmnElement);
                }
                else if (rule.Operator == AssociationRuleOperator.Contains)
                {
                    qtdAcceptedRules += ProcessContains(rule, element, bpmnElement);
                }
            }

            return qtdAcceptedRules;
        }

        private int ProcessRuleHaveSomeContent(AssociationRules rule, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElement.Attributes.Where(attr => attr.Key.ToLower().Trim() == rule.Field.ToLower().Trim()).Any(attr => attr.Value.Trim() != ""))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElement.Elements.Where(e => e.Key.ToLower().Trim() == rule.Field.ToLower().Trim()).Any(a => a.Value != null && a.Value.Count > 0))
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
                Errors.Add(new Exception("Can't possible to process the ProcessRuleHaveSomeContent: " + element.ProcessElement.Name + " ["+ bpmnElement.Attributes["id"] + "]", ex));
                return 0;
            }
        }

        private int ProcessEquals(AssociationRules rule, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElement.Attributes.Where(attr => attr.Key.ToLower().Trim() == rule.Field.ToLower().Trim())
                                                       .Any(attr => attr.Value.ToLower().Trim() == rule.Rule.ToLower().Trim()))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElement.Elements.Where(e => e.Key.ToLower().Trim() == rule.Field.ToLower().Trim())
                                                     .Any(a => a.Value != null && a.Value.Count > 0))
                            {
                                var el = bpmnElement.Elements.FirstOrDefault(f => f.Key.ToLower().Trim() == rule.Field.ToLower().Trim());
                                foreach (var valObj in el.Value)
                                {
                                    if (
                                        valObj.Attributes.Any(
                                            f =>
                                                f.Key.Trim().ToLower() == "value" &&
                                                f.Value.Trim().ToLower() == rule.Rule.Trim().ToLower()))
                                    {
                                        return 1;
                                    }
                                }
                            }
                            break;
                        }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the ProcessEquals: " + element.ProcessElement.Name + " [" + bpmnElement.Attributes["id"] + "]", ex));
                return 0;
            }
        }

        private int ProcessContains(AssociationRules rule, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElement.Attributes.Where(attr => attr.Key.ToLower().Trim() == rule.Field.ToLower().Trim())
                                                       .Any(attr => attr.Value.ToLower().Trim().Contains(rule.Rule.ToLower().Trim())))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElement.Elements.Where(e => e.Key.ToLower().Trim() == rule.Field.ToLower().Trim())
                                                     .Any(a => a.Value != null && a.Value.Count > 0))
                            {
                                var el = bpmnElement.Elements.FirstOrDefault(f => f.Key.ToLower().Trim() == rule.Field.ToLower().Trim());
                                foreach (var valObj in el.Value)
                                {
                                    if (
                                        valObj.Attributes.Any(
                                            f =>
                                                f.Key.Trim().ToLower() == "value" &&
                                                f.Value.Trim().ToLower().Contains(rule.Rule.Trim().ToLower())))
                                    {
                                        return 1;
                                    }
                                }
                            }
                            break;
                        }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the ProcessEquals: " + element.ProcessElement.Name + " [" + bpmnElement.Attributes["id"] + "]", ex));
                return 0;
            }
        }

        private int ProcessNotEquals(AssociationRules rule, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElement.Attributes.Where(attr => attr.Key.ToLower().Trim() == rule.Field.ToLower().Trim())
                                                       .Any(attr => attr.Value.ToLower().Trim() != rule.Rule.ToLower().Trim()))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElement.Elements.Where(e => e.Key.ToLower().Trim() == rule.Field.ToLower().Trim())
                                                     .Any(a => a.Value != null && a.Value.Count > 0))
                            {
                                var el = bpmnElement.Elements.FirstOrDefault(f => f.Key.ToLower().Trim() == rule.Field.ToLower().Trim());
                                foreach (var valObj in el.Value)
                                {
                                    if (
                                        valObj.Attributes.Any(
                                            f =>
                                                f.Key.Trim().ToLower() == "value" &&
                                                f.Value.Trim().ToLower() != rule.Rule.Trim().ToLower()))
                                    {
                                        return 1;
                                    }
                                }
                            }
                            break;
                        }
                }

                return 0;
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the ProcessNotEquals: " + element.ProcessElement.Name + " [" + bpmnElement.Attributes["id"] + "]", ex));
                return 0;
            }
        }

        private int ProcessExists(AssociationRules rule, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                switch (rule.Type.Id)
                {
                    case 1:
                        {
                            if (bpmnElement.Attributes.Any(a => a.Key.ToLower() == rule.Field.ToLower().Trim()))
                            {
                                return 1;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (bpmnElement.Elements.Any(a => a.Key.ToLower() == rule.Field.ToLower().Trim()))
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
                Errors.Add(new Exception("Can't possible to process the ProcessExists: " + element.ProcessElement.Name + " [" + bpmnElement.Attributes["id"] + "]", ex));
                return 0;
            }
        }

        private void ProcessMessageAssociations(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
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

                    if (rules != null && rules.Any(a => a.Type.Id == 6))
                    {
                        //GetProcessFlows
                    }
                    else if (rules != null && rules.Any())
                    {
                        var qtdAcceptedRules = ProcessRules(rules, element, bpmnElement);

                        if (qtdAcceptedRules == rules.Count)
                        {
                            ProcessDefaultRelation(element, bpmnElement);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the ProcessMessageAssociations. " + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessGateways(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                if (bpmnElement.TypeName.ToLower() == "exclusivegateway")
                {
                    ProcessBpmnProcessExclusiveGateways(rules, element, bpmnElement);
                }else if (bpmnElement.TypeName.ToLower() == "inclusivegateway")
                {
                    
                }else if (bpmnElement.TypeName.ToLower() == "inclusivegateway")
                {
                    
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:" + bpmnElement.TypeName + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessIntermidiateEvents(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                if (bpmnElement.TypeName.ToLower().Contains("throw"))
                {
                    foreach (var seqIncomming in bpmnElement.Elements["incoming"])
                    {
                        var outgoingSeqFlow =
                            FindBpmnElementById(
                                seqIncomming.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value, "id");

                        var incomingTask = FindBpmnElementById(outgoingSeqFlow.Attributes["sourceRef"], "id");
                        if (incomingTask != null)
                        {
                            const string ruleFormat = "<{0}> creates event <{1}>";

                            var ge = new GameDesignMappingElements()
                            {
                                AssociateElement = element,
                                Descricao = String.Format(ruleFormat,
                                                            incomingTask.Attributes.ContainsKey("name") ? incomingTask.Attributes["name"] : incomingTask.Attributes["id"],
                                                            bpmnElement.Attributes.ContainsKey("name") ? bpmnElement.Attributes["name"] : bpmnElement.Attributes["id"]),
                                DesignMapping = DesignMapping,
                                GameGenreElement = element.GameGenreElement,
                                ModelElementId = bpmnElement.Attributes["id"],
                                IsManual = false
                            };

                            MappingList.Add(ge);
                        }
                    }
                }
                else if (bpmnElement.TypeName.ToLower().Contains("catch"))
                {
                    foreach (var seqOutgoing in bpmnElement.Elements["outgoing"])
                    {
                        var outgoingSeqFlow =
                            FindBpmnElementById(
                                seqOutgoing.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value, "id");

                        var outgoingTask = FindBpmnElementById(outgoingSeqFlow.Attributes["targetRef"], "id");
                        if (outgoingTask != null)
                        {
                            const string ruleFormat = "<{0}> needs of event <{1}>";

                            var ge = new GameDesignMappingElements()
                            {
                                AssociateElement = element,
                                Descricao = String.Format(ruleFormat,
                                                            outgoingTask.Attributes.ContainsKey("name") ? outgoingTask.Attributes["name"] : outgoingTask.Attributes["id"],
                                                            bpmnElement.Attributes.ContainsKey("name") ? bpmnElement.Attributes["name"] : bpmnElement.Attributes["id"]),
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
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:" + bpmnElement.TypeName + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessSequenceFlow(AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                var sourceElement = FindBpmnElementById(bpmnElement.Attributes["sourceRef"], "id");

                if (
                    (!sourceElement.TypeName.ToLower().Contains("task")) && 
                    (!sourceElement.TypeName.ToLower().Contains("subprocess")) &&
                    (!sourceElement.TypeName.ToLower().Contains("start")))
                    return;

                var targetElements = FindElementIgnoringNonActivities(sourceElement, bpmnElement);
                foreach (var targetElement in targetElements)
                {
                    var sourceParent = FindOwnerElementByFlowNode(bpmnElement.Attributes["sourceRef"]);
                    var targetParent = FindOwnerElementByFlowNode(targetElement.Attributes["id"]);

                    if ((targetParent != null) && (sourceParent != null))
                    {
                        const string ruleFormat = "<{0}> <---> <{1}> (<{2}>)";

                        var rule = String.Format(ruleFormat,
                            sourceParent.Attributes["name"].Trim() == ""
                                ? sourceParent.Attributes["id"]
                                : sourceParent.Attributes["name"],
                            targetParent.Attributes["name"].Trim() == ""
                                ? targetParent.Attributes["id"]
                                : targetParent.Attributes["name"],
                            targetElement.Attributes["id"].Trim() == ""
                                ? targetElement.Attributes["id"]
                                : targetElement.Attributes["name"]);

                        var ge = new GameDesignMappingElements()
                        {
                            AssociateElement = element,
                            Descricao = rule,
                            DesignMapping = DesignMapping,
                            GameGenreElement = element.GameGenreElement,
                            ModelElementId = bpmnElement.Attributes["id"],
                            IsManual = false
                        };

                        if (MappingList.All(a => a.Descricao != ge.Descricao))
                        {
                            MappingList.Add(ge);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:" + bpmnElement.TypeName + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessMessageFlow(AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                var sourceElement = FindOwnerElementByFlowNode(bpmnElement.Attributes["sourceRef"]);
                var targetElement = FindOwnerElementByFlowNode(bpmnElement.Attributes["targetRef"]);

                if ((targetElement != null) && (sourceElement != null))
                {
                    var ge = new GameDesignMappingElements()
                    {
                        AssociateElement = element,
                        Descricao = (sourceElement.Attributes["name"].Trim() == "" ? sourceElement.Attributes["id"] : sourceElement.Attributes["name"]) + " <---> "
                                + (targetElement.Attributes["name"].Trim() == "" ? targetElement.Attributes["id"] : targetElement.Attributes["name"]),
                        DesignMapping = DesignMapping,
                        GameGenreElement = element.GameGenreElement,
                        ModelElementId = bpmnElement.Attributes["id"],
                        IsManual = false
                    };

                    MappingList.Add(ge);
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:" + bpmnElement.TypeName + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessExclusiveGateways(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                //Check if is a Split Gateway. I know that if there is more than one outgoing element
                if (bpmnElement.Elements.ContainsKey("outgoing") && bpmnElement.Elements["outgoing"].Count > 1)
                {
                    var incoming = bpmnElement.Elements["incoming"].FirstOrDefault();
                    if (incoming != null)
                    {
                        var incomeSeqFlow =
                            FindBpmnElementById(
                                incoming.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value, "id");

                        var incomeTask = FindBpmnElementById(incomeSeqFlow.Attributes["sourceRef"], "id");

                        foreach (var outgoing in bpmnElement.Elements["outgoing"])
                        {
                            var value = outgoing.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value;
                            var seqFlowElement = FindBpmnElementById(value, "id");
                            if (seqFlowElement != null)
                            {
                                var outgoingTask = FindBpmnElementById(seqFlowElement.Attributes["targetRef"], "id");

                                const string ruleFormat = "If <{0}> == <{1}> them <{2}>";

                                if (incomeTask != null && outgoingTask != null)
                                {
                                    var ge = new GameDesignMappingElements()
                                    {
                                        AssociateElement = element,
                                        Descricao = String.Format(ruleFormat,
                                                                    incomeTask.Attributes.ContainsKey("name") ? incomeTask.Attributes["name"] : incomeTask.Attributes["id"],
                                                                    seqFlowElement.Attributes.ContainsKey("name") ? seqFlowElement.Attributes["name"] : seqFlowElement.Attributes["id"],
                                                                    outgoingTask.Attributes.ContainsKey("name") ? outgoingTask.Attributes["name"] : outgoingTask.Attributes["id"]),
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
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:" + bpmnElement.TypeName + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessElement(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                var partElement = FindBpmnElementById(bpmnElement.Attributes["id"], "processRef");
                if (partElement != null)
                {
                    if (rules.Any(a => a.Type.Id == 6))
                    {
                        //GetProcessFlows
                    } else if (rules != null && rules.Any())
                    {
                        var qtdAcceptedRules = ProcessRules(rules, element, partElement);

                        if (qtdAcceptedRules == rules.Count)
                        {
                            ProcessDefaultRelation(element, partElement);
                        }
                    }
                    else
                    {
                        ProcessDefaultRelation(element, partElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:Process." + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessTextAnnotation(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                var textAnnotation = bpmnElement.Elements.FirstOrDefault(f => f.Key.Trim().ToLower() == "text").Value;
                if (textAnnotation != null)
                {
                    foreach (var text in textAnnotation)
                    {
                        if (rules.Any(a => a.Type.Id == 6))
                        {
                            //GetProcessFlows
                        }
                        else if (rules != null && rules.Any())
                        {
                            var qtdAcceptedRules = ProcessRules(rules, element, text);

                            if (qtdAcceptedRules == rules.Count)
                            {
                                ProcessDefaultRelation(element, text, "value");
                            }
                        }
                        else
                        {
                            ProcessDefaultRelation(element, text, "value");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:TextAnnotation." + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessDefaultRelation(AssociationConfElements element, Element bpmnElement, String field = "")
        {
            try
            {
                var description = "";
                if (field.Trim() == "")
                {
                    description = bpmnElement.Attributes["name"] == ""
                        ? bpmnElement.Attributes["id"]
                        : bpmnElement.Attributes["name"];
                }
                else
                {
                    if (bpmnElement.Attributes.Any(a => a.Key.ToLower() == field.Trim().ToLower()))
                    {
                        description = bpmnElement.Attributes.FirstOrDefault(f => f.Key.ToLower() == field.Trim().ToLower()).Value;    
                    }
                }

                var ge = new GameDesignMappingElements()
                {
                    AssociateElement = element,
                    Descricao = description,
                    DesignMapping = DesignMapping,
                    GameGenreElement = element.GameGenreElement,
                    ModelElementId = (bpmnElement.Attributes.ContainsKey("id") ? bpmnElement.Attributes["id"] : ""),
                    IsManual = false
                };

                MappingList.Add(ge);
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to Process the Relation " + element.ProcessElement.Name + "." + " [" + bpmnElement.Attributes["id"] + "]", ex));
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

        private Element FindOwnerElementByFlowNode(String node)
        {
            try
            {
                var elements = Model.Elements.Where(w => w.TypeName.ToLower() == "lane").ToList();
                foreach (var lane in elements)
                {
                    var flownoderefs = lane.Elements.Where(w => w.Key.ToLower() == "flownoderef");
                    if (flownoderefs.Select(flownoderef => flownoderef.Value.FirstOrDefault(
                        f => f.Attributes.FirstOrDefault(a => a.Key.ToLower() == "value").Value == node)).Any(element => element != null))
                    {
                        return lane;
                    }
                }

                return Model.Elements.FirstOrDefault(f => f.Attributes["id"] == node);
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }

        private List<Element> FindElementIgnoringNonActivities(Element source, Element bpmnElement)
        {
            try
            {
                var lstElements = new List<Element>();

                if (source.Elements.Any(a => a.Key.ToLower() == "outgoing"))
                {
                    if (source.Elements["outgoing"] != null && source.Elements["outgoing"].Count > 0)
                    {
                        foreach (var outgoing in source.Elements["outgoing"])
                        {
                            var value = outgoing.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value;
                            var seqFlowElement = FindBpmnElementById(value, "id");
                            if (seqFlowElement != null)
                            {
                                var elementId =
                                    seqFlowElement.Attributes.FirstOrDefault(f => f.Key.ToLower() == "targetref").Value;
                                var element = FindBpmnElementById(elementId, "id");
                                if (element != null)
                                {
                                    if (element.TypeName.ToLower().Contains("intermediate"))
                                    {
                                        lstElements.AddRange(FindElementIgnoringNonActivities(element, bpmnElement));
                                    }
                                    else if (element.TypeName.ToLower().Contains("end"))
                                    {
                                        continue;
                                    }
                                    else if (element.TypeName.ToLower().Contains("gateway"))
                                    {
                                        lstElements.AddRange(FindElementIgnoringNonActivities(element, bpmnElement));
                                    }
                                    else
                                    {
                                        lstElements.Add(element);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    lstElements.Add(FindBpmnElementById(bpmnElement.Attributes["targetRef"], "id"));
                }

                return lstElements;
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }

        private Element FindElementIgnoringIntermidiateEvents(Element source)
        {
            try
            {
                if (source.Elements.Any(a => a.Key.ToLower() == "outgoing"))
                {
                    var element = source.Elements.FirstOrDefault(f => f.Key.ToLower() == "outgoing").Value.FirstOrDefault();
                    if (element != null)
                    {
                        var elementId = element.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value;
                        element = FindBpmnElementById(elementId, "id");
                        if (element != null)
                        {
                            if (element.Attributes.Any(a => a.Key.ToLower() == "targetref"))
                            {
                                elementId = element.Attributes.FirstOrDefault(f => f.Key.ToLower() == "targetref").Value;
                                element = FindBpmnElementById(elementId, "id");
                                if (element != null)
                                {
                                    if (element.TypeName.ToLower().Contains("intermediate"))
                                    {
                                        return FindElementIgnoringIntermidiateEvents(element);
                                    }
                                    else
                                    {
                                        return element;
                                    }
                                }
                            }
                        }
                    }
                }

                return null;
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
