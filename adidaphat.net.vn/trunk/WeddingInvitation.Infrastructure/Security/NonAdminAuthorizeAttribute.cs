using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WeddingInvitation.Data;

namespace WeddingInvitation.Infrastructure.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NonAdminAuthorizeAttribute : AuthorizeAttribute
    {
        public NonAdminAuthorizeAttribute()
        {
            this.Roles = RoleType.KeToan + "," + RoleType.KinhDoanh + "," + RoleType.Administrator;
        }
    }
}
