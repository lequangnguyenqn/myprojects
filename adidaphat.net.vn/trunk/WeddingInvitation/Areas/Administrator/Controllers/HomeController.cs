using System.Web.Mvc;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Users;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase<IUserRepository, User>
    {
        public HomeController(IUnitOfWork unitOfWork,
                              IUserRepository repository)
            : base(repository, unitOfWork)
        {
        }
        //
        // GET: /Administrator/Home/

        public ActionResult Index()
        {
            return View();
        }

        #region Giao Hàng
        public ActionResult OrderCancelForDelivery()
        {
            return View("_OrderCancelForDelivery");
        }
        #endregion

        #region Bộ phận in
        public ActionResult OrderCancelForPrint()
        {
            return View("_OrderCancelForPrint");
        }
        #endregion

        #region Bộ phận kho
        public ActionResult OrderCancelForStorage()
        {
            return View("_OrderCancelForStorage");
        }

        public ActionResult StorageStaffApprove()
        {
            return View("_StorageStaffApprove");
        }

        public ActionResult StorageStaffApproveExport()
        {
            return View("_StorageStaffApproveExport");
        }
        #endregion

        #region Quản Lý Chung
        public ActionResult OrderCancelForManager()
        {
            return View("_OrderCancelForManager");
        }

        public ActionResult ManagerApprove()
        {
            return View("_ManagerApprove");
        }
        #endregion

    }
}
