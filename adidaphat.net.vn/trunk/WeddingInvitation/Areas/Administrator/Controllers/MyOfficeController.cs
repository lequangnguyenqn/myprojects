using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [SuperAdminAuthorize]
    public class MyOfficeController : ControllerBase<IMyOfficeRepository, MyOffice>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStorageRepository _storageRepository;

        public MyOfficeController(IUnitOfWork unitOfWork, IMyOfficeRepository repository, IUserRepository userRepository, IStorageRepository storageRepository)
            : base(repository, unitOfWork)
        {
            _userRepository = userRepository;
            _storageRepository = storageRepository;
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

            var gridModel = new GridModel<MyOfficeModel>
            {
                Data = model.Select(x => new MyOfficeModel
                                             {
                                                 Address = x.Address,
                                                 MyOfficeId = x.MyOfficeId,
                                                 OfficeName = x.OfficeName,
                                                 PhoneNumber = x.PhoneNumber,
                                                 Fax = x.Fax,
                                                 IsRetailCustomer = x.IsRetailCustomer
                                             })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var storages = _storageRepository.Search("").ToList();
            var model = new MyOfficeModel { Storages = storages};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var entity = Repository.GetById(id);
            var storages = _storageRepository.Search("").ToList();
            var model = new MyOfficeModel()
            {
                Address = entity.Address,
                MyOfficeId = entity.MyOfficeId,
                OfficeName = entity.OfficeName,
                PhoneNumber = entity.PhoneNumber,
                Fax = entity.Fax,
                IsRetailCustomer = entity.IsRetailCustomer,
                Storages = storages,
                StorageId = entity.StorageId
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(MyOfficeModel myOfficeModel)
        {
            if (myOfficeModel.MyOfficeId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var storages = _storageRepository.Search("").ToList();
                    myOfficeModel.Storages = storages;
                    return View("Create", myOfficeModel);
                }
                var myOffice = new MyOffice()
                {
                    IsDeleted =false,
                    Address = myOfficeModel.Address,
                    OfficeName = myOfficeModel.OfficeName,
                    PhoneNumber = myOfficeModel.PhoneNumber,
                    Fax = myOfficeModel.Fax,
                    IsRetailCustomer = myOfficeModel.IsRetailCustomer,
                    StorageId = myOfficeModel.StorageId
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
                    var storages = _storageRepository.Search("").ToList();
                    myOfficeModel.Storages = storages;
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.MyOfficeId);
                myOffice.Address = myOfficeModel.Address;
                myOffice.OfficeName = myOfficeModel.OfficeName;
                myOffice.PhoneNumber = myOfficeModel.PhoneNumber;
                myOffice.Fax = myOfficeModel.Fax;
                myOffice.IsRetailCustomer = myOfficeModel.IsRetailCustomer;
                myOfficeModel.StorageId = myOfficeModel.StorageId;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Chi nhánh"));
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
                this.SetSuccessNotification("Chi nhánh đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Chi nhánh này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
