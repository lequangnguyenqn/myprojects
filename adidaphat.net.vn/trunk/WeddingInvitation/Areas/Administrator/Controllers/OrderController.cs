using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Orders;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using System.Data.Entity;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Services.Users;
using WeddingInvitation.Data;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class OrderController : ControllerBase<IOrderRepository, Order>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IShippingServiceRepository _shippingServiceRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductInStorageRepository _productInStorageRepository;
        private readonly IRateMappingRepository _rateMappingRepository;
        private readonly IExtraFeeRepository _extraFeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExportService _exportService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IShippingFeeRepository _shippingFeeRepository;
        private readonly IExportTrackRepository _exportTrackRepository;
        private readonly IOrderDeliveryPackageRepository _orderDeliveryPackageRepository;
        private string OrderDetailsSession = "OrderDetailsSession";
        public List<OrderDetailModel> OrderDetails
        {
            get
            {
                if (Session[OrderDetailsSession] == null)
                {
                    var orderDetails = new List<OrderDetailModel>();
                    Session[OrderDetailsSession] = orderDetails;
                    return orderDetails;
                }
                return (List<OrderDetailModel>)Session[OrderDetailsSession];
            }
            set { Session[OrderDetailsSession] = value; }
        }
        private string OrderSessionString = "OrderSessionString";
        public OrderModel OrderSession
        {
            get
            {
                if (Session[OrderSessionString] == null)
                {
                    var orderModel = new OrderModel();
                    Session[OrderSessionString] = orderModel;
                    return orderModel;
                }
                return (OrderModel)Session[OrderSessionString];
            }
            set { Session[OrderSessionString] = value; }
        }
        private string OrderDetailSessionString = "OrderDetailSessionString";
        public OrderDetailModel OrderDetailSession
        {
            get
            {
                if (Session[OrderDetailSessionString] == null)
                {
                    var orderModel = new OrderDetailModel();
                    Session[OrderDetailSessionString] = orderModel;
                    return orderModel;
                }
                return (OrderDetailModel)Session[OrderDetailSessionString];
            }
            set { Session[OrderDetailSessionString] = value; }
        }

        public bool IsFirstTimeCallEdit
        {
            get
            {
                if (Session["IsFirstTimeCallEdit"] == null)
                    return false;
                return (bool)Session["IsFirstTimeCallEdit"];
            }
            set { Session["IsFirstTimeCallEdit"] = value; }
        }

        public OrderController(IUnitOfWork unitOfWork, IOrderRepository repository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository,
            ICustomerRepository customerRepository, IShippingServiceRepository shippingServiceRepository,
            IStorageRepository storageRepository,IOrderDetailRepository orderDetailRepository,
            IProductInStorageRepository productInStorageRepository, IRateMappingRepository rateMappingRepository,
            IExtraFeeRepository extraFeeRepository, IUserRepository userRepository,
            IExportService exportService, ICategoryRepository categoryRepository,
            IShippingFeeRepository shippingFeeRepository, IExportTrackRepository exportTrackRepository,
            IOrderDeliveryPackageRepository orderDeliveryPackageRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _shippingServiceRepository = shippingServiceRepository;
            _storageRepository = storageRepository;
            _orderDetailRepository = orderDetailRepository;
            _productInStorageRepository = productInStorageRepository;
            _rateMappingRepository = rateMappingRepository;
            _extraFeeRepository = extraFeeRepository;
            _userRepository = userRepository;
            _exportService = exportService;
            _categoryRepository = categoryRepository;
            _shippingFeeRepository = shippingFeeRepository;
            _exportTrackRepository = exportTrackRepository;
            _orderDeliveryPackageRepository = orderDeliveryPackageRepository;
        }

        #region Order

        public ActionResult Index()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices
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
            var listOffices = new int[1];
            var isGetAllOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllOffices = false;
            }
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
            var model = from x in Repository.Search(filterModel.search).Where(p =>
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.OrderDetails)
                        join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join t in _userRepository.GetAll() on x.PrintByUserId equals t.UserId into t1
                        from t2 in t1.DefaultIfEmpty()
                        join k in _userRepository.GetAll() on x.DeliveredUserId equals k.UserId into k1
                        from k2 in k1.DefaultIfEmpty()
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            PrintUserName = (t2 == null ? "" : t2.FirstName + " " + t2.LastName),
                            DeliveredUserName = (k2 == null ? "" : k2.FirstName + " " + k2.LastName),
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create(bool? blank)
        {
            if (!blank.HasValue || blank.Value)
            {
                OrderDetails = new List<OrderDetailModel>();
                OrderSession = new OrderModel();
            }
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var shippingServices = _shippingServiceRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel 
            { 
                MyOffices = offices, 
                Customers = customers, 
                ShippingServices = shippingServices, 
                PaymentType = OrderSession.PaymentType >0 ? OrderSession.PaymentType : (int)PaymentTypes.InShippingPlace,
                CustomerId = OrderSession.CustomerId,
                CustomerName = OrderSession.CustomerName,
                MyOfficeId = OrderSession.MyOfficeId,
                Note =OrderSession.Note,
                ShippingServiceId = OrderSession.ShippingServiceId,
                ShowOnTop = OrderSession.ShowOnTop,
                WaitForPrint =OrderSession.WaitForPrint,
                HaveShippingFee = OrderSession.HaveShippingFee
            };
            return View(model);
        }

        public virtual ActionResult Edit(int id, int? backId, bool? blank)
        {
            IsFirstTimeCallEdit = true;
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var shippingServices = _shippingServiceRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var entity = Repository.GetAll().Where(p => p.OrderId == id).Include(p => p.OrderDetails).FirstOrDefault();
            var model = new OrderModel()
            {
                OrderId = entity.OrderId,
                Note = entity.Note,
                MyOfficeId = entity.MyOfficeId,
                MyOffices = offices,
                Customers = customers,
                ShippingServices = shippingServices,
                CustomerId = entity.CustomerId,
                Paid = entity.Paid,
                PaymentType = entity.PaymentType,
                ShippingServiceId = entity.ShippingServiceId,
                ShippingServiceName = entity.Customer.CustomerShortName,
                Status = entity.Status,
                TotalCost = entity.TotalCost,
                BackId = backId,
                ShowOnTop = entity.ShowOnTop,
                CreateDate = entity.CreateDate,
                ExtraFee = entity.ExtraFee
            };
            if (!blank.HasValue || blank.Value)
            {
                OrderDetails = new List<OrderDetailModel>();
                OrderSession = new OrderModel();
                OrderDetails = entity.OrderDetails.Select(p => new OrderDetailModel
                {
                    OrderId = p.OrderId,
                    Amount = p.Amount,
                    FilePath = p.FilePath,
                    Note = p.Note,
                    OrderDetailId = p.OrderDetailId,
                    ProductId = p.ProductId,
                    CategoryId = p.Product.CategoryId,
                    CategoryName = p.Product.Category.CategoryName,
                    ProductName = p.Product.ProductName,
                    StorageId = p.StorageId,
                    TotalCost = p.TotalCost,
                    PrintIncludeImage = p.PrintIncludeImage,
                    PrintWithoutImage = p.PrintWithoutImage,
                    Printable = p.Product.Printable,
                    FilePathBia = p.FilePathBia,
                    FilePathRuot = p.FilePathRuot,
                    GroupId = p.UpdateUserId.Value
                }).ToList();
            }
            FillCostForOrderDetail(OrderDetails);
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(OrderModel model)
        {
            if (model.OrderId <= 0) //Create News
            {
                SaveOrderToSession(model);
                if (!OrderDetails.Any())
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa có Sản phẩm."));
                    return RedirectToAction("Create", "Order", new { area = "Administrator" });
                }
                if (model.PaymentType != (int)PaymentTypes.InOffice && !model.ShippingServiceId.HasValue)
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa chọn chành xe."));
                    //return RedirectToAction("Create", "Order", new { area = "Administrator" });
                    var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var shippingServices = _shippingServiceRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    model.MyOffices = offices;
                    model.Customers = customers;
                    model.ShippingServices = shippingServices;
                    return View("Create", model);
                }
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var shippingServices = _shippingServiceRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    model.MyOffices = offices;
                    model.Customers = customers;
                    model.ShippingServices = shippingServices;
                    return View("Create", model);
                }
                var office = _myOfficeRepository.GetById(model.MyOfficeId);
                if (!OrderDetails.Any(p => p.PrintIncludeImage || p.PrintWithoutImage) && office.IsRetailCustomer)
                {
                    SetErrorNotification(string.Format("Không bán phôi cho khách lẻ."));
                    return RedirectToAction("Create", "Order", new { area = "Administrator", blank = false });
                }
                var entity = new Order
                {
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = model.Note,
                    MyOfficeId = model.MyOfficeId,
                    TotalCost = model.TotalCost,
                    ExtraFee = model.ExtraFee,
                    Status = (int)OrderStatus.Imported,
                    PaymentType = model.PaymentType,
                    ShippingServiceId = model.ShippingServiceId,
                    WaitForPrint = model.WaitForPrint,
                    ShowOnTop= model.ShowOnTop,
                    OrderDetails = new List<OrderDetail>()
                };
                SetCustomerToOrder(model, entity);
                AddOrderDetailToOrder(entity, OrderDetails);
                
                //Update product in storage
                foreach (var orderDetailModel in OrderDetails)
                {
                    var productInStorage = _productInStorageRepository.Search("").FirstOrDefault(p => p.ProductId == orderDetailModel.ProductId && p.StorageId == orderDetailModel.StorageId);
                    if (productInStorage != null && productInStorage.Amount > orderDetailModel.Amount)
                    {
                        productInStorage.Amount -= orderDetailModel.Amount;
                    }
                    else
                    {
                        var product = _productRepository.GetById(orderDetailModel.ProductId);
                        var storage = _storageRepository.GetById(orderDetailModel.StorageId);
                        SetErrorNotification(string.Format("Sản phẩm {0} đã hết trong kho {1}.", product.ProductName, storage.StorageName));
                        return RedirectToAction("Create", "Order", new { area = "Administrator", blank = false });
                    }
                }
                using (UnitOfWork)
                {
                    Repository.Insert(entity);
                }

                OrderDetails = new List<OrderDetailModel>();
                OrderSession = new OrderModel();
                this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Đơn hàng"));
                return RedirectToAction("Create", "Order", new { area = "Administrator", blank = false });
            }
            else //Edit user
            {
                if (!OrderDetails.Any())
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa có Sản phẩm."));
                    return RedirectToAction("Edit", "Order", new { area = "Administrator", id= model.OrderId });
                }
                if (model.PaymentType != (int)PaymentTypes.InOffice && !model.ShippingServiceId.HasValue && string.IsNullOrEmpty(model.ShippingServiceName))
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa chọn chành xe."));
                    return RedirectToAction("Edit", "Order", new { area = "Administrator", id = model.OrderId });
                }
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").ToList();
                    var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var shippingServices = _shippingServiceRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    model.MyOffices = offices;
                    model.Customers = customers;
                    model.ShippingServices = shippingServices;
                    return View("Edit", model);
                }

                var entity = Repository.Search("").Where(p=> p.OrderId == model.OrderId).Include(p => p.OrderDetails).FirstOrDefault();
                entity.Note = model.Note;
                entity.MyOfficeId = model.MyOfficeId;
                entity.TotalCost = model.TotalCost;
                entity.ExtraFee = model.ExtraFee;
                entity.PaymentType = model.PaymentType;
                entity.ShippingServiceId = model.ShippingServiceId;
                entity.WaitForPrint = model.WaitForPrint;
                entity.ShowOnTop = model.ShowOnTop;
                entity.CreateDate = model.CreateDate;
                SetCustomerToOrder(model, entity);

                var orderDetailIds = entity.OrderDetails.Select(p => p.OrderDetailId).ToList();
                //Remove order details
                var listBeRemoved = entity.OrderDetails.Where(p => !OrderDetails.Select(o => o.OrderDetailId).Contains(p.OrderDetailId)).ToList();
                entity.OrderDetails = entity.OrderDetails.Where(p => !listBeRemoved.Select(o => o.OrderDetailId).Contains(p.OrderDetailId)).ToList();
                //Add order details
                var listBeAdded = OrderDetails.Where(p => !orderDetailIds.Contains(p.OrderDetailId)).ToList();
                AddOrderDetailToOrder(entity, listBeAdded);
                //Update image path
                var listUpdate = entity.OrderDetails.Where(p => OrderDetails.Select(o => o.OrderDetailId).Contains(p.OrderDetailId)).ToList();
                foreach (var orderDetail in listUpdate)
                {
                    var orderDetailModel = OrderDetails.FirstOrDefault(p => p.OrderDetailId == orderDetail.OrderDetailId);
                    orderDetail.FilePath = orderDetailModel.FilePath;
                    orderDetail.Note = orderDetailModel.Note;
                }

                //Update product in storage
                foreach (var orderDetailModel in listBeRemoved)
                {
                    var productInStorage = _productInStorageRepository.Search("").FirstOrDefault(p => p.ProductId == orderDetailModel.ProductId && p.StorageId == orderDetailModel.StorageId);
                    if (productInStorage != null)
                    {
                        productInStorage.Amount += orderDetailModel.Amount;
                    }
                    else
                    {
                        var product = _productRepository.GetById(orderDetailModel.ProductId);
                        var storage = _storageRepository.GetById(orderDetailModel.StorageId);
                        SetErrorNotification(string.Format("Sản phẩm {0} chưa được nhập kho {1}.", product.ProductName, storage.StorageName));
                        return RedirectToAction("Edit", "Order", new { area = "Administrator", id = model.OrderId });
                    }
                }
                foreach (var orderDetailModel in listBeAdded)
                {
                    var productInStorage = _productInStorageRepository.Search("").FirstOrDefault(p => p.ProductId == orderDetailModel.ProductId && p.StorageId == orderDetailModel.StorageId);
                    if (productInStorage != null && productInStorage.Amount > orderDetailModel.Amount)
                    {
                        productInStorage.Amount -= orderDetailModel.Amount;
                    }
                    else
                    {
                        var product = _productRepository.GetById(orderDetailModel.ProductId);
                        var storage = _storageRepository.GetById(orderDetailModel.StorageId);
                        SetErrorNotification(string.Format("Sản phẩm {0} đã hết trong kho {1}.", product.ProductName, storage.StorageName));
                        return RedirectToAction("Edit", "Order", new { area = "Administrator", id = model.OrderId });
                    }
                }
                
               
                using (UnitOfWork)
                {
                    Repository.Update(entity);
                }
            }
            OrderDetails = new List<OrderDetailModel>();
            OrderSession = new OrderModel();
            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Đơn hàng"));
            if (model.BackId == (int)BackToPage.UploadFile)
            {
                return RedirectToAction("UploadFile", "Order", new { area = "Administrator" });
            }
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult AjaxSaveOrderToSession(OrderModel model)
        {
            SaveOrderToSession(model);
            return Json(true);
        }

        private void SaveOrderToSession(OrderModel model)
        {
            OrderSession.CustomerId = model.CustomerId;
            OrderSession.CustomerName = model.CustomerName;
            OrderSession.MyOfficeId = model.MyOfficeId;
            OrderSession.Note = model.Note;
            OrderSession.PaymentType = model.PaymentType;
            OrderSession.ShippingServiceId = model.ShippingServiceId;
            OrderSession.ShowOnTop = model.ShowOnTop;
            OrderSession.WaitForPrint = model.WaitForPrint;
            OrderSession.HaveShippingFee = model.HaveShippingFee;
        }

        private void AddOrderDetailToOrder(Order entity, List<OrderDetailModel> listBeAdded)
        {
            foreach (var orderDetailModel in listBeAdded)
            {
                entity.OrderDetails.Add(new OrderDetail
                {
                    Amount = orderDetailModel.Amount,
                    CreateDate = DateTime.Now,
                    StorageId = orderDetailModel.StorageId,
                    ProductId = orderDetailModel.ProductId,
                    TotalCost = orderDetailModel.TotalCost,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = orderDetailModel.Note,
                    FilePath = orderDetailModel.FilePath,
                    IsDeleted = false,
                    UpdateUserId = orderDetailModel.GroupId,
                    PrintIncludeImage = orderDetailModel.PrintIncludeImage,
                    PrintWithoutImage = orderDetailModel.PrintWithoutImage
                });
            }
        }

        private void SetCustomerToOrder(OrderModel model, Order entity)
        {
            if (model.CustomerId <= 0)
            {
                entity.Customer = new Customer
                {
                    CustomerName = string.IsNullOrEmpty(model.CustomerName) ? "Customer " + DateTime.Now.ToString("G") : model.CustomerName,
                    IsDeleted = true,
                    MyOfficeId = model.MyOfficeId,
                    CustomerShortName = model.ShippingServiceName
                };
            }
            else
            {
                entity.CustomerId = model.CustomerId;
                var customer = _customerRepository.GetById(model.CustomerId);
                if (customer.CustomerShortName != model.ShippingServiceName || customer.MyOfficeId != model.MyOfficeId)
                {
                    using (UnitOfWork)
                    {
                        customer.CustomerShortName = model.ShippingServiceName;
                        customer.MyOfficeId = model.MyOfficeId;
                        _customerRepository.Update(customer);
                    }
                }
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    entity.IsDeleted = true;

                    //Update product in storage
                    foreach (var orderDetailModel in entity.OrderDetails)
                    {
                        var productInStorage = _productInStorageRepository.Search("").FirstOrDefault(p => p.ProductId == orderDetailModel.ProductId && p.StorageId == orderDetailModel.StorageId);
                        if (productInStorage != null)
                        {
                            productInStorage.Amount += orderDetailModel.Amount;
                        }
                        else
                        {
                            var product = _productRepository.GetById(orderDetailModel.ProductId);
                            var storage = _storageRepository.GetById(orderDetailModel.StorageId);
                            SetErrorNotification(string.Format("Sản phẩm {0} chưa được nhập kho {1}.", product.ProductName, storage.StorageName));
                            return RedirectToAction("index", new { area = "Administrator" });
                        }
                    }
                }
                this.SetSuccessNotification("Đơn hàng đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Đơn hàng này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        public JsonResult GetCustomers(int? MyOfficeId)
        {
            var customers = _customerRepository.Search("").Where(p => MyOfficeId == p.MyOfficeId).ToList();
            return Json(new SelectList(customers, "CustomerId", "CustomerName"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShippingServices(int? MyOfficeId)
        {
            bool isRetailCustomer = false;
            var myOffice = _myOfficeRepository.GetById(MyOfficeId);
            if (myOffice != null)
            {
                isRetailCustomer = myOffice.IsRetailCustomer;
            }
            var customers = _shippingServiceRepository.Search("").Where(p => MyOfficeId == p.MyOfficeId).ToList();
            return Json(new { IsRetailCustomer = isRetailCustomer, ShippingServiceData = new SelectList(customers, "ShippingServiceId", "ShippingServiceName") }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCost(int? myOfficeId)
        {
            OrderSession.MyOfficeId = myOfficeId.HasValue ? myOfficeId.Value : 0;
            if (!OrderDetails.Any())
            {
                return Json(new { TotalCost = 0, ExtraFee = 0 });
            }
            
            decimal totalCost = OrderDetails.Sum(p => p.TotalCost);
            decimal extraFeeCost = OrderDetails.Sum(p => p.ExtraFee);
            if (IsFirstTimeCallEdit)
            {
                IsFirstTimeCallEdit = false;
                return Json(new { TotalCost = Repository.GetById(OrderDetails.FirstOrDefault().OrderId).TotalCost, ExtraFee = extraFeeCost });
            }
            return Json(new { TotalCost = totalCost, ExtraFee = extraFeeCost });
        }

        public ActionResult OrderStopping(int id, int? mode)
        {
            var entity = Repository.GetById(id);
            if (entity.Status < (int) OrderStatus.Printed)
            {
                using (UnitOfWork)
                {
                    entity.Stopping = !entity.Stopping;
                    Repository.Update(entity);
                }
                this.SetSuccessNotification("Đã chuyển trạng thái thành công cho đơn hàng!");
            }
            else
            {
                SetErrorNotification("Đơn hàng này đã in, không thể chuyển trạng thái!");
            }
            if (mode.HasValue)
            {
                return RedirectToAction("Printing", "Order", new { area = "Administrator" });
            }
            return RedirectToAction("Index", "Order", new { area = "Administrator" });
        }

        public ActionResult CanelOrderDone(int id)
        {
            var entity = Repository.GetAll().Where(p => p.OrderId == id).Include(p => p.OrderDetails).FirstOrDefault();
            if (entity.Status < (int)OrderStatus.Printed)
            {
                using (UnitOfWork)
                {
                    entity.Cancel = true;
                    entity.ApproveCancelFrombusinessMan = true;
                    entity.ApproveCancelFrombusinessManAt = DateTime.Now;
                    entity.ApproveCancelFrombusinessManId = WorkContext.CurrentUserId;
                    //Update Order
                    using (UnitOfWork)
                    {
                        Repository.Update(entity);
                    }
                }
                this.SetSuccessNotification("Đã hủy đơn hàng thành công!");
            }
            else
            {
                SetErrorNotification("Đơn hàng này đã in, không thể hủy!");
            }
            return RedirectToAction("Index", "Order", new { area = "Administrator" });
        }

        #endregion
        
        #region Chi tiết đơn hàng
        public ActionResult ListOrderDetail(int id)
        {
            var orderModel = new OrderModel
            {
                OrderId = id
            };
            return View(orderModel);
        }

        [GridAction]
        public ActionResult GridModelOrderDetail(int? orderId)
        {
            if (orderId.HasValue)
            {
                var orderDetails = _orderDetailRepository.Search("").Where(p => p.OrderId == orderId.Value).Select(p => new OrderDetailModel
                    {
                        Amount = p.Amount,
                        FilePath = p.FilePath,
                        Note = p.Note,
                        OrderId = p.OrderId,
                        ProductId = p.ProductId,
                        CategoryId = p.Product.CategoryId,
                        ProductName = p.Product.ProductName,
                        CategoryName = p.Product.Category.CategoryName,
                        StorageId = p.StorageId,
                        TotalCost = p.TotalCost,
                        OrderDetailId = p.OrderDetailId,
                        PrintIncludeImage = p.PrintIncludeImage,
                        PrintWithoutImage = p.PrintWithoutImage,
                        Printable = p.Product.Printable,
                        GroupId = p.UpdateUserId.Value
                    }).ToList();
                foreach (var item in orderDetails)
                {
                    if (item.Printable && (item.PrintIncludeImage || item.PrintWithoutImage))
                    {
                        item.DownloadActionLink = Url.Action("DownloadOrderDetail", "Order", new { area = "Administrator" });
                        item.DownloadImagePath = Url.Content("~/Content/Images/download_16_16.png");
                        item.UploadImagePath = Url.Content("~/Content/Images/upload_16_16.png");
                        item.DownloadBiaActionLink = Url.Action("DownloadFileBia", "Home", new { area = "" });
                        item.DownloadRuotActionLink = Url.Action("DownloadFileRuot", "Home", new { area = "" });
                    }
                } 
                FillCostForOrderDetail(orderDetails);
                var model = new List<OrderDetailModel>();
                foreach (var item in orderDetails)
                {
                    if (!model.Any(p => p.CategoryId == item.CategoryId))
                    {
                        var orderDetails1 = orderDetails.Where(p => p.CategoryId == item.CategoryId).ToList();
                        foreach (var item1 in orderDetails1)
                        {
                            if (!model.Any(p => p.GroupId == item1.GroupId))
                            {
                                var orderDetails2 = orderDetails1.Where(p => p.GroupId == item1.GroupId).ToList();
                                //trường hợp bộ hoặc phôi
                                if (orderDetails2.Count() == 2)
                                {
                                    var orderDetailGroup = orderDetails2.Where(p => p.Printable).FirstOrDefault();
                                    if (orderDetailGroup.PrintIncludeImage || orderDetailGroup.PrintWithoutImage)
                                    {
                                        orderDetailGroup.ProductName = orderDetailGroup.CategoryName + " Bộ";
                                    }
                                    else
                                    {
                                        orderDetailGroup.ProductName = orderDetailGroup.CategoryName + " Phôi";
                                    }
                                    orderDetailGroup.PriceByCategory = orderDetails2[0].Price + orderDetails2[1].Price;
                                    orderDetailGroup.TotalCostByCategory = orderDetails2[0].TotalCost + orderDetails2[1].TotalCost;
                                    model.Add(orderDetailGroup);
                                }
                                else
                                {
                                    model.Add(item1);
                                }
                            }
                        }
                    }
                }
                var gridModel1 = new GridModel<OrderDetailModel>
                {
                    Data = model
                };
                return new JsonResult
                {
                    Data = gridModel1,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            foreach (var item in OrderDetails)
            {
                if (item.Printable && (item.PrintIncludeImage || item.PrintWithoutImage))
                {
                    item.DownloadActionLink = Url.Action("DownloadOrderDetail", "Order", new { area = "Administrator" });
                    item.DownloadImagePath = Url.Content("~/Content/Images/download_16_16.png");
                    item.UploadImagePath = Url.Content("~/Content/Images/upload_16_16.png");
                    item.DownloadBiaActionLink = Url.Action("DownloadFileBia", "Home", new { area = "" });
                    item.DownloadRuotActionLink = Url.Action("DownloadFileRuot", "Home", new { area = "" });
                }
            }
            var model1 = new List<OrderDetailModel>();
            foreach (var item in OrderDetails)
            {
                if (!model1.Any(p => p.CategoryId == item.CategoryId))
                {
                    var orderDetails = OrderDetails.Where(p => p.CategoryId == item.CategoryId).ToList();
                    foreach (var item1 in orderDetails)
                    {
                        if (!model1.Any(p => p.GroupId == item1.GroupId))
                        {
                            var orderDetails1 = orderDetails.Where(p => p.GroupId == item1.GroupId).ToList();
                            //trường hợp bộ hoặc phôi
                            if (orderDetails1.Count() == 2)
                            {
                                var orderDetailGroup = orderDetails1.Where(p => p.Printable).FirstOrDefault();
                                if (orderDetailGroup.PrintIncludeImage || orderDetailGroup.PrintWithoutImage)
                                {
                                    orderDetailGroup.ProductName = orderDetailGroup.CategoryName + " Bộ";
                                }
                                else
                                {
                                    orderDetailGroup.ProductName = orderDetailGroup.CategoryName + " Phôi";
                                }
                                orderDetailGroup.PriceByCategory = orderDetails1[0].Price + orderDetails1[1].Price;
                                orderDetailGroup.TotalCostByCategory = orderDetails1[0].TotalCost + orderDetails1[1].TotalCost;
                                model1.Add(orderDetailGroup);
                            }
                            else
                            {
                                model1.Add(item1);
                            }
                        }
                    }
                }
            }
            var gridModel = new GridModel<OrderDetailModel>
            {
                Data = model1
            };
            return new JsonResult
            {
                Data = gridModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult CreateOrderDetail(int orderId)
        {
            var categories = _categoryRepository.Search("").ToList();
            var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new OrderDetailModel { 
                Categories = categories, 
                Storages = storages, 
                OrderId = orderId,
                CategoryId = OrderDetailSession.CategoryId,
                Amount = OrderDetailSession.Amount,
                PrintIncludeImage = OrderDetailSession.PrintIncludeImage,
                PrintWithoutImage = OrderDetailSession.PrintWithoutImage,
                FilePathBia = OrderDetailSession.FilePathBia,
                FilePathRuot = OrderDetailSession.FilePathRuot,
                Note = OrderDetailSession.Note
            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult SaveOrderDetail(OrderDetailModel model)
        {
            if (model.OrderDetailId <= 0) //Create News
            {
                SaveOrderDetailToSession(model);
                if (!ModelState.IsValid)
                {
                    var categories = _categoryRepository.Search("").ToList();
                    var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
                    model.Categories = categories;
                    model.Storages = storages;
                    return View("CreateOrderDetail", model);
                }
                var amount = model.Amount;
                //Cộng thêm 5 thiệp cho số lượng > 500
                //Cộng thêm 3 thiệp cho số lượng <= 500
                if (model.Amount > 500)
                {
                    model.Amount += 5;
                }
                else
                {
                    model.Amount += 3;
                }
                Random rnd = new Random();
                var products = _productRepository.Search("").Where(p => model.ProductIds.Contains(p.ProductId)).ToList();
                var extraFee = _extraFeeRepository.GetAll().FirstOrDefault(p => p.AmountFrom < amount && amount < p.AmountTo && p.IsDeleted == false);
                foreach (var item in products)
                {
                    int orderDetailId = rnd.Next(-9999, -1);
                    OrderDetails.Add(new OrderDetailModel
                    {
                        Amount = model.Amount,
                        FilePath = model.FilePath,
                        Note = model.Note,
                        OrderId = model.OrderId,
                        ProductId = item.ProductId,
                        CategoryName = item.Category.CategoryName,
                        ProductName = item.ProductName,
                        StorageId = model.StorageId,
                        OrderDetailId = orderDetailId,
                        PrintIncludeImage = model.PrintIncludeImage,
                        PrintWithoutImage = model.PrintWithoutImage,
                        Printable = item.Printable,
                        CategoryId = item.CategoryId
                    });
                }
                FillCostForOrderDetail(OrderDetails);
            }
            OrderDetailSession = new OrderDetailModel();
            if (model.OrderId > 0)
            {
                //Save success
                this.SetSuccessNotification(string.Format("{0} đã được thêm vào đơn hàng thành công.", "Sản phẩm"));
                return RedirectToAction("Edit", "Order", new { area = "Administrator", id = model.OrderId });
            }
            else
            {
                this.SetSuccessNotification(string.Format("{0} đã được thêm vào đơn hàng thành công.", "Sản phẩm"));
                return RedirectToAction("Create", "Order", new { area = "Administrator", blank = false });
            }
            
        }

        private void SaveOrderDetailToSession(OrderDetailModel model)
        {
            OrderDetailSession.CategoryId = model.CategoryId;
            OrderDetailSession.Amount = model.Amount;
            OrderDetailSession.PrintIncludeImage = model.PrintIncludeImage;
            OrderDetailSession.PrintWithoutImage = model.PrintWithoutImage;
            OrderDetailSession.FilePathBia = model.FilePathBia;
            OrderDetailSession.FilePathRuot = model.FilePathRuot;
            OrderDetailSession.Note = model.Note;
        }

        public ActionResult DeleteOrderDetail(int id)
        {
            try
            {
                var orderDetail = OrderDetails.FirstOrDefault();
                OrderDetails = OrderDetails.Where(p => p.OrderDetailId != id).ToList();
                this.SetSuccessNotification("Sản phẩm đã được bỏ khỏi đơn hàng.");
                if (orderDetail.OrderId > 0)
                {
                    return RedirectToAction("Edit", "Order", new { area = "Administrator", id = orderDetail.OrderId });
                }
                else
                {
                    return RedirectToAction("Create", "Order", new { area = "Administrator", blank = false });
                }
            }
            catch
            {
                this.SetErrorNotification("Sản phẩm này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator", blank = false });
        }

        public FileResult DownloadOrderDetail(int id)
        {
            string filePath;
            var orderDetail = OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);
            if (orderDetail == null)
            {
                filePath = _orderDetailRepository.GetById(id).FilePath;
            }
            else
            {
                filePath = orderDetail.FilePath;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(filePath));
            string fileName = Path.GetFileName(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult UploadOrderDetailFile(OrderDetailModel model)
        {
            try
            {
                var orderDetail = OrderDetails.FirstOrDefault(p => p.OrderDetailId == model.OrderDetailId);
                //if (orderDetail.OrderDetailId > 0)
                //{
                //    using (UnitOfWork)
                //    {
                //        var entity = _orderDetailRepository.GetById(orderDetail.OrderDetailId);
                //        entity.FilePath = model.FilePath;
                //        entity.Note = model.Note;
                //        _orderDetailRepository.Update(entity);
                //    }
                //}
                orderDetail.FilePath = model.FilePath;
                orderDetail.Note = model.Note;
                this.SetSuccessNotification("Cập nhật file thành công!");
                if (orderDetail.OrderId > 0)
                {
                    return RedirectToAction("Edit", "Order", new { area = "Administrator", id = orderDetail.OrderId, backId = 1, blank = false });
                }
                else
                {
                    return RedirectToAction("Create", "Order", new { area = "Administrator" });
                }
            }
            catch
            {
                this.SetErrorNotification("Đã có lỗi xảy ra, không cập nhật được file!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        public ActionResult GetProducts(int? categoryId)
        {
            var products = _productRepository.Search("").Where(p => p.CategoryId == categoryId).Select(p=> new {Id = p.ProductId, Name = p.ProductName}).ToList();
            return Json(products);
        }
        private void FillCostForOrderDetail(List<OrderDetailModel> orderDetails)
        {
            //Caculate all order details
            foreach (var orderDetailModel in OrderDetails)
            {
                decimal defaultPrice = 0;
                decimal totalCost = 0;
                decimal extraFeeCost = 0;
                var amount = 0;
                if (orderDetailModel.Amount > 503)
                {
                    amount = orderDetailModel.Amount - 5;
                }
                else
                {
                    amount = orderDetailModel.Amount - 3;
                }
                var product = _productRepository.GetById(orderDetailModel.ProductId);
                var rateMapping = _rateMappingRepository.Search("").FirstOrDefault(p => p.MyOfficeId == OrderSession.MyOfficeId && p.ProductId == orderDetailModel.ProductId);
                if (rateMapping != null)
                {
                    totalCost += amount * rateMapping.Price;
                    defaultPrice = rateMapping.Price;
                    if (orderDetailModel.PrintIncludeImage && product.Printable)
                    {
                        totalCost += amount * rateMapping.PrintingIncludeImagePrice;
                        defaultPrice += rateMapping.PrintingIncludeImagePrice;
                    }
                    if (orderDetailModel.PrintWithoutImage && product.Printable)
                    {
                        totalCost += amount * rateMapping.PrintingWithoutImagePrice;
                        defaultPrice += rateMapping.PrintingWithoutImagePrice;
                    }
                }
                else
                {
                    totalCost += amount * product.DefaultPrice;
                    defaultPrice = product.DefaultPrice;
                    if (orderDetailModel.PrintIncludeImage && product.Printable)
                    {
                        totalCost += amount * product.DefaultPrintingIncludeImagePrice;
                        defaultPrice += product.DefaultPrintingIncludeImagePrice;
                    }
                    if (orderDetailModel.PrintWithoutImage && product.Printable)
                    {
                        totalCost += amount * product.DefaultPrintingWithoutImagePrice;
                        defaultPrice += product.DefaultPrintingWithoutImagePrice;
                    }
                }
                //Add extra fee
                if (product.Printable)
                {
                    var extraFee = _extraFeeRepository.GetAll().FirstOrDefault(p => p.AmountFrom < amount && amount < p.AmountTo && p.IsDeleted == false);
                    if (extraFee != null)
                    {
                        extraFeeCost += extraFee.Cost;
                        totalCost += extraFee.Cost;
                    }
                }
                orderDetailModel.ExtraFee = extraFeeCost;
                orderDetailModel.TotalCost = totalCost;
                orderDetailModel.Price = defaultPrice;
            }
        }
        
        #endregion

        #region Kinh doanh Cập nhật file
        public ActionResult UploadFile()
        {
            return View();
        }

        [GridAction]
        public ActionResult UploadFileGridModel(string search)
        {
            // Đơn hàng chưa có file hoặc đang chờ duyệt in & thuộc chi nhánh user quản lý
            var model = from x in Repository.Search(search)
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .Where(p => p.OrderDetails.All(o => (string.IsNullOrEmpty(o.FilePath)) || p.WaitForPrint) && WorkContext.MyOffices.Contains(p.MyOfficeId))
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate)
                        join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName= x.Customer.CustomerName,
                            CustomerPhone = x.Customer.PhoneNumber,
                            MyOfficeIName= x.MyOffice.OfficeName,
                            ShowOnTop = x.ShowOnTop,
                            ShippingServiceName = (m2 == null ? x.Customer.CustomerShortName : m2.ShippingServiceName)
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Bộ Phận Kho Xác Nhận
        public ActionResult StorageStaffApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult StorageStaffApproveGridModel(string search)
        {
            var model = Repository.Search(search).Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) &&
                !p.ApproveDeliveryFromStorageStaff &&
                p.OrderDetails.Any(o => !string.IsNullOrEmpty(o.FilePath))).Include(p => p.OrderDetails);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop,
                    Printable = x.OrderDetails.Any(p=>p.PrintIncludeImage || p.PrintWithoutImage)
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult StorageStaffApproveDone(int id)
        {
            var entity = Repository.GetAll().Where(p => p.OrderId == id).Include(p => p.OrderDetails).FirstOrDefault();
            using (UnitOfWork)
            {
                entity.ApproveDeliveryFromStorageStaff = true;
                entity.ApproveDeliveryFromStorageStaffAt = DateTime.Now;
                entity.ApproveDeliveryFromStorageStaffId = WorkContext.CurrentUserId;
                entity.Status = (int)OrderStatus.ReadyInStorage;
                //Update Order
                Repository.Update(entity);
                //Update ExportTrack
                using (UnitOfWork)
                {
                    var user = _userRepository.GetAll().FirstOrDefault();
                    var exportTrack = new ExportTrack
                    {
                        IsDeleted = false,
                        CreateDate = DateTime.Now,
                        CreateUserId = WorkContext.CurrentUserId,
                        Note = entity.Note,
                        MyOfficeId = entity.MyOfficeId,
                        ReceivedUserId = user.UserId,
                        OrderId = entity.OrderId,
                        ExportDetails = new List<ExportDetail>()
                    };
                    foreach (var orderDetail in entity.OrderDetails)
                    {
                        exportTrack.ExportDetails.Add(new ExportDetail
                        { 
                            Amount = orderDetail.Amount,
                            IsDeleted = false,
                            ProductId = orderDetail.ProductId,
                            StorageId = orderDetail.StorageId
                        });
                        _exportTrackRepository.Insert(exportTrack);
                    }
                }
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("StorageStaffApprove", "Order", new { area = "Administrator" });
        }
        #endregion

        #region Bộ phận in
        public ActionResult Printing()
        {
            return View();
        }

        [GridAction]
        public ActionResult PrintingGridModel(string search)
        {
            //Đơn hàng đã có file không chờ duyệt in  & trạng thái chưa in & ko là khách lẻ
            //Hoặc Đơn hàng đã có file & không chờ duyệt in  & trạng thái chưa in & đã thanh toán & là khách lẻ
            //& đơn hàng thuộc chi nhánh user quản lý 
            //& đơn hàng phải không phải nhận ở văn phòng
            var model = Repository.Search(search)
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .Where(p =>(p.OrderDetails.Any(o => (!string.IsNullOrEmpty(o.FilePath)) && !p.WaitForPrint && p.Status < (int)OrderStatus.Printed && !p.MyOffice.IsRetailCustomer)
                    || p.OrderDetails.Any(o => (!string.IsNullOrEmpty(o.FilePath)) && !p.WaitForPrint && p.Status < (int)OrderStatus.Printed && p.Paid && p.MyOffice.IsRetailCustomer))
                    && WorkContext.MyOffices.Contains(p.MyOfficeId))
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate).ToList();
            var listOrderModel = new List<OrderModel>();
            var printUserid = model.Select(u => u.PrintByUserId).AsEnumerable();
            var listUser = _userRepository.GetAll().Where(p => printUserid.Contains(p.UserId)).ToList();
            foreach (var item in model)
            {
                var orderModel = new OrderModel
                {
                    OrderId = item.OrderId,
                    CreateDate = item.CreateDate,
                    Status = item.Status,
                    TotalCost = item.TotalCost,
                    CustomerName = item.Customer.CustomerName,
                    MyOfficeIName = item.MyOffice.OfficeName,
                    ShowOnTop = item.ShowOnTop,
                    Stoppping = item.Stopping,
                };
                if (item.PrintByUserId.HasValue)
                {
                    var user = listUser.FirstOrDefault(p => p.UserId == item.PrintByUserId);
                    orderModel.PrintUserName = user.FirstName + " " + user.LastName;
                }
                listOrderModel.Add(orderModel);
            }

            var gridModel = new GridModel<OrderModel>
            {
                Data = listOrderModel
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult PrintDone(int id)
        {
            var entity = Repository.GetById(id);
            if (entity.Status < (int) OrderStatus.Printing)
            {
                using (UnitOfWork)
                {
                    entity.Status = (int)OrderStatus.Printing;
                    entity.PrintByUserId = WorkContext.CurrentUserId;
                    Repository.Update(entity);
                }
            }
            else
            {
                if (entity.PrintByUserId != WorkContext.CurrentUserId)
                {
                    var user = _userRepository.GetById(entity.PrintByUserId);
                    SetErrorNotification(string.Format("Nhân viên {0} {1} đang in đơn hàng này!", user.FirstName, user.LastName));
                    return RedirectToAction("Printing", "Order", new { area = "Administrator" });
                }
                using (UnitOfWork)
                {
                    entity.Status = (int)OrderStatus.Printed;
                    entity.PrintDoneAt = DateTime.Now;
                    entity.PrintByUserId = WorkContext.CurrentUserId;
                    Repository.Update(entity);
                }
            }
            this.SetSuccessNotification("Đã cập nhật thành công!");
            return RedirectToAction("Printing", "Order", new { area = "Administrator" });
        }

        public ActionResult ListOrderDetailPrint(int id, int? mode)
        {
            var orderModel = new OrderModel
            {
                OrderId = id,
                Mode = mode
            };
            return View(orderModel);
        }

        [GridAction]
        public ActionResult GridModelOrderDetailPrint(int? orderId, int? mode)
        {
            if (orderId.HasValue)
            {
                var orderDetails = _orderDetailRepository.Search("").Where(p => p.OrderId == orderId.Value && (p.Product.Printable || mode.HasValue)).Select(p => new OrderDetailModel
                {
                    Amount = p.Amount,
                    FilePath = p.FilePath,
                    Note = p.Note,
                    OrderId = p.OrderId,
                    ProductId = p.ProductId,
                    CategoryId = p.Product.CategoryId,
                    ProductName = p.Product.ProductName,
                    CategoryName = p.Product.Category.CategoryName,
                    StorageId = p.StorageId,
                    TotalCost = p.TotalCost,
                    OrderDetailId = p.OrderDetailId,
                    PrintIncludeImage = p.PrintIncludeImage,
                    PrintWithoutImage = p.PrintWithoutImage,
                    Printable = p.Product.Printable,
                    GroupId=p.UpdateUserId.Value
                }).ToList();
                foreach (var item in orderDetails)
                {
                    if (item.Printable)
                    {
                        item.DownloadActionLink = Url.Action("DownloadOrderDetail", "Order", new { area = "Administrator" });
                        item.DownloadImagePath = Url.Content("~/Content/Images/download_16_16.png");
                    }
                }
                var model = new List<OrderDetailModel>();
                foreach (var item in orderDetails)
                {
                    if (!model.Any(p => p.CategoryId == item.CategoryId))
                    {
                        var orderDetails2 = orderDetails.Where(p => p.CategoryId == item.CategoryId).ToList();
                        foreach (var item1 in orderDetails2)
                        {
                            if (!model.Any(p => p.GroupId == item1.GroupId))
                            {
                                var orderDetails1 = orderDetails2.Where(p => p.GroupId == item1.GroupId).ToList();
                                //trường hợp bộ hoặc phôi
                                if (orderDetails1.Count() == 2)
                                {
                                    var orderDetailGroup = orderDetails1.Where(p => p.Printable).FirstOrDefault();
                                    if (orderDetailGroup.PrintIncludeImage || orderDetailGroup.PrintWithoutImage)
                                    {
                                        orderDetailGroup.ProductName = orderDetailGroup.CategoryName + " Bộ";
                                    }
                                    else
                                    {
                                        orderDetailGroup.ProductName = orderDetailGroup.CategoryName + " Phôi";
                                    }
                                    orderDetailGroup.PriceByCategory = orderDetails1[0].Price + orderDetails1[1].Price;
                                    orderDetailGroup.TotalCostByCategory = orderDetails1[0].TotalCost + orderDetails1[1].TotalCost;
                                    model.Add(orderDetailGroup);
                                }
                                else
                                {
                                    model.Add(item1);
                                }
                            }
                        }
                    }
                }
                var gridModel1 = new GridModel<OrderDetailModel>
                {
                    Data = model
                };
                return new JsonResult
                {
                    Data = gridModel1
                };
            }
            return new JsonResult
            {
                Data = null
            };
        }

        #endregion

        #region Bộ phận giao hàng
        public ActionResult Delivery()
        {
            return View();
        }

        [GridAction]
        public ActionResult DeliveryGridModel(string search)
        {
            //Đơn hàng đã được in & đơn hàng thuộc chi nhánh user quản lý & đơn hàng phải không phải nhận ở văn phòng
            var model = from x in Repository.Search(search)
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .Where(p => p.Status <= (int)OrderStatus.Printed && WorkContext.MyOffices.Contains(p.MyOfficeId) && p.PaymentType != (int)PaymentTypes.InOffice)
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate)
                        join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            MyOfficeId = x.MyOfficeId,
                            ShowOnTop = x.ShowOnTop,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ShippingServiceName = (m2 == null ? x.Customer.CustomerShortName : m2.ShippingServiceName)
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult DeliveryPackage()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices
            };
            return View(model);
        }
        [GridAction]
        public ActionResult DeliveryPackageGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var model = GetDeliveryPackage(filterModel);

            var gridModel = new GridModel<OrderDeliveryPackageModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        private IQueryable<OrderDeliveryPackageModel> GetDeliveryPackage(ReportFilterModel filterModel)
        {
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
            var isGetAllOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllOffices = false;
            }
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
            Session["DeliveryPackageFilter"] = filterModel;
            var model = from x in _orderDeliveryPackageRepository.Search(filterModel.search).Where(p =>
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        join m in _userRepository.GetAll() on x.CreateUserId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join s in _userRepository.GetAll() on x.DeliveryUserId equals s.UserId into s1
                        from s2 in s1.DefaultIfEmpty()
                        select new OrderDeliveryPackageModel
                        {
                            OrderDeliveryPackageId = x.OrderDeliveryPackageId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            ShippingFee = x.ShippingFee,
                            CreateUserName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            DeliveryUserName = (s2 == null ? String.Empty : s2.FirstName + " " + s2.LastName),
                        };
            return model;
        }

        public ActionResult CreateDeliveryPackage(string orders)
        {
            var firstItem = orders.Split(',').FirstOrDefault();
            var order = Repository.GetById(int.Parse(firstItem));
            //var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var users = _userRepository.Search("").Where(p => p.MyOffices.Select(o=> o.MyOfficeId).Contains(order.MyOfficeId) && p.Roles.Select(t=> t.RoleName).Contains(RoleType.NVGiaoHangTinh)).ToList();
            var shippingFees = _shippingFeeRepository.Search("").Where(p => p.MyOfficeId == order.MyOfficeId).ToList();
            var model = new OrderDeliveryPackageModel
            {
                Orders = orders,
                MyOfficeId = order.MyOfficeId,
                ShippingFees = shippingFees,
                Users = users
            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult SaveDeliveryPackage(OrderDeliveryPackageModel model)
        {
            if (model.OrderDeliveryPackageId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var firstItem = model.Orders.Split(',').FirstOrDefault();
                    var order = Repository.GetById(int.Parse(firstItem));
                    var users = _userRepository.Search("").Where(p => p.MyOffices.Select(o => o.MyOfficeId).Contains(order.MyOfficeId) && p.Roles.Select(t => t.RoleName).Contains(RoleType.NVGiaoHangTinh)).ToList();
                    var shippingFees = _shippingFeeRepository.Search("").Where(p => p.MyOfficeId == order.MyOfficeId).ToList();
                    model.ShippingFees = shippingFees;
                    model.Users = users;
                    return View("CreateDeliveryPackage", model);
                }
                
                var entity = new OrderDeliveryPackage
                {
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = model.Note,
                    MyOfficeId = model.MyOfficeId,
                    ShippingFee = model.ShippingFee,
                    DeliveryUserId = model.DeliveryUserId,
                    Orders = new List<Order>()
                };
                var orderInts = model.Orders.Split(',').Select(p => int.Parse(p)).ToList();
                foreach (var item in orderInts)
                {
                    var order = Repository.GetById(item);
                    entity.CustomerId = order.CustomerId;
                    entity.Orders.Add(order);
                    order.Status = (int)OrderStatus.Packaged;
                    order.ApproveDeliveryFromGeneralDeliveryMan = true;
                    order.ApproveDeliveryFromGeneralDeliveryManAt = DateTime.Now;
                    order.ApproveDeliveryFromGeneralDeliveryManId = WorkContext.CurrentUserId;
                    Repository.Update(order);
                }

                using (UnitOfWork)
                {
                    _orderDeliveryPackageRepository.Insert(entity);
                }
                this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Đóng gói đơn hàng"));
            }
            return RedirectToAction("Delivery", new { area = "Administrator" });
        }

        public ActionResult ApproveDeliveryFromGeneralDeliveryMan(OrderModel model)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(model.OrderId);
                entity.Status = (int)OrderStatus.Packaged;
                entity.ApproveDeliveryFromGeneralDeliveryMan = true;
                entity.ApproveDeliveryFromGeneralDeliveryManAt = DateTime.Now;
                entity.ApproveDeliveryFromGeneralDeliveryManId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Cập nhật đơn hàng thành công!");
            return RedirectToAction("DeliveryStaffApprove", "Order", new { area = "Administrator" });
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult DownloadDeliveryPackage()
        {
            var filterModel = (ReportFilterModel)Session["DeliveryPackageFilter"];
            var reportFilter = new ReportFilter { FromDate = filterModel.FromDate, ToDate = filterModel.ToDate,ListOffices = filterModel.ListOffices, AllowListOffices = WorkContext.MyOffices};
            var fileName = string.Format("DonHangCanGiao_{0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            return PdfResult(_exportService.DownloadDeliveryPackage(reportFilter), fileName);
        }

        [GridAction]
        public ActionResult OrderDeliveryGridModel(string orders)
        {
            var orderInts = orders.Split(',').Select(p => int.Parse(p)).ToList();
            var model = Repository.GetAll().Where(p => orderInts.Contains(p.OrderId))
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate).ToList();
            var printUserids = model.Select(u => u.PrintByUserId).AsEnumerable();
            var listPrintUser = _userRepository.GetAll().Where(p => printUserids.Contains(p.UserId)).ToList();
            var deliveryUserids = model.Select(u => u.DeliveredUserId).AsEnumerable();
            var listDeliveryUser = _userRepository.GetAll().Where(p => deliveryUserids.Contains(p.UserId)).ToList();
            var listOrderModel = new List<OrderModel>();
            foreach (var item in model)
            {
                var printUser = listPrintUser.FirstOrDefault(p => p.UserId == item.PrintByUserId);
                var deliveryUser = listDeliveryUser.FirstOrDefault(p => p.UserId == item.DeliveredUserId);
                listOrderModel.Add(new OrderModel
                {
                    OrderId = item.OrderId,
                    CreateDate = item.CreateDate,
                    Status = item.Status,
                    TotalCost = item.TotalCost,
                    CustomerName = item.Customer.CustomerName,
                    MyOfficeIName = item.MyOffice.OfficeName,
                    Paid = item.Paid,
                    Stoppping = item.Stopping,
                    PrintUserName = printUser == null ? string.Empty : printUser.FirstName + " " + printUser.LastName,
                    DeliveredUserName = deliveryUser == null ? string.Empty : deliveryUser.FirstName + " " + deliveryUser.LastName
                });
            }
            var gridModel = new GridModel<OrderModel>
            {
                Data = listOrderModel
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ListOrdersInAPackage(int id)
        {
            var entity = _orderDeliveryPackageRepository.GetAll().Where(p => p.OrderDeliveryPackageId == id).Include(p => p.Orders).FirstOrDefault();
            object model = string.Join(",", entity.Orders.Select(p=>p.OrderId).ToList());
            return View("_ListOrdersInAPackage", model);

        }
        #endregion

        #region Duyệt đơn hàng giao trong ngày cho chi nhánh

        public ActionResult ApproveDeliveryInDay()
        {
            return View();
        }

        [GridAction]
        public ActionResult ApproveDeliveryInDayGridModel(string search)
        {
            var model = from x in Repository.Search(search)
                        .Where( p => WorkContext.MyOffices.Contains(p.MyOfficeId) && !p.ApproveDeliveryInDay && p.Status >= (int) OrderStatus.Packaged)
                        .Include(p => p.OrderDetails)
                        join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            Printable = x.OrderDetails.Any(p => p.PrintIncludeImage || p.PrintWithoutImage),
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ShippingServiceName = (m2 == null ? x.Customer.CustomerShortName : m2.ShippingServiceName),
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ApproveDeliveryInDayDone(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.Status = (int)OrderStatus.ReadyInStateprovince;
                entity.ApproveDeliveryInDay = true;
                entity.ApproveDeliveryInDayAt = DateTime.Now;
                entity.ApproveDeliveryInDayBy = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Cập nhật đơn hàng thành công!");
            return RedirectToAction("ApproveDeliveryInDay", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportApproveDeliveryInDay()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices
            };
            return View(model);
        }

        [GridAction]
        public ActionResult ReportApproveDeliveryInDayGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
            var isGetAllOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllOffices = false;
            }
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
            var model = from x in Repository.Search(filterModel.search).Where(p => (WorkContext.Roles.Contains(RoleType.Administrator) || WorkContext.Roles.Contains(RoleType.QuanLyChung)) && p.ApproveDeliveryInDay &&
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.OrderDetails)
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            Printable = x.OrderDetails.Any(p => p.PrintIncludeImage || p.PrintWithoutImage),
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Giao hàng tỉnh
        public ActionResult DeliveryStaffApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult DeliveryStaffApproveGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.Status >= (int)OrderStatus.Packaged && p.Status < (int)OrderStatus.BeDelivered).Include(p => p.OrderDetails)
                            join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                            from m2 in m1.DefaultIfEmpty()
                            select new OrderModel
                            {
                                OrderId = x.OrderId,
                                CreateDate = x.CreateDate,
                                Status = x.Status,
                                TotalCost = x.TotalCost,
                                CustomerName = x.Customer.CustomerName,
                                MyOfficeIName = x.MyOffice.OfficeName,
                                Paid = x.Paid,
                                Stoppping = x.Stopping,
                                ShowOnTop = x.ShowOnTop,
                                Printable = x.OrderDetails.Any(p => p.PrintIncludeImage || p.PrintWithoutImage),
                                PaymentType = x.PaymentType,
                                CustomerId = x.CustomerId,
                                CustomerAddress = x.Customer.Address,
                                CustomerPhone = x.Customer.PhoneNumber,
                                ShippingServiceAt = (m2 == null ? DateTime.Now : m2.StartAt),
                                ShippingServiceName = (m2 == null ? x.Customer.CustomerShortName : m2.ShippingServiceName),
                                ShippingServicePhone = (m2 == null ? String.Empty : m2.PhoneNumber),
                            }; 

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ReportDeliveryStaffApprove()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices
            };
            return View(model);
        }

        [GridAction]
        public ActionResult ReportDeliveryStaffApproveGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
            var isGetAllOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllOffices = false;
            }
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
            var model = from x in Repository.Search(filterModel.search).Where(p => (p.DeliveredUserId == WorkContext.CurrentUserId || WorkContext.Roles.Contains(RoleType.Administrator) || WorkContext.Roles.Contains(RoleType.QuanLyChung)) &&
                p.Status == (int)OrderStatus.BeDelivered && p.PaidForDeliveryStaff &&
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                ((p.DeliveredDate >= fromDate || isAllFromDate) && (p.DeliveredDate <= toDate || isAllToDate))).Include(p => p.OrderDetails)
                        join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            Printable = x.OrderDetails.Any(p => p.PrintIncludeImage || p.PrintWithoutImage),
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ShippingServiceAt = (m2 == null ? DateTime.Now : m2.StartAt),
                            ShippingServiceName = (m2 == null ? x.Customer.CustomerShortName : m2.ShippingServiceName),
                            ShippingServicePhone = (m2 == null ? String.Empty : m2.PhoneNumber),
                            DeliveredDate = x.DeliveredDate
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult DeliveryStaffApproveConfirmPopup()
        {
            return View("_DeliveryStaffApproveConfirmPopup", new OrderModel { DeliveredDate = DateTime.Now });
        }

        public ActionResult DeliveryStaffApproveDone(OrderModel model)
        {
            var entity = Repository.GetAll().Where(p => p.OrderId == model.OrderId).Include(p => p.OrderDetails).FirstOrDefault();
            using (UnitOfWork)
            {
                entity.Status = (int)OrderStatus.BeDelivered;
                entity.PaidForDeliveryStaff = true;
                entity.DeliveredDate = model.DeliveredDate;
                entity.DeliveredUserId = WorkContext.CurrentUserId;
                //Update Order
                using (UnitOfWork)
                {
                    Repository.Update(entity);
                }
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("DeliveryStaffApprove", "Order", new { area = "Administrator" });
        }

        public ActionResult OrderBeDelivered(OrderModel model)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(model.OrderId);
                entity.Status = (int)OrderStatus.BeDelivered;
                entity.DeliveredDate = DateTime.Now;
                entity.DeliveredUserId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Cập nhật đơn hàng thành công!");
            return RedirectToAction("DeliveryStaffApprove", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportNotDeliveryYet()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices
            };
            return View(model);
        }

        [GridAction]
        public ActionResult ReportNotDeliveryYetModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
            var isGetAllOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllOffices = false;
            }
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
            var model = from x in Repository.Search(filterModel.search).Where(p => (p.DeliveredUserId == WorkContext.CurrentUserId || WorkContext.Roles.Contains(RoleType.Administrator) || WorkContext.Roles.Contains(RoleType.QuanLyChung)) &&
                p.Status >= (int)OrderStatus.ReadyInStorage && p.Status < (int)OrderStatus.BeDelivered &&
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.OrderDetails)
                        join m in _shippingServiceRepository.GetAll() on x.ShippingServiceId equals m.ShippingServiceId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            Printable = x.OrderDetails.Any(p => p.PrintIncludeImage || p.PrintWithoutImage),
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ShippingServiceAt = (m2 == null ? DateTime.Now : m2.StartAt),
                            ShippingServiceName = (m2 == null ? String.Empty : m2.ShippingServiceName),
                            ShippingServicePhone = (m2 == null ? String.Empty : m2.PhoneNumber),
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region PriceApproval
        public ActionResult PriceApproval()
        {
            return View();
        }

        [GridAction]
        public ActionResult PriceApprovalGridModel(string search)
        {
            var model = Repository.Search(search)
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .Where(p => !p.Paid && WorkContext.MyOffices.Contains(p.MyOfficeId))
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    ShowOnTop = x.ShowOnTop
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult PricingApproved(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.Paid = true;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Cập nhật đơn hàng thành công!");
            return RedirectToAction("PriceApproval", "Order", new { area = "Administrator" });
        }
        #endregion
                
        #region Kế Toán quản lý thu tiền mặt của khách
        public ActionResult NotPaid()
        {
            return View();
        }

        [GridAction]
        public ActionResult NotPaidGridModel(string search)
        {
            var model = Repository.Search(search)
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .Where(p => p.Status <= (int)OrderStatus.BeDelivered && !p.Paid && WorkContext.MyOffices.Contains(p.MyOfficeId))
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    ShowOnTop = x.ShowOnTop
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult OrderBePaid(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.Paid = true;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Cập nhật đơn hàng thành công!");
            return RedirectToAction("NotPaid", "Order", new { area = "Administrator" });
        }
        #endregion

        #region Đơn hàng tạm dừng
        [ChildActionOnly]
        public ActionResult OrderIsStopping()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderIsStoppingGridModel(string search)
        {
            var model = Repository.Search(search).Where(p => p.Stopping && WorkContext.MyOffices.Contains(p.MyOfficeId))
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .OrderBy(p => p.ShowOnTop).ThenByDescending(p => p.CreateDate);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Bộ phận in xác nhận hoàn tất
        [ChildActionOnly]
        public ActionResult OrderIsPrintIsDone()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderIsPrintIsDoneGridModel(string search)
        {
            var model = from x in Repository.Search(search)
                .Include(p => p.MyOffice).Include(p => p.Customer)
                .Where(p => p.Status == (int)OrderStatus.Printed && WorkContext.MyOffices.Contains(p.MyOfficeId))
                join p in _userRepository.GetAll() on x.PrintByUserId equals p.UserId
                            select new OrderModel
                            {
                                OrderId = x.OrderId,
                                CreateDate = x.CreateDate,
                                Status = x.Status,
                                TotalCost = x.TotalCost,
                                CustomerName = x.Customer.CustomerName,
                                MyOfficeIName = x.MyOffice.OfficeName,
                                MyOfficeId = x.MyOfficeId,
                                ShowOnTop = x.ShowOnTop,
                                PrintUserName = p.FirstName + " " + p.LastName
                            };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Đơn hàng hủy

        #region Bộ phận in
        public ActionResult OrderCancelForPrint()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderCancelForPrintGridModel(string search)
        {
            var model = Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.Cancel && !p.ApproveCancelFromPrinter && p.Status >= (int)OrderStatus.ReadyInStorage);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop,
                    PaymentType = x.PaymentType,
                    CustomerAddress = x.Customer.Address,
                    CustomerPhone = x.Customer.PhoneNumber,
                    ExtraFee = x.ExtraFee
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ApproveOrderCancelForPrintDone(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.ApproveCancelFromPrinter = true;
                entity.ApproveCancelFromPrinterAt = DateTime.Now;
                entity.ApproveCancelFromPrinterId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Xác nhận hủy đơn hàng thành công!");
            return RedirectToAction("OrderCancelForPrint", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportApproveOrderCancelForPrint()
        {
            return View();
        }

        [GridAction]
        public ActionResult ReportApproveOrderCancelForPrintGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
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
            var model = from x in Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.ApproveCancelFromPrinter && p.Cancel &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ExtraFee = x.ExtraFee
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Bộ phận kinh doanh
        public ActionResult OrderCancelForSale()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderCancelForSaleGridModel(string search)
        {
            var model = Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.Cancel && !p.ApproveCancelFrombusinessMan);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop,
                    PaymentType = x.PaymentType,
                    CustomerAddress = x.Customer.Address,
                    CustomerPhone = x.Customer.PhoneNumber,
                    ExtraFee = x.ExtraFee
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ApproveOrderCancelForSaleDone(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.ApproveCancelFrombusinessMan = true;
                entity.ApproveCancelFrombusinessManAt = DateTime.Now;
                entity.ApproveCancelFrombusinessManId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Xác nhận hủy đơn hàng thành công!");
            return RedirectToAction("OrderCancelForSale", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportApproveOrderCancelForSale()
        {
            return View();
        }

        [GridAction]
        public ActionResult ReportApproveOrderCancelForSaleGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
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
            var model = from x in Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.ApproveCancelFrombusinessMan && p.Cancel &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ExtraFee = x.ExtraFee
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Bộ phận kho
        public ActionResult OrderCancelForStorage()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderCancelForStorageGridModel(string search)
        {
            var model = Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.Cancel && !p.ApproveCancelFromStorageStaff);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop,
                    PaymentType = x.PaymentType,
                    CustomerAddress = x.Customer.Address,
                    CustomerPhone = x.Customer.PhoneNumber,
                    ExtraFee = x.ExtraFee
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ApproveOrderCancelForStorageDone(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.ApproveCancelFromStorageStaff = true;
                entity.ApproveCancelFromStorageStaffAt = DateTime.Now;
                entity.ApproveCancelFromStorageStaffId = WorkContext.CurrentUserId;
                Repository.Update(entity);
                //foreach (var orderDetail in entity.OrderDetails)
                //{
                //    var productInStorage =
                //        _productInStorageRepository.Search("")
                //            .FirstOrDefault(
                //                p => p.ProductId == orderDetail.ProductId && p.StorageId == orderDetail.StorageId);
                //    if (productInStorage == null)
                //    {
                //        var entityProductInStorage = new ProductInStorage
                //        {
                //            Amount = 0,
                //            ProductId = orderDetail.ProductId,
                //            StorageId = orderDetail.StorageId,
                //            IsDeleted = false
                //        };
                //        _productInStorageRepository.Insert(entityProductInStorage);
                //    }
                //    else
                //    {
                //        productInStorage.Amount += orderDetail.Amount;
                //        _productInStorageRepository.Update(productInStorage);
                //    }
                //}
            }
            this.SetSuccessNotification("Xác nhận hủy đơn hàng thành công!");
            return RedirectToAction("OrderCancelForStorage", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportApproveOrderCancelForStorage()
        {
            return View();
        }

        [GridAction]
        public ActionResult ReportApproveOrderCancelForStorageGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
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
            var model = from x in Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.ApproveCancelFromStorageStaff && p.Cancel &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ExtraFee = x.ExtraFee
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Bộ phận giao hàng
        public ActionResult OrderCancelForDelivery()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderCancelForDeliveryGridModel(string search)
        {
            var model = Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.Cancel && !p.ApproveCancelFromDelivery);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop,
                    PaymentType = x.PaymentType,
                    CustomerAddress = x.Customer.Address,
                    CustomerPhone = x.Customer.PhoneNumber,
                    ExtraFee = x.ExtraFee
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ApproveOrderCancelForDeliveryDone(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.ApproveCancelFromDelivery = true;
                entity.ApproveCancelFromDeliveryAt = DateTime.Now;
                entity.ApproveCancelFromDeliveryId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Xác nhận hủy đơn hàng thành công!");
            return RedirectToAction("OrderCancelForDelivery", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportApproveOrderCancelForDelivery()
        {
            return View();
        }

        [GridAction]
        public ActionResult ReportApproveOrderCancelForDeliveryGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
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
            var model = from x in Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.ApproveCancelFromDelivery && p.Cancel &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ExtraFee = x.ExtraFee
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #region Quản lý chung
        public ActionResult OrderCancelForManager()
        {
            return View();
        }

        [GridAction]
        public ActionResult OrderCancelForManagerGridModel(string search)
        {
            var model = Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.Cancel && !p.ApproveCancelFromManager);

            var gridModel = new GridModel<OrderModel>
            {
                Data = model.Select(x => new OrderModel
                {
                    OrderId = x.OrderId,
                    CreateDate = x.CreateDate,
                    Status = x.Status,
                    TotalCost = x.TotalCost,
                    CustomerName = x.Customer.CustomerName,
                    MyOfficeIName = x.MyOffice.OfficeName,
                    Paid = x.Paid,
                    Stoppping = x.Stopping,
                    ShowOnTop = x.ShowOnTop,
                    PaymentType = x.PaymentType,
                    CustomerAddress = x.Customer.Address,
                    CustomerPhone = x.Customer.PhoneNumber,
                    ExtraFee = x.ExtraFee
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult OrderCancelForManagerDone(int id)
        {
            using (UnitOfWork)
            {
                var entity = Repository.GetById(id);
                entity.ApproveCancelFromManager = true;
                entity.ApproveCancelFromManagerAt = DateTime.Now;
                entity.ApproveCancelFromManagerId = WorkContext.CurrentUserId;
                Repository.Update(entity);
                foreach (var orderDetail in entity.OrderDetails)
                {
                    var productInStorage =
                        _productInStorageRepository.Search("")
                            .FirstOrDefault(
                                p => p.ProductId == orderDetail.ProductId && p.StorageId == orderDetail.StorageId);
                    if (productInStorage == null)
                    {
                        var entityProductInStorage = new ProductInStorage
                        {
                            Amount = 0,
                            ProductId = orderDetail.ProductId,
                            StorageId = orderDetail.StorageId,
                            IsDeleted = false
                        };
                        _productInStorageRepository.Insert(entityProductInStorage);
                    }
                    else
                    {
                        productInStorage.Amount += orderDetail.Amount;
                        _productInStorageRepository.Update(productInStorage);
                    }
                }
            }
            this.SetSuccessNotification("Xác nhận hủy đơn hàng thành công!");
            return RedirectToAction("OrderCancelForDelivery", "Order", new { area = "Administrator" });
        }

        public ActionResult ReportApproveOrderCancelForManager()
        {
            return View();
        }

        [GridAction]
        public ActionResult ReportApproveOrderCancelForManagerGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            var listOffices = new int[1];
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
            var model = from x in Repository.GetAll().Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) && p.ApproveCancelFromManager && p.Cancel &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Status = x.Status,
                            TotalCost = x.TotalCost,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            Paid = x.Paid,
                            Stoppping = x.Stopping,
                            ShowOnTop = x.ShowOnTop,
                            PaymentType = x.PaymentType,
                            CustomerAddress = x.Customer.Address,
                            CustomerPhone = x.Customer.PhoneNumber,
                            ExtraFee = x.ExtraFee
                        };

            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

        #endregion
    }
}
