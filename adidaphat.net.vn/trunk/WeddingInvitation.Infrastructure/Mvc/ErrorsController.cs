using System;
using System.Web.Mvc;
using WeddingInvitation.Infrastructure.Unity;

namespace WeddingInvitation.Infrastructure.Mvc
{
    public class ErrorsController : Controller
    {
        public ActionResult General(Exception exception)
        {
            return Json(new
            {
                resultcode = (int)ApiStatusCodes.BadRequest,
                resulttext = "BAD REQUEST"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
