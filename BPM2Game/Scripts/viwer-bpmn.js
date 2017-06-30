/**
 * bpmn-js-seed
 *
 * This is an example script that loads an embedded diagram file <diagramXML>
 * and opens it using the bpmn-js viewer.
 */
 
(function(BpmnViewer) {

  // create viewer
  var bpmnViewer = new BpmnViewer({
    container: '#canvas'
  });


  // import function
  function importXML(xml) {

    // import diagram
    bpmnViewer.importXML(xml, function(err) {

      if (err) {
       alert('Could not import BPMN 2.0 diagram', err);
      }

      var canvas = bpmnViewer.get('canvas');

      // zoom to fit full viewport
      canvas.zoom('fit-viewport');
    });
  }

    //Essa variável foi carregada na ViewModelling
  importXML(bpmnData);

})(window.BpmnJS);