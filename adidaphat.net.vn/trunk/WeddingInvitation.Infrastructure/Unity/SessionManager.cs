using System.Web;
using System.Web.SessionState;

namespace WeddingInvitation.Infrastructure.Unity
{
    /// <summary>
    /// Class manage session
    /// </summary>
    public static class SessionManager
    {
        private const string SessionCulture = "SessionCulture";

        public static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static string Culture
        {
            get
            {
                if (Session[SessionCulture] == null)
                    return "en-US";
                return Session[SessionCulture].ToString();
            }
            set { Session[SessionCulture] = value; }
        }
    }
}
