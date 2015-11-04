using System;
using System.Web.Mvc;
using WeddingInvitation.Data;

namespace WeddingInvitation.Infrastructure.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            this.Roles = RoleType.KinhDoanh + "," + RoleType.Administrator;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    var result = new ViewResult();
        //    result.ViewName = "AccessDenied";
        //    result.MasterName = "";
        //    filterContext.Result = result;
        //}
    }
}
