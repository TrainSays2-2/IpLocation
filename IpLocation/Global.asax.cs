using IpLocation.App_Start;
using IpLocation.Models;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IpLocation
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            InitLogger();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BaseUpdater upd = new BaseUpdater(new GeoLite2Updater());

            logger.Info("\n___________Application_Start___________\n");
        }


        void InitLogger()
        {
            var config = new LoggingConfiguration();

            var target = new FileTarget()
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory + @"\logs\" + DateTime.Today.ToString() + ".log",
                CreateDirs = true,
                Layout = "${longdate}|${level}|${message}",
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveEvery = FileArchivePeriod.Day,
            };

            config.AddTarget("logfile", target);
            var rule = new LoggingRule("*", LogLevel.Info, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

        }

    }
}
