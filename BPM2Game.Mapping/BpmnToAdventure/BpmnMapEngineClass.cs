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
    class ElementNode
    {
        public Element Element { get; set; }
        public Element SeqIncomingElement { get; set; }
    }

    public class BpmnMapEngineClass
    {
        public Model Model { get; set; }
        public DesignMapping DesignMapping { get; set; }
        public Decimal ModelMappedScore { get; set; }
        public Decimal ModelElmentPrediction { get; set; }
        public List<GameDesignMappingElements> MappingList { get; set; }
        public List<DesignMappingScores> MappingScores { get; set; }
        public List<Exception> Errors { get; set; }

        private Decimal _elementMappingPrediction;

        public BpmnMapEngineClass()
        {
            ModelElmentPrediction = 0;
            _elementMappingPrediction = 0;

            Errors = new List<Exception>();
            MappingList = new List<GameDesignMappingElements>();
            MappingScores = new List<DesignMappingScores>();
        }

        public void StartMapping(DesignMapping designMapping)
        {
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
                        _elementMappingPrediction = bpmnElements.Count * assocElements.Count(ass => ass.Ruleses.All(a => a.Type.Id != 4));
                        ProcessMapping(assocElements, bpmnElements);
                    }
                }
            });

            ModelElmentPrediction = MappingScores.Sum(s => s.ExpectedElements);

            //score result
            ModelMappedScore = MappingList.Count / ModelElmentPrediction;
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
                
                var qBefore= MappingList.Count;

                ProcessRelationRules(rules, element, bpmnElements);

                var expectedMapping = rules.Exists(e => e.Type.Id == 4) ? 0 : bpmnElements.Count;
                if (element.ProcessElement.Metamodel.ToLower().Contains("sequenceflow"))
                {
                    expectedMapping = Convert.ToInt32(_elementMappingPrediction);
                }
                var qAfter = MappingList.Count - qBefore;

                var mappingScore = new DesignMappingScores()
                {
                    DesignMapping = DesignMapping,
                    ExpectedElements = expectedMapping,
                    MappedElements = qAfter,
                    GameGenreElement = element.GameGenreElement.Name,
                    GameGenreElementId = element.GameGenreElement.Id,
                    ModelElement = element.ProcessElement.Name,
                    ModelElementId = element.ProcessElement.Id
                };
                MappingScores.Add(mappingScore);
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
                else if ((element.ProcessElement.Metamodel.ToLower().Contains("process")) && (!element.ProcessElement.Metamodel.ToLower().Contains("subprocess")))
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
                }else if (bpmnElement.TypeName.ToLower() == "parallelgateway")
                {
                    ProcessBpmnProcessParallelGateways(rules, element, bpmnElement);
                }
                else if (bpmnElement.TypeName.ToLower() == "inclusivegateway")
                {
                    throw new Exception("Not implemented yet.");
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
                            const string ruleFormat = "[{0}] creates event ({1})";

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

                            if (MappingList.All(a => a.Descricao != ge.Descricao))
                            {
                                MappingList.Add(ge);
                            }
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
                            const string ruleFormat = "[{0}] needs of event [{1}]";

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

                            if (MappingList.All(a => a.Descricao != ge.Descricao))
                            {
                                MappingList.Add(ge);
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

        private void ProcessBpmnProcessSequenceFlow(AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                var sourceElement = FindBpmnElementById(bpmnElement.Attributes["sourceRef"], "id");

                if (
                    (!sourceElement.TypeName.ToLower().Contains("task")) &&
                    (!sourceElement.TypeName.ToLower().Contains("subprocess")) &&
                    (!sourceElement.TypeName.ToLower().Contains("boundary")) &&
                    (!sourceElement.TypeName.ToLower().Contains("start")))
                {
                    _elementMappingPrediction--;
                    return;
                }
                
                var targetElementNodes = FindElementIgnoringNonActivities(sourceElement, bpmnElement, new []{ "outgoing", "targetref" });
                foreach (var targetElementNode in targetElementNodes)
                {
                    var targetElement = targetElementNode.Element;

                    var sourceElementId = bpmnElement.Attributes["sourceRef"];
                    if (sourceElement.TypeName.ToLower().Contains("boundary"))
                    {
                        sourceElementId = sourceElement.Attributes["attachedToRef"];
                    }
                    
                    var sourceParent = FindOwnerElementByFlowNode(sourceElementId);
                    var targetParent = FindOwnerElementByFlowNode(targetElement.Attributes["id"]);

                    if ((targetParent != null) && (sourceParent != null))
                    {
                        const string ruleFormat = "[{0}] <---> [{1}] ([{2}])";

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

                        var targetElementNodes = FindElementIgnoringNonActivities(incomeTask, bpmnElement, new[] { "outgoing", "targetref" });

                        foreach (var targetElementNode in targetElementNodes)
                        {
                            var outgoingTask = targetElementNode.Element;

                            if (incomeTask != null && outgoingTask != null)
                            {
                                var seqFlowElement = FindConnectionElement(bpmnElement, targetElementNode);

                                var ruleFormat = "If [{0}] == [{1}] them [{2}]";
                                var description = String.Format(ruleFormat,
                                    incomeTask.Attributes.ContainsKey("name") ? incomeTask.Attributes["name"] : incomeTask.Attributes["id"],
                                    seqFlowElement != null ? (seqFlowElement.Attributes.ContainsKey("name") ? seqFlowElement.Attributes["name"] : seqFlowElement.Attributes["id"]) : "",
                                    outgoingTask.Attributes.ContainsKey("name") ? outgoingTask.Attributes["name"] : outgoingTask.Attributes["id"]);

                                if (bpmnElement.Attributes.ContainsKey("name") &&
                                    bpmnElement.Attributes["name"].Trim() != "")
                                {
                                    ruleFormat = "If [{0}] and [{1}] == [{2}] them [{3}]";
                                    description = String.Format(ruleFormat,
                                        incomeTask.Attributes.ContainsKey("name") ? incomeTask.Attributes["name"] : incomeTask.Attributes["id"],
                                        bpmnElement.Attributes.ContainsKey("name") ? bpmnElement.Attributes["name"] : bpmnElement.Attributes["id"],
                                        seqFlowElement != null ? (seqFlowElement.Attributes.ContainsKey("name") ? seqFlowElement.Attributes["name"] : seqFlowElement.Attributes["id"]) : "",
                                        outgoingTask.Attributes.ContainsKey("name") ? outgoingTask.Attributes["name"] : outgoingTask.Attributes["id"]);
                                }

                                var ge = new GameDesignMappingElements()
                                {
                                    AssociateElement = element,
                                    Descricao = description,
                                    DesignMapping = DesignMapping,
                                    GameGenreElement = element.GameGenreElement,
                                    ModelElementId = bpmnElement.Attributes["id"],
                                    IsManual = false
                                };

                                MappingList.Add(ge);
                            }
                        }

                        //foreach (var outgoing in bpmnElement.Elements["outgoing"])
                        //{
                        //    var value = outgoing.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value;
                        //    var seqFlowElement = FindBpmnElementById(value, "id");
                        //    if (seqFlowElement != null)
                        //    {
                        //        var outgoingTasks = FindElementIgnoringNonActivities(seqFlowElement, bpmnElement);
                        //        foreach (var outgoingTask in outgoingTasks)
                        //        {
                        //            const string ruleFormat = "If [{0}] == [{1}] them [{2}]";

                        //            if (incomeTask != null && outgoingTask != null)
                        //            {
                        //                var ge = new GameDesignMappingElements()
                        //                {
                        //                    AssociateElement = element,
                        //                    Descricao = String.Format(ruleFormat,
                        //                                                incomeTask.Attributes.ContainsKey("name") ? incomeTask.Attributes["name"] : incomeTask.Attributes["id"],
                        //                                                seqFlowElement.Attributes.ContainsKey("name") ? seqFlowElement.Attributes["name"] : seqFlowElement.Attributes["id"],
                        //                                                outgoingTask.Attributes.ContainsKey("name") ? outgoingTask.Attributes["name"] : outgoingTask.Attributes["id"]),
                        //                    DesignMapping = DesignMapping,
                        //                    GameGenreElement = element.GameGenreElement,
                        //                    ModelElementId = bpmnElement.Attributes["id"],
                        //                    IsManual = false
                        //                };

                        //                MappingList.Add(ge);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Errors.Add(new Exception("Can't possible to process the bpmn:" + bpmnElement.TypeName + " [" + bpmnElement.Attributes["id"] + "]", ex));
            }
        }

        private void ProcessBpmnProcessParallelGateways(List<AssociationRules> rules, AssociationConfElements element, Element bpmnElement)
        {
            try
            {
                //Check if is a Split Gateway. I know that if there is more than one outgoing element
                if (bpmnElement.Elements.ContainsKey("outgoing") && bpmnElement.Elements["outgoing"].Count > 1)
                {
                    var previousTaskNodes = FindElementIgnoringNonActivities(bpmnElement, bpmnElement, new[] { "incoming", "sourceref" });

                    if (previousTaskNodes.Count == 0)
                    {
                        throw new Exception("Some error occured into running " + bpmnElement.TypeName + " mapping.");
                    }

                    var targetElementNodes = FindElementIgnoringNonActivities(bpmnElement, bpmnElement, new[] {"outgoing", "targetref"});

                    var parallelTasksStr = "";
                    foreach (var targetElementNode in targetElementNodes)
                    {
                        var outgoingTask = targetElementNode.Element;
                        parallelTasksStr += "|" + "<" +
                                         (outgoingTask.Attributes.ContainsKey("name")
                                             ? outgoingTask.Attributes["name"]
                                             : outgoingTask.Attributes["id"]) + ">";
                    }

                    if (parallelTasksStr.Trim() != "")
                    {
                        parallelTasksStr = parallelTasksStr.Remove(0, 1);
                        parallelTasksStr = parallelTasksStr.Replace("|", " and ");
                    }

                    if (parallelTasksStr == "")
                    {
                        throw new Exception("Some error occured into running " + bpmnElement.TypeName + " mapping.");
                    }

                    foreach (var previousTaskNode in previousTaskNodes)
                    {
                        var previousTask = previousTaskNode.Element;

                        const string ruleString = "Perform [{0}] them perform in parallel [{1}]";
                        var description = String.Format(ruleString,
                                previousTask.Attributes.ContainsKey("name") ? previousTask.Attributes["name"] : previousTask.Attributes["id"],
                                parallelTasksStr);

                        var ge = new GameDesignMappingElements()
                        {
                            AssociateElement = element,
                            Descricao = description,
                            DesignMapping = DesignMapping,
                            GameGenreElement = element.GameGenreElement,
                            ModelElementId = bpmnElement.Attributes["id"],
                            IsManual = false
                        };

                        MappingList.Add(ge);
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

        private List<Element> FindAllBpmnElementById(String id, String idName)
        {
            try
            {
                return Model.Elements.Where(w => w.Attributes.ContainsKey(idName) && w.Attributes[idName] == id).ToList();
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }

        private Element FindBpmnElementByRespectivelyFields(String[] fields, String[] contents)
        {
            try
            {
                Element element = null;
                var elementList = Model.Elements;

                for (var i = 0; i < fields.Count(); i++)
                {
                    elementList = elementList.Where(w => w.Attributes.ContainsKey(fields[i]) && w.Attributes[fields[i]] == contents[i]).ToList();
                }

                element = elementList.FirstOrDefault();

                return element;
            }
            catch (Exception ex)
            {
                Errors.Add(ex);
                return null;
            }
        }

        private Element FindConnectionElement(Element elementFrom, ElementNode elementNodeTo)
        {
            try
            {
                foreach (var outgoing in elementFrom.Elements["outgoing"])
                {
                    var element = FindBpmnElementById(outgoing.Attributes["Value"], "id");
                    var nextElementNodes = FindElementIgnoringNonActivities(element, elementFrom, new[] { "outgoing", "targetref" });
                    foreach (var nextElementNode in nextElementNodes)
                    {
                        if ((nextElementNode.Element.Attributes["id"] == elementNodeTo.Element.Attributes["id"]) &&
                            (nextElementNode.SeqIncomingElement.Attributes["id"] == elementNodeTo.SeqIncomingElement.Attributes["id"]))
                        {
                            return element;
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

        private List<ElementNode> FindElementIgnoringNonActivities(Element source, Element bpmnElement, string [] attributes)
        {
            try
            {
                var lstElements = new List<ElementNode>();

                if (source.Elements.Any(a => a.Key.ToLower() == attributes[0]))
                {
                    if (source.Elements[attributes[0]] != null && source.Elements[attributes[0]].Count > 0)
                    {
                        foreach (var outgoing in source.Elements[attributes[0]])
                        {
                            var value = outgoing.Attributes.FirstOrDefault(f => f.Key.ToLower() == "value").Value;
                            var seqFlowElement = FindBpmnElementById(value, "id");
                            if (seqFlowElement != null)
                            {
                                var elementId =
                                    seqFlowElement.Attributes.FirstOrDefault(f => f.Key.ToLower() == attributes[1]).Value;
                                var element = FindBpmnElementById(elementId, "id");
                                if (element != null)
                                {
                                    if (element.TypeName.ToLower().Contains("intermediate"))
                                    {
                                        lstElements.AddRange(FindElementIgnoringNonActivities(element, bpmnElement, attributes));
                                    }
                                    else if (element.TypeName.ToLower().Contains("end"))
                                    {
                                        continue;
                                    }
                                    else if (element.TypeName.ToLower().Contains("gateway"))
                                    {
                                        lstElements.AddRange(FindElementIgnoringNonActivities(element, bpmnElement, attributes));
                                    }
                                    else
                                    {
                                        var elementNome = new ElementNode()
                                        {
                                            Element = element,
                                            SeqIncomingElement = seqFlowElement
                                        };

                                        lstElements.Add(elementNome);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (source.TypeName.ToLower().Contains("flow"))
                {
                    var elementId = source.Attributes.FirstOrDefault(f => f.Key.ToLower() == "targetref").Value;
                    var element = FindBpmnElementById(elementId, "id");
                    if (element != null)
                    {
                        if (element.TypeName.ToLower().Contains("intermediate"))
                        {
                            lstElements.AddRange(FindElementIgnoringNonActivities(element, bpmnElement, attributes));
                        }
                        else if (element.TypeName.ToLower().Contains("end"))
                        {
                            
                        }
                        else if (element.TypeName.ToLower().Contains("gateway"))
                        {
                            lstElements.AddRange(FindElementIgnoringNonActivities(element, bpmnElement, attributes));
                        }
                        else
                        {
                            var elementNome = new ElementNode()
                            {
                                Element = element,
                                SeqIncomingElement = source
                            };

                            lstElements.Add(elementNome);
                        }
                    }
                }
                else
                {
                    var elementNome = new ElementNode()
                    {
                        Element = FindBpmnElementById(bpmnElement.Attributes["targetRef"], "id"),
                        SeqIncomingElement = null
                    };

                    lstElements.Add(elementNome);
                }

                return lstElements;
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


