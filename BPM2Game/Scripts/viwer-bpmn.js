/**
 * bpmn-js-seed
 *
 * This is an example script that loads an embedded diagram file <diagramXML>
 * and opens it using the bpmn-js viewer.
 */
 
(function (BpmnNavigatedViewer) {

  // create viewer
    var bpmnViewer = new BpmnNavigatedViewer({
    container: '#canvas'
  });

    var downloadBtn = document.querySelector('#save-download');
    downloadBtn.addEventListener("click", function (e) {
        bpmnViewer.saveXML({ format: true }, function (err, xml) {
            var encodedData = encodeURIComponent(xml);

            $('#save-download').addClass('active').attr({
                'href': 'data:application/bpmn20-xml;charset=UTF-8,' + encodedData,
                'download': $("#projectId").val() + ".bpmn"
            });
        });
    });

  // import function
  function importXML() {
      var url = "http://" + modelFile;
      $.ajax({
          type: 'GET',
          url: url,
          processData: false,
          success: function (data) {
              // import diagram
              bpmnViewer.importXML(data, function (err) {

                if (err) {
                 alert('Could not import BPMN 2.0 diagram', err);
                }

                var canvas = bpmnViewer.get('canvas');

                // zoom to fit full viewport
                canvas.zoom('fit-viewport');
              });
          }
      });
  }

  //Essa variável foi carregada na ViewModelling
  importXML();

})(window.BpmnJS);