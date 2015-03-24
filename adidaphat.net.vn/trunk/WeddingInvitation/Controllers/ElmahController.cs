using System;
using System.Web.Mvc;

namespace WeddingInvitation.Controllers
{
    /// <summary>
    //In normal setup, we use http://localhost:[port]/elmah.axd to access the log page, which is not MVC way. 
    //I create a manual work (rewrite path) for "elmah.axd" in the ElmahController.axd with the help of ErrorLogPageFactory.cs, please have a look
    //That is why we can access the route like this http://localhost:[port]/elmah/rss (not http://localhost:[port]/elmah?axd/rss)
    //So i comment out this line & hanlders section in the Web.config like so
    //*********************************************************************************************
    //routes.IgnoreRoute("elmah.axd");  //in Global.asax.cs
    //<httpHandlers>
    //<!--<add verb="POST,GET,HEAD" path="elmah.axd" type="Elmah.ErrorLogPageFactory, Elmah" /> -->
    //</httpHandlers>
    //*********************************************************************************************
    //NOTE:
    //Make this addornment [Authorize(Users = "Admin")]  to class ElmahController //if we want only "special" Users will see the log
    //I comment out during development
    /// </summary>
    //[Authorize(Users = "Admin")] 
    public class ElmahController : Controller
    {
        /// <summary>
        /// This is used for elmah error loging stuff
        /// this is called when we open this page
        /// http://localhost:[port]/elmah
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Index(string type)
        {
            return new ElmahResult(type);
        }

        /// <summary>
        /// This is used for elmah error loging stuff
        /// this is called when we open this page
        /// http://localhost:[port]/elmah/detail
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult Detail(string type)
        {
            return new ElmahResult(type);

        }

        public class ElmahResult : ActionResult
        {
            private string _resourceType;

            public ElmahResult(string type)
            {
                _resourceType = type;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                //HUYDO: Please check the source of ErrorLogPageFactory.cs for how it works
                var factory = new Elmah.ErrorLogPageFactory();
                if (!string.IsNullOrEmpty(_resourceType))
                {
                    var pathInfo = "." + _resourceType;
                    System.Web.HttpContext.Current.RewritePath(_resourceType != "stylesheet"
                            ? System.Web.HttpContext.Current.Request.Path.Replace(String.Format("/{0}", _resourceType), string.Empty)
                            : System.Web.HttpContext.Current.Request.Path, pathInfo, System.Web.HttpContext.Current.Request.QueryString.ToString());
                }
                //let the Elmah factory handle this normally
                var handler = factory.GetHandler(System.Web.HttpContext.Current, null, null, null);
                handler.ProcessRequest(System.Web.HttpContext.Current);
            }
        }

    }
}
