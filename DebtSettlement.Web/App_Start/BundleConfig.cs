using System.Web.Optimization;

namespace DebtSettlement.Web
{
    /// <summary>
    /// Class BundleConfig.
    /// </summary>
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/libs/jquery/jquery-3.2.1.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/libs/jquery-validation/dist/localization/messages_ru.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/libs/jQuery-ui/jquery-ui.js",
                        "~/libs/jQuery-ui/i18n/jquery.datepicker-ru.js"
                        ));

            //bundles.Add(new StyleBundle("~/bundles/jqueryui/css").IncludeDirectory(
            //            "~/Content/themes", "*.css", true));
            bundles.Add(new StyleBundle("~/libs/jquery-ui-bootstrap/css").Include(
                        "~/libs/jQuery-ui/jquery-ui.css",
                        "~/libs/jqueryui-bootstrap-adapter/jqueryui-bootstrap-adapter.css"
                        //"~/libs/jquery-ui-bootstrap/jquery-ui-1.10.3.theme.css",
                        //"~/libs/jquery-ui-bootstrap/jquery-ui-1.10.3.custom.css"//,
                        //"~/libs/jquery-ui-bootstrap/jquery-ui-1.10.3.ie.css"
                        ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*",
                        "~/libs/es6-shim/es6-shim.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/vendors/tables/bootstrap-table.js",
                      "~/Scripts/vendors/tables/colResizable.min.js",
                      "~/Scripts/vendors/tables/bootstrap-table-resizable.js",
                      "~/Scripts/vendors/tables/bootstrap-table-locale.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                        "~/Scripts/jquery.datetimepicker.js",
                        "~/Libs/jquery-dateFormat/dist/jquery-dateFormat.js",
                        "~/libs/jquery.bindings/jquery.bindings.js",
                        "~/libs/bootstrap-notify/bootstrap-notify.js",
                        "~/libs/select2/dist/js/select2.full.js",
                        "~/libs/fogLoader/jquery.fogLoader.{version}.js",
                        "~/libs/jQuery-Mask-Plugin/jquery.mask.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/Scripts/DataTables/jquery.dataTables.odata.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/vendors/tables/bootstrap-table.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/libs/select2/dist/css/select2.css",
                      "~/libs/animate/animate.css",
                      //"~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/styles/main.css",
                      "~/Content/styles/slide.css"));
        }
    }
}
