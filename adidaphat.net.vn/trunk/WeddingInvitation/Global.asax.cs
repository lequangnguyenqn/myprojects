using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using WeddingInvitation.Data;
using System.Web.Security;
using System.Data.Entity;
using WeddingInvitation.Infrastructure.Unity;
using WeddingInvitation.Infrastructure.Mvc;
using Quartz;
using Quartz.Impl;
using C5;
using WeddingInvitation.Infrastructure.Services;
using System.Threading;
using Elmah;
using System.Globalization;

namespace WeddingInvitation
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //Route Elmah
            routes.MapRoute("Admin_elmah", "elmah/{type}",
                new { action = "Index", controller = "Elmah", type = UrlParameter.Optional });
            routes.MapRoute("Admin_elmah_detail", "elmah/detail/{type}",
                new { action = "Detail", controller = "Elmah", type = UrlParameter.Optional }
            );
            routes.MapRoute("CreateOrder", "order",
                new { action = "CreateOrder", controller = "Home", type = UrlParameter.Optional },
                new string[] { "WeddingInvitation.Controllers" }
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new string[] { "WeddingInvitation.Controllers"}
            );

        }

        protected void Application_Start()
        {
            UnityConfiguration.Setup();
            // setup dbconext
            if (ConfigurationManager.AppSettings["CodeFirstEnabled"].ToLower() == "true")
            {
                Database.SetInitializer(new WeddingInvitationContextInitializer());
            }
            else
            {
                Database.SetInitializer<WeddingInvitationContext>(null);
            }

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AcquireRequestState()
        {
            const string culture = "vi-VN";
            CultureInfo ci = CultureInfo.GetCultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}