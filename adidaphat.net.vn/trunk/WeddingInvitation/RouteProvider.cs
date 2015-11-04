using System.Web.Mvc;

namespace WeddingInvitation
{
    public class RouteProvider : AreaRegistration
    {
        public override string AreaName { get { return "api"; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Api_default",
                "api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}