using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ImportTrackController : ControllerBase<IImportTrackRepository, ImportTrack>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductInStorageRepository _productInStorageRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private string ImportDetailSession = "ImportDetailSession";
        public List<ImportDetailModel> ImportDetails
        {
            get
            {
                if (Session[ImportDetailSession] == null)
                {
                    var importDetails = new List<ImportDetailModel>();
                    Session[ImportDetailSession] = importDetails;
                    return importDetails;
                }
                return (List<ImportDetailModel>)Session[ImportDetailSession];
            }
            set { Session[ImportDetailSession] = value; }
        }

        private string ImportTrackSessionString = "ImportTrackSessionString";
        public ImportTrackModel ImportTrackSession
        {
            get
            {
                if (Session[ImportTrackSessionString] == null)
                {
                    var orderModel = new ImportTrackModel();
                    Session[ImportTrackSessionString] = orderModel;
                    return orderModel;
                }
                return (ImportTrackModel)Session[ImportTrackSessionString];
            }
            set { Session[ImportTrackSessionString] = value; }
        }

        public ImportTrackController(IUnitOfWork unitOfWork, IImportTrackRepository repository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository,
            IProductInStorageRepository productInStorageRepository, IStorageRepository storageRepository,
            IImportDetailRepository importDetailRepository, ICategoryRepository categoryRepository,
            IUserRepository userRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _productInStorageRepository = productInStorageRepository;
            _storageRepository = storageRepository;
            _importDetailRepository = importDetailRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new ImportTrackModel
            {
                Storages = storages
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
            var toListStorages = new int[1];
            var isGetAllToStorages = true;
            //Filter by deliver to
            if (filterModel.ToListStorages != null && filterModel.ToListStorages.Count() > 0)
            {
                toListStorages = filterModel.ToListStorages;
                isGetAllToStorages = false;
            }
            var model = from x in Repository.Search(filterModel.search).Where(p => WorkContext.MyStorages.Contains(p.ToStorageId) &&
                (toListStorages.Contains(p.ToStorageId) || (isGetAllToStorages && WorkContext.MyStorages.Contains(p.ToStorageId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _userRepository.GetAll() on x.ApproveFromStorageStaffId equals s.UserId into s1
                        from s2 in s1.DefaultIfEmpty()
                        select new ImportTrackModel
                        {
                            ImportTrackId = x.ImportTrackId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            StorageId = x.ToStorageId,
                            ImportUserName = x.CreateUser.FirstName + " " + x.CreateUser.LastName,
                            ApproveFromManagerName =  (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            ApproveFromStorageStaffName = (s2 == null ? String.Empty : s2.FirstName + " " + s2.LastName),
                        };

            var gridModel = new GridModel<ImportTrackModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create(int? mode, bool? blank)
        {
            if (!blank.HasValue || blank.Value)
            {
                ImportDetails = new List<ImportDetailModel>();
            }
            var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new ImportTrackModel 
            {
                CreateDate = ImportTrackSession.CreateDate!= DateTime.MinValue ? ImportTrackSession.CreateDate : DateTime.Now,
                StorageId = ImportTrackSession.StorageId,
                Note = ImportTrackSession.Note,
                Mode = mode, 
                Storages = storages
            };
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var products = _productRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new ImportTrackModel
            {
                ImportTrackId = entity.ImportTrackId,
                Note = entity.Note,
                Products = products,
                CreateDate = entity.CreateDate
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ImportTrackModel model)
        {
            if (model.ImportTrackId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var products = _productRepository.Search("").ToList();
                    var storages = _storageRepository.Search("").ToList();
                    model.Products = products;
                    model.Storages = storages;
                    return View("Create", model);
                }
                if (!ImportDetails.Any())
                {
                    SetErrorNotification(string.Format("Bạn chưa có sản phẩm nào."));
                    return RedirectToAction("Create", new { area = "Administrator" });
                }
                var entity = new ImportTrack
                {
                    IsDeleted = false,
                    CreateDate = model.CreateDate,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = model.Note,
                    ToStorageId = model.StorageId,
                    ImportDetails = new List<ImportDetail>()
                };
                foreach (var orderDetailModel in ImportDetails)
                {
                    entity.ImportDetails.Add(new ImportDetail
                    {
                        Amount = orderDetailModel.Amount,
                        ProductId = orderDetailModel.ProductId,
                        Note = orderDetailModel.Note,
                        IsDeleted = false
                    });
                }
                using (UnitOfWork)
                {
                    Repository.Insert(entity);
                }
                
            }
            ImportTrackSession = new ImportTrackModel();
            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Nhập kho"));
            return RedirectToAction("Create", new { area = "Administrator" });
        }

        public ActionResult Delete(int id, int storageId)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    entity.IsDeleted = true;
                }
                this.SetSuccessNotification("Nhập kho đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Nhập kho này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        public ActionResult ListImportDetail(int id)
        {
            var model = new ImportTrackModel
            {
                ImportTrackId = id
            };
            return View("_ListImportDetail", model);
        }
        [GridAction]
        public ActionResult GridModelImportDetail(int? id)
        {
            if (id.HasValue)
            {
                var orderDetails = _importDetailRepository.Search("").Where(p => p.ImportTrackId == id.Value).Select(p => new ImportDetailModel
                {
                    Amount = p.Amount,
                    ProductId = p.ProductId,
                    ProductName = p.Product.ProductName,
                    CategoryName = p.Product.Category.CategoryName
                }).ToList();
                var gridModel1 = new GridModel<ImportDetailModel>
                {
                    Data = orderDetails
                };
                return new JsonResult
                {
                    Data = gridModel1
                };
            }
            return new JsonResult
            {
                Data = new GridModel<ImportDetailModel>
                {
                    Data = ImportDetails
                }
            };
        }

        public ActionResult SaveImportTrackToSession(ImportTrackModel model)
        {
            ImportTrackSession.CreateDate = model.CreateDate;
            ImportTrackSession.Note = model.Note;
            ImportTrackSession.StorageId = model.StorageId;
            return Json(true);
        }

        #region Import Detail

        public ActionResult CreateImportDetail(int id)
        {
            var categories = _categoryRepository.Search("").ToList();
            var model = new ImportDetailModel { Categories = categories, ImportTrackId = id};
            return View("_CreateImportDetail",model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult SaveImportDetail(ImportDetailModel model)
        {
            if (model.ImportDetailId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var categories = _categoryRepository.Search("").ToList();
                    model.Categories = categories;
                    return View("_CreateImportDetail", model);
                }
                Random rnd = new Random();
                var products = _productRepository.Search("").Where(p => model.ProductIds.Contains(p.ProductId)).ToList();
                foreach (var item in products)
                {
                    int importDetailId = rnd.Next(-9999, -1);
                    ImportDetails.Add(new ImportDetailModel
                    {
                        ImportDetailId = importDetailId,
                        Amount = model.Amount,
                        Note = model.Note,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ImportTrackId = model.ImportTrackId,
                        CategoryName = item.Category.CategoryName
                    });
                }
            }
            this.SetSuccessNotification(string.Format("{0} đã được thêm vào thành công.", "Sản phẩm"));
            return RedirectToAction("Create", "ImportTrack", new { area = "Administrator", mode = 1, blank = false });
        }

        public ActionResult DeleteImportDetail(int id)
        {
            try
            {
                ImportDetails = ImportDetails.Where(p => p.ImportDetailId != id).ToList();
                this.SetSuccessNotification("Sản phẩm đã được bỏ khỏi đơn hàng.");
                return RedirectToAction("Create", "Order", new { area = "Administrator" });
            }
            catch
            {
                this.SetErrorNotification("Sản phẩm này không thể xóa!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
        #endregion

        #region Quản Lý Xác Nhận Nhập Kho
        public ActionResult ManagerApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult ManagerApproveGridModel(string search)
        {
            var model = Repository.Search(search).Where(p => WorkContext.MyStorages.Contains(p.ToStorageId) && !p.ApproveFromManager);

            var gridModel = new GridModel<ImportTrackModel>
            {
                Data = model.Select(x => new ImportTrackModel
                {
                    ImportTrackId = x.ImportTrackId,
                    CreateDate = x.CreateDate,
                    Note = x.Note,
                    StorageId = x.ToStorageId,
                    ImportUserName = x.CreateUser.FirstName + " " + x.CreateUser.LastName
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ManagerApproveDone(int id)
        {
            var entity = Repository.GetById(id);
            using (UnitOfWork)
            {
                entity.ApproveFromManager = true;
                entity.ApproveFromManagerAt = DateTime.Now;
                entity.ApproveFromManagerId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("ManagerApprove", "ImportTrack", new { area = "Administrator" });
        }
        #endregion

        #region Bộ Phận Kho Xác Nhận Nhập Kho
        public ActionResult StorageStaffApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult StorageStaffApproveGridModel(string search)
        {
            var model = Repository.Search(search).Where(p => WorkContext.MyStorages.Contains(p.ToStorageId) && !p.ApproveFromStorageStaff && p.ApproveFromManager);

            var gridModel = new GridModel<ImportTrackModel>
            {
                Data = model.Select(x => new ImportTrackModel
                {
                    ImportTrackId = x.ImportTrackId,
                    CreateDate = x.CreateDate,
                    Note = x.Note,
                    StorageId = x.ToStorageId,
                    ImportUserName = x.CreateUser.FirstName + " " + x.CreateUser.LastName
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult StorageStaffApproveDone(int id)
        {
            var entity = Repository.GetAll().Where(p => p.ImportTrackId == id).Include(p => p.ImportDetails).FirstOrDefault();
            using (UnitOfWork)
            {
                entity.ApproveFromStorageStaff = true;
                entity.ApproveFromStorageStaffAt = DateTime.Now;
                entity.ApproveFromStorageStaffId = WorkContext.CurrentUserId;
                Repository.Update(entity);
                //Update Product In Storage
                using (UnitOfWork)
                {
                    foreach (var importDetail in entity.ImportDetails)
                    {
                        var productInStorage =
                            _productInStorageRepository.Search("")
                                .FirstOrDefault(
                                    p => p.ProductId == importDetail.ProductId && p.StorageId == entity.ToStorageId);
                        if (productInStorage == null)
                        {
                            var entityProductInStorage = new ProductInStorage
                            {
                                Amount = importDetail.Amount,
                                ProductId = importDetail.ProductId,
                                StorageId = entity.ToStorageId,
                                IsDeleted = false
                            };
                            _productInStorageRepository.Insert(entityProductInStorage);
                        }
                        else
                        {
                            productInStorage.Amount += importDetail.Amount;
                            _productInStorageRepository.Update(productInStorage);
                        }
                    }
                }
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("StorageStaffApprove", "ImportTrack", new { area = "Administrator" });
        }
        #endregion

    }
}
