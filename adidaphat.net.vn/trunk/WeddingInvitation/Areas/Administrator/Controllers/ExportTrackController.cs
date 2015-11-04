using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ExportTrackController : ControllerBase<IExportTrackRepository, ExportTrack>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IExportDetailRepository _exportDetailRepository;

        public ExportTrackController(IUnitOfWork unitOfWork, IExportTrackRepository repository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository,
            IUserRepository userRepository, IStorageRepository storageRepository,
            ICustomerRepository customerRepository, IExportDetailRepository exportDetailRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _storageRepository = storageRepository;
            _customerRepository = customerRepository;
            _exportDetailRepository = exportDetailRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            var customers = _customerRepository.Search("").Where(p=> WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var products = _productRepository.Search("").ToList();
            var model = new ExportTrackModel
            {
                Customers = customers,
                MyOffices = offices,
                Products = products
            };
            return View(model);
        }

        [GridAction]
        public ActionResult GridModel(ReportFilterModel filterModel)
        {

            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            //Filter by date
            if (filterModel.FromDate.HasValue)
            {
                fromDate = filterModel.FromDate.Value.Date;
                isAllFromDate = false;
            }
            if (filterModel.ToDate.HasValue)
            {
                toDate = filterModel.ToDate.Value.Date.AddDays(1);
                isAllToDate = false;
            }
            var fromListCustomers = new int[1];
            var isGetAllFromCustomers = true;
            //Filter by deliver to
            if (filterModel.ListCustomers != null && filterModel.ListCustomers.Count() > 0)
            {
                fromListCustomers = filterModel.ListCustomers;
                isGetAllFromCustomers = false;
            }
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listProducts = new int[1];
            var isGetAllListProducts = true;
            //Filter by deliver to
            if (filterModel.ListProducts != null && filterModel.ListProducts.Count() > 0)
            {
                listProducts = filterModel.ListProducts;
                isGetAllListProducts = false;
            }

            var model = from x in Repository.Search(filterModel.search).Where(p => (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                (fromListCustomers.Contains(p.Order.CustomerId) || (isGetAllFromCustomers)) &&
                (p.ExportDetails.Any(k => listProducts.Contains(k.ProductId)) || (isGetAllListProducts)) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        join m in _myOfficeRepository.GetAll() on x.MyOfficeId equals m.MyOfficeId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _customerRepository.GetAll() on x.Order.CustomerId equals s.CustomerId into s1
                        from s2 in s1.DefaultIfEmpty()
                        select new ExportTrackModel
                        {
                            ExportTrackId = x.ExportTrackId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            MyOfficeName = (m2 == null ? String.Empty : m2.OfficeName),
                            CustomerName = (s2 == null ? String.Empty : s2.CustomerName)
                        };

            var gridModel = new GridModel<ExportTrackModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var products = _productRepository.Search("").ToList();
            var users = _userRepository.Search("").ToList();
            var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new ExportTrackModel { MyOffices = offices, Products = products, Users = users, Storages = storages};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var products = _productRepository.Search("").ToList();
            var users = _userRepository.Search("").ToList();
            var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var entity = Repository.GetById(id);
            var model = new ExportTrackModel()
            {
                ExportTrackId = entity.ExportTrackId,
                Note = entity.Note,
                MyOfficeId = entity.MyOfficeId,
                MyOffices = offices,
                Products = products,
                Users = users,
                Storages = storages
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ExportTrackModel myOfficeModel)
        {
            if (myOfficeModel.ExportTrackId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var products = _productRepository.Search("").ToList();
                    var users = _userRepository.Search("").ToList();
                    var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
                    myOfficeModel.MyOffices = offices;
                    myOfficeModel.Products = products;
                    myOfficeModel.Users = users;
                    myOfficeModel.Storages = storages;
                    return View("Create", myOfficeModel);
                }
                var myOffice = new ExportTrack
                {
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = myOfficeModel.Note,
                    MyOfficeId = myOfficeModel.MyOfficeId,
                    ReceivedUserId = myOfficeModel.ReceivedUserId
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
                    var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var products = _productRepository.Search("").ToList();
                    var users = _userRepository.Search("").ToList();
                    var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
                    myOfficeModel.MyOffices = offices;
                    myOfficeModel.Products = products;
                    myOfficeModel.Users = users;
                    myOfficeModel.Storages = storages;
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.ExportTrackId);
                myOffice.Note = myOfficeModel.Note;
                myOffice.MyOfficeId = myOfficeModel.MyOfficeId;
                myOffice.ReceivedUserId = myOfficeModel.ReceivedUserId;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Xuất kho"));
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
                this.SetSuccessNotification("Xuất kho đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Xuất kho này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        public ActionResult ListExportDetail(int id)
        {
            var model = new ExportTrackModel
            {
                ExportTrackId = id
            };
            return View("_ListExportDetail", model);
        }
        [GridAction]
        public ActionResult ExportDetailGridModel(int? id)
        {
            if (id.HasValue)
            {
                var transferItems = _exportDetailRepository.Search("").Where(p => p.ExportTrackId == id.Value).Select(p => new ExportDetailModel
                {
                    Amount = p.Amount,
                    ProductId = p.ProductId,
                    ProductName = p.Product.ProductName,
                    CategoryName = p.Product.Category.CategoryName
                }).ToList();
                var gridModel1 = new GridModel<ExportDetailModel>
                {
                    Data = transferItems
                };
                return new JsonResult
                {
                    Data = gridModel1
                };
            }
            return new JsonResult
            {
                Data = new GridModel<ExportDetailModel>
                {
                    Data = new List<ExportDetailModel>()
                }
            };
        }
    }
}
