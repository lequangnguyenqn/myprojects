using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
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
    public class StorageController : ControllerBase<IStorageRepository, Storage>
    {
        private readonly IProductInStorageRepository _productInStorageRepository;

        public StorageController(IUnitOfWork unitOfWork, IStorageRepository repository,
            IProductInStorageRepository productInStorageRepository)
            : base(repository, unitOfWork)
        {
            _productInStorageRepository = productInStorageRepository;
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

            var gridModel = new GridModel<StorageModel>
            {
                Data = model.Select(x => new StorageModel
                {
                    StorageId = x.StorageId,
                    Desciption = x.Desciption,
                    StorageName =x.StorageName
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [ChildActionOnly]
        public ActionResult StoreNeedImport()
        {
            return View();
        }

        [GridAction]
        public ActionResult StoreNeedImportGridModel(string search)
        {
            var model = _productInStorageRepository.Search(search).Where(p => p.Amount < 1000).OrderBy(p => p.Amount);

            var gridModel = new GridModel<ProductInStorageModel>
            {
                Data = model.Select(x => new ProductInStorageModel
                {
                    StorageId = x.StorageId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.ProductName,
                    StorageName = x.Storage.StorageName,
                    Amount = x.Amount,
                    ProductInStorageId = x.ProductInStorageId
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var model = new StorageModel();
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var entity = Repository.GetById(id);
            var model = new StorageModel()
            {
                StorageId = entity.StorageId,
                Desciption =entity.Desciption,
                StorageName= entity.StorageName
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(StorageModel myOfficeModel)
        {
            if (myOfficeModel.StorageId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", myOfficeModel);
                }
                var myOffice = new Storage
                {
                    IsDeleted = false,
                    Desciption = myOfficeModel.Desciption,
                    StorageName = myOfficeModel.StorageName
                };
                using (UnitOfWork)
                {
                    Repository.Insert(myOffice);
                }
                SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Kho"));
                return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.StorageId);
                myOffice.Desciption = myOfficeModel.Desciption;
                myOffice.StorageName = myOfficeModel.StorageName;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Kho"));
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
                this.SetSuccessNotification("Kho đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Kho này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
