using System.Web.Mvc;
using WeddingInvitation.Infrastructure.Services;

namespace WeddingInvitation.Controllers
{
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paypal(string errorCode, string desc, string desc2)
        {
            var model = new PaypalErrorModel
                            {
                                ErrorCode = errorCode,
                                Desc = desc,
                                Desc2 = desc2
                            };
            return View(model);
        }
    }
}
