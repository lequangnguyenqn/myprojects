using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ShippingFeeController : ControllerBase<IShippingFeeRepository, ShippingFee>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;

        public ShippingFeeController(IUnitOfWork unitOfWork, IShippingFeeRepository repository,
            IMyOfficeRepository myOfficeRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index(int? mode)
        {
            return View(mode);
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var model = Repository.Search(search);

            var gridModel = new GridModel<ShippingFeeModel>
            {
                Data = model.Select(x => new ShippingFeeModel
                {
                    ShippingFeeId = x.ShippingFeeId,
                    ShippingFeeName = x.ShippingFeeName,
                    Cost = x.Cost,
                    MyOfficeId = x.MyOfficeId
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var model = new ShippingFeeModel { MyOffices = offices };
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new ShippingFeeModel()
            {
                MyOfficeId = entity.MyOfficeId,
                MyOffices = offices,
                Cost = entity.Cost,
                Note = entity.Note,
                ShippingFeeId = entity.ShippingFeeId,
                ShippingFeeName = entity.ShippingFeeName
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ShippingFeeModel myOfficeModel)
        {
            if (myOfficeModel.ShippingFeeId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").ToList();
                    myOfficeModel.MyOffices = offices;
                    return View("Create", myOfficeModel);
                }
                var myOffice = new ShippingFee()
                {
                    IsDeleted = false,
                    MyOfficeId = myOfficeModel.MyOfficeId,
                    Cost = myOfficeModel.Cost,
                    CreateDate = DateTime.Now,
                    Note = myOfficeModel.Note,
                    ShippingFeeName = myOfficeModel.ShippingFeeName,
                    CreateUserId = WorkContext.CurrentUserId
                };
                using (UnitOfWork)
                {
                    Repository.Insert(myOffice);
                }
                //Save success
                this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Phí vận chuyển"));
                return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").ToList();
                    myOfficeModel.MyOffices = offices;
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.ShippingFeeId);
                myOffice.Note = myOfficeModel.Note;
                myOffice.ShippingFeeName = myOfficeModel.ShippingFeeName;
                myOffice.MyOfficeId = myOfficeModel.MyOfficeId;
                myOffice.Cost = myOfficeModel.Cost;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Phí vận chuyển"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    entity.IsDeleted = true;
                }
                this.SetSuccessNotification("Phí vận chuyển đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Phí vận chuyển này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        public ActionResult GetCost(int id)
        {
            var entity = Repository.GetById(id);
            return Json(entity.Cost);
        }
    }
}
