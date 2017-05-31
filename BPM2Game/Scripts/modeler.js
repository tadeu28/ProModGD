/**
 * bpmn-js-seed
 *
 * This is an example script that loads an embedded diagram file <diagramXML>
 * and opens it using the bpmn-js modeler.
 */
(function(BpmnModeler) {

    
  // create modeler
  var bpmnModeler = new BpmnModeler({
    container: '#canvas'
  });

  // import function
  function importXML(xml) {

    // import diagram
    bpmnModeler.importXML(xml, function(err) {

      if (err) {
        return console.error('could not import BPMN 2.0 diagram', err);
      }

      var canvas = bpmnModeler.get('canvas');

      // zoom to fit full viewport
      canvas.zoom('fit-viewport');
    });
      
    // save diagram on button click
    var saveButton = document.querySelector('#save-button');
      

    saveButton.addEventListener('click', function() {

      // get the diagram contents
      bpmnModeler.saveXML({ format: true }, function(err, xml) {

        if (err) {
          console.error('diagram save failed', err);
        } else {
          console.info('diagram saved');
          console.info(xml);
        }

        alert('diagram saved (see console (F12))');
      });
    });
  }

  var diagramXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<definitions xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\"\r\n             xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\"\r\n             xmlns:omgdc=\"http://www.omg.org/spec/DD/20100524/DC\"\r\n             xmlns:omgdi=\"http://www.omg.org/spec/DD/20100524/DI\"\r\n             xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n             expressionLanguage=\"http://www.w3.org/1999/XPath\"\r\n             typeLanguage=\"http://www.w3.org/2001/XMLSchema\"\r\n             targetNamespace=\"\"\r\n             xsi:schemaLocation=\"http://www.omg.org/spec/BPMN/20100524/MODEL http://www.omg.org/spec/BPMN/2.0/20100501/BPMN20.xsd\">\r\n " +
  "<process id=\"Process_0m9u8n9\" /> " +
    "<bpmndi:BPMNDiagram id=\"sid-74620812-92c4-44e5-949c-aa47393d3830\"> " +
      "<bpmndi:BPMNPlane id=\"sid-cdcae759-2af7-4a6d-bd02-53f3352a731d\" bpmnElement=\"Process_0m9u8n9\" /> " +
      "<bpmndi:BPMNLabelStyle id=\"sid-e0502d32-f8d1-41cf-9c4a-cbb49fecf581\"> " +
        "<omgdc:Font name=\"Arial\" size=\"11\" isBold=\"false\" isItalic=\"false\" isUnderline=\"false\" isStrikeThrough=\"false\" /> " +
      "</bpmndi:BPMNLabelStyle> " +
      "<bpmndi:BPMNLabelStyle id=\"sid-84cb49fd-2f7c-44fb-8950-83c3fa153d3b\"> " +
      "  <omgdc:Font name=\"Arial\" size=\"12\" isBold=\"false\" isItalic=\"false\" isUnderline=\"false\" isStrikeThrough=\"false\" /> " +
     " </bpmndi:BPMNLabelStyle> " +
    "</bpmndi:BPMNDiagram> " +
  "</definitions>";

  // import xml
  importXML(diagramXML);

  $("#modelArea").addClass("hide");

  $('#newProcess').click(function () {
      $("#modelArea").removeClass("hide");
      importXML(diagramXML);
  });

})(window.BpmnJS);