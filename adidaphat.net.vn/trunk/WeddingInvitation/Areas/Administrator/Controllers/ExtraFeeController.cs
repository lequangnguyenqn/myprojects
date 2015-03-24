using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ExtraFeeController : ControllerBase<IExtraFeeRepository, ExtraFee>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;

        public ExtraFeeController(IUnitOfWork unitOfWork, IExtraFeeRepository repository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var model = Repository.Search(search);

            var gridModel = new GridModel<ExtraFeeModel>
            {
                Data = model.Select(x => new ExtraFeeModel
                {
                    ExtraFeeId = x.ExtraFeeId,
                    AmountFrom =x.AmountFrom,
                    AmountTo = x.AmountTo,
                    Cost = x.Cost,
                    ExtraFeeName = x.ExtraFeeName
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var model = new ExtraFeeModel();
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var entity = Repository.GetById(id);
            var model = new ExtraFeeModel()
            {
                ExtraFeeId = entity.ExtraFeeId,
                AmountFrom = entity.AmountFrom,
                AmountTo = entity.AmountTo,
                Cost = entity.Cost,
                ExtraFeeName = entity.ExtraFeeName
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ExtraFeeModel myOfficeModel)
        {
            if (myOfficeModel.ExtraFeeId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", myOfficeModel);
                }
                var myOffice = new ExtraFee
                {
                    IsDeleted = false,
                    AmountFrom = myOfficeModel.AmountFrom,
                    AmountTo = myOfficeModel.AmountTo,
                    ExtraFeeName = myOfficeModel.ExtraFeeName,
                    Cost = myOfficeModel.Cost
                };
                using (UnitOfWork)
                {
                    Repository.Insert(myOffice);
                }
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.ExtraFeeId);
                myOffice.ExtraFeeName = myOfficeModel.ExtraFeeName;
                myOffice.AmountFrom = myOfficeModel.AmountFrom;
                myOffice.AmountTo = myOfficeModel.AmountTo;
                myOffice.Cost = myOfficeModel.Cost;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Phí phụ thêm"));
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
                this.SetSuccessNotification("Phí phụ thêm đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Phí phụ thêm này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
