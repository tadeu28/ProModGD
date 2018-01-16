using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPMN;

namespace BPM2Game.Mapping.Bpmn
{
    public class BpmnElementNode
    {
        public Element Element { get; set; }
        public Element SequenceFlowIncomed { get; set; }
    }
}
