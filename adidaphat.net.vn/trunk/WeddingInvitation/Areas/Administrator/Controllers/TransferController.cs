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
    public class TransferController : ControllerBase<ITransferRepository, Transfer>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductInStorageRepository _productInStorageRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IImportDetailRepository _importDetailRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransferItemRepository _transferItemRepository;
        private string TransferItemsSession = "TransferItemsSession";
        public List<TransferItemModel> TransferItems
        {
            get
            {
                if (Session[TransferItemsSession] == null)
                {
                    var transferItems = new List<TransferItemModel>();
                    Session[TransferItemsSession] = transferItems;
                    return transferItems;
                }
                return (List<TransferItemModel>)Session[TransferItemsSession];
            }
            set { Session[TransferItemsSession] = value; }
        }

        public TransferController(IUnitOfWork unitOfWork, ITransferRepository repository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository,
            IProductInStorageRepository productInStorageRepository, IStorageRepository storageRepository,
            IImportDetailRepository importDetailRepository, ICategoryRepository categoryRepository,
            IUserRepository userRepository, ITransferItemRepository transferItemRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _productInStorageRepository = productInStorageRepository;
            _storageRepository = storageRepository;
            _importDetailRepository = importDetailRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _transferItemRepository = transferItemRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            var toStorages = _storageRepository.Search("").ToList();
            var storages = toStorages.Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new TransferModel
            {
                Storages = storages,
                ToStorages = toStorages
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
            var fromListStorages = new int[1];
            var isGetAllFromStorages = true;
            //Filter by deliver to
            if (filterModel.FromListStorages != null && filterModel.FromListStorages.Count() > 0)
            {
                fromListStorages = filterModel.FromListStorages;
                isGetAllFromStorages = false;
            }
            var toListStorages = new int[1];
            var isGetAllToStorages = true;
            //Filter by deliver to
            if (filterModel.ToListStorages != null && filterModel.ToListStorages.Count() > 0)
            {
                toListStorages = filterModel.ToListStorages;
                isGetAllToStorages = false;
            }

            var model = from x in Repository.Search(filterModel.search).Where(p => (fromListStorages.Contains(p.FromStorageId) || (isGetAllFromStorages && WorkContext.MyStorages.Contains(p.FromStorageId))) &&
                (toListStorages.Contains(p.ToStorageId) || (isGetAllToStorages && WorkContext.MyStorages.Contains(p.ToStorageId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _userRepository.GetAll() on x.ApproveFromStorageStaffId equals s.UserId into s1
                        from s2 in s1.DefaultIfEmpty()
                        join t in _userRepository.GetAll() on x.ApproveFromGeneralDeliveryManId equals t.UserId into t1
                        from t2 in s1.DefaultIfEmpty()
                        join h in _userRepository.GetAll() on x.ApproveFromReceiveStorageStaffId equals h.UserId into h1
                        from h2 in s1.DefaultIfEmpty()
                        select new TransferModel
                        {
                            TransferId = x.TransferId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            ApproveFromStorageStaffName = (s2 == null ? String.Empty : s2.FirstName + " " + s2.LastName),
                            ApproveFromGeneralDeliveryMan = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                            StorageStaffApproveReceive = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                        };

            var gridModel = new GridModel<TransferModel>
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
                TransferItems = new List<TransferItemModel>();
            }
            var toStorages = _storageRepository.Search("").ToList();
            var storages = toStorages.Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new TransferModel { CreateDate = DateTime.Now, Mode = mode, Storages = storages,ToStorages=toStorages};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var toStorages = _storageRepository.Search("").ToList();
            var storages = toStorages.Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var entity = Repository.GetById(id);
            var model = new TransferModel
            {
                TransferId = entity.TransferId,
                Note = entity.Note,
                CreateDate = entity.CreateDate,
                Storages = storages,
                FromStorageId = entity.FromStorageId,
                ToStorageId = entity.ToStorageId,
                ToStorages =toStorages
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(TransferModel model)
        {
            if (model.TransferId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var toStorages = _storageRepository.Search("").ToList();
                    var storages = toStorages.Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
                    model.Storages = storages;
                    model.ToStorages = toStorages;
                    return View("Create", model);
                }
                if (!TransferItems.Any())
                {
                    SetErrorNotification(string.Format("Bạn chưa có sản phẩm nào."));
                    return RedirectToAction("Create", new { area = "Administrator" });
                }
                var entity = new Transfer
                {
                    IsDeleted = false,
                    CreateDate = model.CreateDate,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = model.Note,
                    ToStorageId = model.ToStorageId,
                    FromStorageId = model.FromStorageId,
                    TransferItems = new List<TransferItem>()
                };
                foreach (var transferItemModel in TransferItems)
                {
                    entity.TransferItems.Add(new TransferItem
                    {
                        Amount = transferItemModel.Amount,
                        ProductId = transferItemModel.ProductId,
                        Note = transferItemModel.Note,
                        IsDeleted = false
                    });
                }
                using (UnitOfWork)
                {
                    Repository.Insert(entity);
                }

            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Chuyển kho nội bộ"));
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
                this.SetSuccessNotification("Chuyển kho nội bộ đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Chuyển kho nội bộ này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        public ActionResult ListTransferItem(int id)
        {
            var model = new TransferModel
            {
                TransferId = id
            };
            return View("_ListTransferItem", model);
        }
        [GridAction]
        public ActionResult GridModelTransferItem(int? id)
        {
            if (id.HasValue)
            {
                var transferItems = _transferItemRepository.Search("").Where(p => p.TransferId == id.Value).Select(p => new TransferItemModel
                {
                    Amount = p.Amount,
                    ProductId = p.ProductId,
                    ProductName = p.Product.ProductName,
                    CategoryName = p.Product.Category.CategoryName
                }).ToList();
                var gridModel1 = new GridModel<TransferItemModel>
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
                Data = new GridModel<TransferItemModel>
                {
                    Data = TransferItems
                }
            };
        }

        #region Transfer Item

        public ActionResult CreateTransferItem(int id)
        {
            var categories = _categoryRepository.Search("").ToList();
            var model = new TransferItemModel { Categories = categories, TransferId = id };
            return View("_CreateTransferItem", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult SaveTransferItem(TransferItemModel model)
        {
            if (model.TransferItemId <= 0) //Create News
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
                    int transferItemId = rnd.Next(-9999, -1);
                    TransferItems.Add(new TransferItemModel
                    {
                        TransferId = model.TransferId,
                        Amount = model.Amount,
                        Note = model.Note,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        CategoryName = item.Category.CategoryName
                    });
                }
            }
            this.SetSuccessNotification(string.Format("{0} đã được thêm vào thành công.", "Sản phẩm"));
            return RedirectToAction("Create", "Transfer", new { area = "Administrator", mode = 1, blank = false });
        }

        public ActionResult DeleteTransferItem(int id)
        {
            try
            {
                TransferItems = TransferItems.Where(p => p.TransferItemId != id).ToList();
                this.SetSuccessNotification("Sản phẩm đã được bỏ khỏi chuyển kho nôi bộ.");
                return RedirectToAction("Create", "Transfer", new { area = "Administrator" });
            }
            catch
            {
                this.SetErrorNotification("Sản phẩm này không thể xóa!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
        #endregion

        #region Quản Lý Xác nhận chuyển kho nội bộ
        public ActionResult ManagerApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult ManagerApproveGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => WorkContext.MyStorages.Contains(p.FromStorageId) && !p.ApproveFromManager)
                        join m in _userRepository.GetAll() on x.CreateUserId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _storageRepository.GetAll() on x.FromStorageId equals s.StorageId into s1
                        from s2 in s1.DefaultIfEmpty()
                        join t in _storageRepository.GetAll() on x.ToStorageId equals t.StorageId into t1
                        from t2 in s1.DefaultIfEmpty()
                        select new TransferModel
                        {
                            TransferId =x.TransferId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            FromStorageId = x.FromStorageId,
                            CreateUserName = (m2 == null ? "" : m2.FirstName + " " + m2.LastName),
                            FromStorageName = (s2 == null ? "" : s2.StorageName),
                            ToStorageName = (t2 == null ? "" : t2.StorageName),
                        };

            var gridModel = new GridModel<TransferModel>
            {
                Data = model
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
            return RedirectToAction("ManagerApprove", "Transfer", new { area = "Administrator" });
        }
        #endregion

        #region Bộ phận kho xác nhận chuyển kho nội bộ
        public ActionResult StorageStaffApproveExport()
        {
            return View();
        }

        [GridAction]
        public ActionResult StorageStaffApproveExportGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => WorkContext.MyStorages.Contains(p.FromStorageId) && !p.ApproveFromStorageStaff && p.ApproveFromManager)
                        join m in _userRepository.GetAll() on x.CreateUserId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _storageRepository.GetAll() on x.FromStorageId equals s.StorageId into s1
                        from s2 in s1.DefaultIfEmpty()
                        join t in _storageRepository.GetAll() on x.ToStorageId equals t.StorageId into t1
                        from t2 in s1.DefaultIfEmpty()
                        select new TransferModel
                        {
                            TransferId = x.TransferId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            FromStorageId = x.FromStorageId,
                            CreateUserName = (m2 == null ? "" : m2.FirstName + " " + m2.LastName),
                            FromStorageName = (s2 == null ? "" : s2.StorageName),
                            ToStorageName = (t2 == null ? "" : t2.StorageName),
                        };

            var gridModel = new GridModel<TransferModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult StorageStaffApproveExportDone(int id)
        {
            var entity = Repository.GetAll().Where(p => p.TransferId == id).Include(p => p.TransferItems).FirstOrDefault();
            using (UnitOfWork)
            {
                entity.ApproveFromStorageStaff = true;
                entity.ApproveFromStorageStaffAt = DateTime.Now;
                entity.ApproveFromStorageStaffId = WorkContext.CurrentUserId;
                Repository.Update(entity);
                //Update Product In Storage
                using (UnitOfWork)
                {
                    foreach (var importDetail in entity.TransferItems)
                    {
                        var productInStorage =
                            _productInStorageRepository.Search("")
                                .FirstOrDefault(
                                    p => p.ProductId == importDetail.ProductId && p.StorageId == entity.FromStorageId);
                        if (productInStorage == null)
                        {
                            var entityProductInStorage = new ProductInStorage
                            {
                                Amount = 0,
                                ProductId = importDetail.ProductId,
                                StorageId = entity.FromStorageId,
                                IsDeleted = false
                            };
                            _productInStorageRepository.Insert(entityProductInStorage);
                        }
                        else
                        {
                            productInStorage.Amount -= importDetail.Amount;
                            _productInStorageRepository.Update(productInStorage);
                        }
                    }
                }
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("StorageStaffApproveExport", "Transfer", new { area = "Administrator" });
        }
        #endregion

        #region Bộ phận giao hàng: xác nhận chuyển hàng cho kho ở tỉnh
        public ActionResult ApproveFromGeneralDeliveryMan()
        {
            return View();
        }

        [GridAction]
        public ActionResult ApproveFromGeneralDeliveryManGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => WorkContext.MyStorages.Contains(p.FromStorageId) && !p.ApproveFromGeneralDeliveryMan && p.ApproveFromStorageStaff)
                        join m in _userRepository.GetAll() on x.CreateUserId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _storageRepository.GetAll() on x.FromStorageId equals s.StorageId into s1
                        from s2 in s1.DefaultIfEmpty()
                        join t in _storageRepository.GetAll() on x.ToStorageId equals t.StorageId into t1
                        from t2 in s1.DefaultIfEmpty()
                        select new TransferModel
                        {
                            TransferId = x.TransferId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            FromStorageId = x.FromStorageId,
                            CreateUserName = (m2 == null ? "" : m2.FirstName + " " + m2.LastName),
                            FromStorageName = (s2 == null ? "" : s2.StorageName),
                            ToStorageName = (t2 == null ? "" : t2.StorageName),
                        };

            var gridModel = new GridModel<TransferModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ApproveFromGeneralDeliveryManDone(int id)
        {
            var entity = Repository.GetAll().Where(p => p.TransferId == id).Include(p => p.TransferItems).FirstOrDefault();
            using (UnitOfWork)
            {
                entity.ApproveFromGeneralDeliveryMan = true;
                entity.ApproveFromGeneralDeliveryManAt = DateTime.Now;
                entity.ApproveFromGeneralDeliveryManId = WorkContext.CurrentUserId;
                using (UnitOfWork)
                {
                    Repository.Update(entity);
                }
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("ApproveFromGeneralDeliveryMan", "Transfer", new { area = "Administrator" });
        }
        #endregion

        #region Chuyển kho nội bộ: xác nhận nhập hàng từ kho khác
        public ActionResult StorageStaffApproveReceive()
        {
            return View();
        }

        [GridAction]
        public ActionResult StorageStaffApproveReceiveGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => WorkContext.MyStorages.Contains(p.FromStorageId) && !p.ApproveFromReceiveStorageStaff && p.ApproveFromManager)
                        join m in _userRepository.GetAll() on x.CreateUserId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _storageRepository.GetAll() on x.FromStorageId equals s.StorageId into s1
                        from s2 in s1.DefaultIfEmpty()
                        join t in _storageRepository.GetAll() on x.ToStorageId equals t.StorageId into t1
                        from t2 in s1.DefaultIfEmpty()
                        select new TransferModel
                        {
                            TransferId = x.TransferId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            FromStorageId = x.FromStorageId,
                            CreateUserName = (m2 == null ? "" : m2.FirstName + " " + m2.LastName),
                            FromStorageName = (s2 == null ? "" : s2.StorageName),
                            ToStorageName = (t2 == null ? "" : t2.StorageName),
                        };

            var gridModel = new GridModel<TransferModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult StorageStaffApproveReceiveDone(int id)
        {
            var entity = Repository.GetAll().Where(p => p.TransferId == id).Include(p => p.TransferItems).FirstOrDefault();
            using (UnitOfWork)
            {
                entity.ApproveFromReceiveStorageStaff = true;
                entity.ApproveFromReceiveStorageStaffAt = DateTime.Now;
                entity.ApproveFromReceiveStorageStaffId = WorkContext.CurrentUserId;
                Repository.Update(entity);
                //Update Product In Storage
                using (UnitOfWork)
                {
                    foreach (var importDetail in entity.TransferItems)
                    {
                        var productInStorage =
                            _productInStorageRepository.Search("")
                                .FirstOrDefault(
                                    p => p.ProductId == importDetail.ProductId && p.StorageId == entity.FromStorageId);
                        if (productInStorage == null)
                        {
                            var entityProductInStorage = new ProductInStorage
                            {
                                Amount = importDetail.Amount,
                                ProductId = importDetail.ProductId,
                                StorageId = entity.FromStorageId,
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
            return RedirectToAction("StorageStaffApproveReceive", "Transfer", new { area = "Administrator" });
        }
        #endregion

        #region Báo cáo nhập hàng từ kho khác
        public ActionResult ImportFromOtherStorage()
        {
            var toStorages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new TransferModel
            {
                ToStorages = toStorages
            };
            return View(model);
        }

        [GridAction]
        public ActionResult ImportFromOtherStorageModel(ReportFilterModel filterModel)
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

            var model = from x in Repository.Search(filterModel.search).Where(p =>
                (toListStorages.Contains(p.ToStorageId) || (isGetAllToStorages && WorkContext.MyStorages.Contains(p.ToStorageId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _userRepository.GetAll() on x.ApproveFromStorageStaffId equals s.UserId into s1
                        from s2 in s1.DefaultIfEmpty()
                        join t in _userRepository.GetAll() on x.ApproveFromGeneralDeliveryManId equals t.UserId into t1
                        from t2 in s1.DefaultIfEmpty()
                        join h in _userRepository.GetAll() on x.ApproveFromReceiveStorageStaffId equals h.UserId into h1
                        from h2 in s1.DefaultIfEmpty()
                        join y in _storageRepository.GetAll() on x.FromStorageId equals y.StorageId into y1
                        from y2 in y1.DefaultIfEmpty()
                        join w in _storageRepository.GetAll() on x.ToStorageId equals w.StorageId into w1
                        from w2 in y1.DefaultIfEmpty()
                        select new TransferModel
                        {
                            TransferId = x.TransferId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            ApproveFromStorageStaffName = (s2 == null ? String.Empty : s2.FirstName + " " + s2.LastName),
                            ApproveFromGeneralDeliveryMan = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                            StorageStaffApproveReceive = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                            FromStorageName = (y2 == null ? "" : y2.StorageName),
                            ToStorageName = (w2 == null ? "" : w2.StorageName),
                        };

            var gridModel = new GridModel<TransferModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion
    }
}
