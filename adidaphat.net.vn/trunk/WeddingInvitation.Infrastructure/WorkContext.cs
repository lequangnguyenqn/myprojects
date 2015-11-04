using System;
using System.Linq;
using System.Web;

namespace WeddingInvitation.Infrastructure
{
    public static class WorkContext
    {
        private const string CurrentUser = "WeddingInvitation.CURRENT_USER";
        public static int CurrentUserId
        {
            get
            {
                if (HttpContext.Current.Session[CurrentUser] == null)
                    return 0;
                return int.Parse(HttpContext.Current.Session[CurrentUser].ToString());
                
            }
            set { HttpContext.Current.Session[CurrentUser] = value; }
        }

        private const string MyOffice = "WeddingInvitation.MY_OFFICE";
        public static int[] MyOffices
        {
            get
            {
                if (HttpContext.Current.Session[MyOffice] == null)
                    return new int[]{};
                var offices = HttpContext.Current.Session[MyOffice].ToString();
                if (string.IsNullOrEmpty(offices))
                {
                    return new int[] {};
                }
                return offices.Split(',').Select(p => Convert.ToInt32(p)).ToArray();

            }
            set { HttpContext.Current.Session[MyOffice] = string.Join(",",value); }
        }

        private const string MyStorage = "WeddingInvitation.MY_STORAGE";
        public static int[] MyStorages
        {
            get
            {
                if (HttpContext.Current.Session[MyStorage] == null)
                    return new int[] { };
                var offices = HttpContext.Current.Session[MyStorage].ToString();
                if (string.IsNullOrEmpty(offices))
                {
                    return new int[] { };
                }
                return offices.Split(',').Select(p => Convert.ToInt32(p)).ToArray();

            }
            set { HttpContext.Current.Session[MyStorage] = string.Join(",", value); }
        }

        private const string MyOfficename = "WeddingInvitation.MY_OFFICE_NAME";
        public static string MyOfficeName
        {
            get
            {
                if (HttpContext.Current.Session[MyOfficename] == null)
                    return string.Empty;
                return HttpContext.Current.Session[MyOfficename].ToString();

            }
            set { HttpContext.Current.Session[MyOfficename] = value; }
        }

        private const string IS_SUPER_ADMIN = "WeddingInvitation.IS_SUPER_ADMIN";
        public static bool IsSuperAdmin
        {
            get
            {
                if (HttpContext.Current.Session[IS_SUPER_ADMIN] == null)
                    return true;
                return bool.Parse(HttpContext.Current.Session[IS_SUPER_ADMIN].ToString());
            }
            set { HttpContext.Current.Session[IS_SUPER_ADMIN] = value; }
        }

        private const string ROLES = "WeddingInvitation.ROLES";
        public static string[] Roles
        {
            get
            {
                if (HttpContext.Current.Session[ROLES] == null)
                    return new string[0];
                return (string[])HttpContext.Current.Session[ROLES];
            }
            set { HttpContext.Current.Session[ROLES] = value; }
        }

        private const string FullNameSession = "WeddingInvitation.FULL_NAME_SESSION";
        public static string FullName
        {
            get
            {
                if (HttpContext.Current.Session[FullNameSession] == null)
                    return string.Empty;
                return HttpContext.Current.Session[FullNameSession].ToString();

            }
            set { HttpContext.Current.Session[FullNameSession] = value; }
        }
    }
}
