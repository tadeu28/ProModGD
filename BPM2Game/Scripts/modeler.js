/**
 * bpmn-js-seed
 *
 * This is an example script that loads an embedded diagram file <diagramXML>
 * and opens it using the bpmn-js modeler.
 */

(function (BpmnModeler) {

  // create modeler
  var bpmnModeler = new BpmnModeler({
      container: '#canvas',
      propertiesPanel: {
          parent: '#js-properties-panel'
      }
  });
    
  $("#fileSelected").submit(function (event) {

      var inputElements = $("#fileSelected")["0"].outerText.split("\n");
      var filePath = inputElements[0].trim();

      var file = $('#input-1a').fileinput('getFileStack')[0];

      if (filePath.indexOf(".bpmn") > 0) {

          var read = new FileReader();

          read.readAsBinaryString(file);

          read.onloadend = function () {

              var xml = read.result;
              importXML(xml);
          }
          
      } else {
          alert("This file is not a bpmn file. Please try to reload other file.");
      }

      event.preventDefault();
    });
    
  
  // import function
  function importXML(xml) {

    $("#modelArea").removeClass("hide");
    
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
    saveButton.addEventListener('click', function(ev) {
        
      // get the diagram contents
      bpmnModeler.saveXML({ format: false }, function(err, xml) {
          
        var blob = new Blob([xml], { type: 'text/xml' });
        console.log(blob);
          
        if (err) {
            console.error('Did not possible to save the model.', err);
        } else {
            var formData = new FormData();
            formData.append("file", blob, $("#projectId").val() + ".txt");
            $.ajax({
                type: "POST",
                 url: "/Project/SalvarXML?id=" + $("#projectId").val(),
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (dados) {
                    if (dados) {
                        $("#save-button").notify(
                            "The model was save with success!",
                            {
                                className: 'success',
                                position: "bottom"
                            }
                        );
                    }
                }
            });
        }
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
      importXML(diagramXML);
  });

  if (edit) {
      var url = "http://localhost:23529/files/bpmn/" + modelFile;

      $.ajax({
          type: 'GET',
          url: url,
          processData: false,
          success: function (data) {
              importXML(data);
          }
      });
  }

})(window.BpmnJS, window.saveAs);