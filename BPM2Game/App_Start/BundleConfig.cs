using System.Web;
using System.Web.Optimization;

namespace BPM2Game
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/notify.js",
                        "~/Scripts/DataTables/jquery.dataTables*",
                        "~/Scripts/DataTables/dataTables.bootstrap*",
                        "~/Scripts/checkListBox_bootstrap.js",
                        "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/fileinput*",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bpmn-js").Include(
                      "~/Scripts/process_modeler/process_modeler.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-custom.css",
                      "~/Content/bootstrap-fileinput/css/fileinput*",
                      "~/Content/DataTables/css/dataTables.bootstrap*",
                      "~/Content/site.css",
                      //"~/Scripts/bpmn-js/assets/diagram-js.css",
                      //"~/Scripts/bpmn-js/assets/bpmn-font/css/bpmn-embedded.css",
                      "~/Content/toastr.css",
                      "~/Content/flaticon.css"));
        }
    }
}
