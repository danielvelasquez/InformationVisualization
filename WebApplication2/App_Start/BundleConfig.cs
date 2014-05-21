using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace WebApplication2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/app/ajaxPrefilters.js",
                "~/Scripts/app/app.bindings.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/app/home.viewmodel.js",
                "~/Scripts/app/login.viewmodel.js",
                "~/Scripts/app/register.viewmodel.js",
                "~/Scripts/app/registerExternal.viewmodel.js",
                "~/Scripts/app/manage.viewmodel.js",
                "~/Scripts/app/userInfo.viewmodel.js",
                "~/Scripts/app/_run.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/ui").Include(
                 "~/Scripts/d3/d3.*",
                 //"~/Scripts/d3/d3.min.js",
                 "~/Scripts/d3/cloud.js",
                 "~/Scripts/JQRangeSlider/jQ*",
                 "~/Scripts/JQueryUI/jquery-ui*",
                 "~/Scripts/loopjSlider/simple-*",
                 "~/Scripts/bootstrap/bootstrap.js",
                 "~/Scripts/movieVis/actorscript.js",
                 "~/Scripts/movieVis/actorviz.js",
                // "~/Scripts/movieVis/linechart.js",
                 "~/Scripts/movieVis/moviescript.js",
                 "~/Scripts/movieVis/movieviz.js",
                 "~/Scripts/movieVis/plugins.js",
                 "~/Scripts/movieVis/CustomTooltip.js"
                 ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css",
                 "~/Content/JQRangeSlider/classic-min.css",
                 "~/Content/JQRangeSlider/classic.css",
                 "~/Content/JQRangeSlider/iThing-min.css",
                 "~/Content/JQRangeSlider/iThing.css",
                 "~/Content/JQueryUI/jquery-ui-1.10.1.custom.css",
                 "~/Content/JQueryUI/jquery-ui-1.10.1.custom.min.css",
                 "~/Content/loopjSlider/simple-*",
                 "~/Content/movieVis/bootstrap*",
                 "~/Content/movieVis/reset.css",
                 "~/Content/movieVis/style.css"
                 ));
            
        }
    }
}
