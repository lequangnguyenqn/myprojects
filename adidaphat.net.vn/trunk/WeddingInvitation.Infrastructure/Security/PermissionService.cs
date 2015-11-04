using System;
using System.Linq;
using Quartz.Util;

namespace WeddingInvitation.Infrastructure.Security
{
    public class PermissionService: IPermissionService
    {
        public PermissionService()
        {
            
        }

        public bool Authorize(string roleName, string[] roles)
        {
            if (String.IsNullOrEmpty(roleName))
                return false;
            return roles.Any(p => p.TrimEmptyToNull().Equals(roleName.TrimEmptyToNull()));
        }

        public bool Authorize(string roleName)
        {
            string[] roles = WorkContext.Roles;
            return Authorize(roleName, roles);
        }

        public bool Authorize(string[] roleNames, string[] roles)
        {
            foreach (var item in roleNames)
            {
                if (Authorize(item, roles))
                    return true;
            }
            return false;
        }
    }
}
