using System.Web.Mvc;
using Elmah;

namespace WeddingInvitation.Infrastructure.Mvc
{
    public class ElmahHandledErrorLoggerFilter : IExceptionFilter
    {

        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            // Log only handled exceptions, because all others will be caught by ELMAH anyway.
            if (filterContext.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
        }

        #endregion
    }
}
