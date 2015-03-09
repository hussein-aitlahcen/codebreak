using System.Web;
using System.Web.Optimization;

namespace Codebreak.App.Website
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var appBundle = new ScriptBundle("~/bundles/app").Include(
                        "~/Content/js/app.js");
            appBundle.Transforms.Clear();

            bundles.Add(appBundle);
            
            // Angular
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Content/js/angular.js",
                        "~/Content/js/angular-animate*",
                        "~/Content/js/loading-bar*"));

            // SignalR
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                        "~/Content/js/jquery.signalR*"));

            // I18n translation
            bundles.Add(new ScriptBundle("~/bundles/i18next").Include(
                        "~/Content/js/i18next*"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Content/js/bootstrap*"));

            // JQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-{version}.js",
                        "~/Content/js/jquery.unobtrusive-ajax*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Content/js/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/js/jquery.unobtrusive*",
                        "~/Content/js/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/css/bootstrap*"));

            // APP css base
            bundles.Add(new StyleBundle("~/Content/app").Include(
                        "~/Content/css/loading-bar.css",
                        "~/Content/css/app.css",
                        "~/Content/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/chat").Include(
                        "~/Content/css/chat.css"));

            bundles.Add(new StyleBundle("~/Content/home").Include(
                        "~/Content/css/home.css"));

            bundles.Add(new StyleBundle("~/Content/ladder").Include(
                        "~/Content/css/ladder.css"));

            bundles.Add(new StyleBundle("~/Content/social").Include(
                        "~/Content/css/social.css"));

            bundles.Add(new StyleBundle("~/Content/team").Include(
                        "~/Content/css/team.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}