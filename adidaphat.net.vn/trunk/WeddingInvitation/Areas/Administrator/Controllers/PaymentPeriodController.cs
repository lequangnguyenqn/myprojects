using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Orders;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Users;
using WeddingInvitation.Services.Customers;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class PaymentPeriodController : ControllerBase<IPaymentPeriodRepository, PaymentPeriod>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IExportService _exportService;

        public PaymentPeriodController(IUnitOfWork unitOfWork, IPaymentPeriodRepository repository,
            IMyOfficeRepository myOfficeRepository, IUserRepository userRepository, IOrderRepository orderRepository,
            ICustomerRepository customerRepository, IExportService exportService)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _exportService = exportService;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices,
                Customers = customers
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
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listCustomers = new int[1];
            var isGetAllListCustomers = true;
            //Filter by deliver to
            if (filterModel.ListCustomers != null && filterModel.ListCustomers.Count() > 0)
            {
                listCustomers = filterModel.ListCustomers;
                isGetAllListCustomers = false;
            }
            var model = from x in _orderRepository.Search("").Where(p => p.TotalPaid <= p.TotalCost && !p.Paid &&
                        (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                        (listCustomers.Contains(p.CustomerId) || (isGetAllListCustomers)) &&
                        ((p.PaymentType == (int)PaymentTypes.Tranfer && filterModel.Tranfer) || !filterModel.Tranfer) &&
                        ((p.PaymentType == (int)PaymentTypes.InShippingPlace && filterModel.InShippingPlace) || !filterModel.InShippingPlace) &&
                        ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.Customer)
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            PaymentType = x.PaymentType,
                            CustomerName = x.Customer.CustomerName,
                            MyOfficeIName = x.MyOffice.OfficeName,
                            TotalCost =x.TotalCost,
                            TotalPaid = x.TotalPaid,
                            NeedPaid = x.TotalCost - x.TotalPaid,
                            Paid = x.Paid
                        };
            Session["PaymentPeriodFilterModel"] = filterModel;
            double total = 0;
            try
            {
                total = Convert.ToDouble(model.Sum(p=>p.NeedPaid));
            }
            catch
            {
            }
            Session["totalneedpaid"] = String.Format("{0:0,0}", total);
            var gridModel = new GridModel<OrderModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult GetTotalNeedPaid()
        {
            return Json(Session["totalneedpaid"]);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Download()
        {
            var filterModel = (ReportFilterModel)Session["PaymentPeriodFilterModel"];
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
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listCustomers = new int[1];
            var isGetAllListCustomers = true;
            //Filter by deliver to
            if (filterModel.ListCustomers != null && filterModel.ListCustomers.Count() > 0)
            {
                listCustomers = filterModel.ListCustomers;
                isGetAllListCustomers = false;
            }
            var model = from x in _orderRepository.Search("").Where(p => p.TotalPaid < p.TotalCost &&
                        (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                        (listCustomers.Contains(p.CustomerId) || (isGetAllListCustomers)) &&
                        ((p.PaymentType == (int)PaymentTypes.Tranfer && filterModel.Tranfer) || !filterModel.Tranfer) &&
                        ((p.PaymentType == (int)PaymentTypes.InShippingPlace && filterModel.InShippingPlace) || !filterModel.InShippingPlace) &&
                        ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.Customer)
                        select x;
            var fileName = string.Format("Danh_Sach_Thu_Tien_Mat_Cua_Khac{0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            var reportFilter = new ReportFilter {FromDate = fromDate, ToDate = toDate, AllowListOffices = listOffices};
            return PdfResult(_exportService.DownloadPaymentPeriod(model, reportFilter), fileName);
        }

        public ActionResult Create(int id)
        {
            var model = new PaymentPeriodModel { OrderId = id, CreateDate = DateTime.Now};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new PaymentPeriodModel()
            {
                CreateDate = entity.CreateDate,
                Note = entity.Note,
                OrderId = entity.OrderId,
                Paid = entity.Paid,
                PaymentPeriodId =entity.PaymentPeriodId
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(PaymentPeriodModel myOfficeModel)
        {
            if (myOfficeModel.PaymentPeriodId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", myOfficeModel);
                }
                var myOffice = new PaymentPeriod()
                {
                    IsDeleted = false,
                    MyOfficeId = myOfficeModel.MyOfficeId,
                    Paid = myOfficeModel.Paid,
                    CreateDate = DateTime.Now,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = myOfficeModel.Note,
                    OrderId = myOfficeModel.OrderId
                };
                var order = _orderRepository.GetById(myOfficeModel.OrderId);
                order.TotalPaid += myOfficeModel.Paid;
                using (UnitOfWork)
                {
                    _orderRepository.Update(order);
                    Repository.Insert(myOffice);
                }
                //Save success
                this.SetSuccessNotification("Đã lưu thành công.");
                return RedirectToAction("Index", new { area = "Administrator"});
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.PaymentPeriodId);
                myOffice.Paid = myOfficeModel.Paid;
                myOffice.Note = myOfficeModel.Note;
                myOffice.CreateDate = myOfficeModel.CreateDate;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification("Đã được lưu thành công.");
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    var order = _orderRepository.GetById(entity.OrderId);
                    order.TotalPaid -= entity.Paid;
                    entity.IsDeleted = true;
                }
                this.SetSuccessNotification("Đã xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("ManagerApprove", new { area = "Administrator" });
        }

        #region Quản Lý Xác nhận thu tiền mặt của khách
        public ActionResult ManagerApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult ManagerApproveGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => !p.ApproveFromManager).Include(p => p.Order.Customer)
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new PaymentPeriodModel
                        {
                            PaymentPeriodId =x.PaymentPeriodId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            Paid = x.Paid,
                            PaymentType = x.Order.PaymentType,
                            CustomerName = x.Order.Customer.CustomerName,
                            MyOfficeName = x.Order.MyOffice.OfficeName,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName)
                        };

            var gridModel = new GridModel<PaymentPeriodModel>
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
            var order = _orderRepository.GetById(entity.OrderId);
            using (UnitOfWork)
            {
                if (order.TotalCost <= order.TotalPaid)
                {
                    order.Paid = true;
                    _orderRepository.Update(order);
                }
                entity.ApproveFromManager = true;
                entity.ApproveFromManagerAt = DateTime.Now;
                entity.ApproveFromManagerId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("ManagerApprove", "PaymentPeriod", new { area = "Administrator" });
        }

        #endregion

        #region Báo cáo công nợ  đã thu tiền mặt  của khách
        public ActionResult CustomerPaid()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
               MyOffices = offices,
               Customers = customers
            };
            return View(model);
        }

        [GridAction]
        public ActionResult CustomerPaidGridModel(ReportFilterModel filterModel)
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
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listCustomers = new int[1];
            var isGetAllListCustomers = true;
            //Filter by deliver to
            if (filterModel.ListCustomers != null && filterModel.ListCustomers.Count() > 0)
            {
                listCustomers = filterModel.ListCustomers;
                isGetAllListCustomers = false;
            }
            var model = from x in Repository.Search(filterModel.search).Where(p =>
                        (listOffices.Contains(p.Order.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.Order.MyOfficeId))) &&
                        (listCustomers.Contains(p.Order.CustomerId) || (isGetAllListCustomers)) &&
                        ((p.Order.PaymentType == (int)PaymentTypes.Tranfer && filterModel.Tranfer) || !filterModel.Tranfer) &&
                        ((p.Order.PaymentType == (int)PaymentTypes.InShippingPlace && filterModel.InShippingPlace) || !filterModel.InShippingPlace) &&
                        (listCustomers.Contains(p.Order.CustomerId) || (isGetAllListCustomers)) &&
                        ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.Order.Customer)
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new PaymentPeriodModel
                        {
                            PaymentPeriodId = x.PaymentPeriodId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            Paid = x.Paid,
                            PaymentType = x.Order.PaymentType,
                            CustomerName = x.Order.Customer.CustomerName,
                            MyOfficeName = x.Order.MyOffice.OfficeName,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName)
                        };
            double total = 0;
            try
            {
                total = Convert.ToDouble(model.Sum(p => p.Paid));
            }
            catch
            {
            }
            Session["TotalCustomerPaid"] = String.Format("{0:0,0}", total);
            var gridModel = new GridModel<PaymentPeriodModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult GetTotalCustomerPaid()
        {
            return Json(Session["TotalCustomerPaid"]);
        }

        public ActionResult CustomerPaidForManager()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices,
                Customers = customers
            };
            return View(model);
        }
        [GridAction]
        public ActionResult CustomerPaidForManagerGridModel(ReportFilterModel filterModel)
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
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listCustomers = new int[1];
            var isGetAllListCustomers = true;
            //Filter by deliver to
            if (filterModel.ListCustomers != null && filterModel.ListCustomers.Count() > 0)
            {
                listCustomers = filterModel.ListCustomers;
                isGetAllListCustomers = false;
            }
            var model = from x in Repository.Search(filterModel.search).Where(p => p.ApproveFromManager &&
                        (listOffices.Contains(p.Order.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.Order.MyOfficeId))) &&
                        (listCustomers.Contains(p.Order.CustomerId) || (isGetAllListCustomers)) &&
                        ((p.Order.PaymentType == (int)PaymentTypes.Tranfer && filterModel.Tranfer) || !filterModel.Tranfer) &&
                        ((p.Order.PaymentType == (int)PaymentTypes.InShippingPlace && filterModel.InShippingPlace) || !filterModel.InShippingPlace) &&
                        (listCustomers.Contains(p.Order.CustomerId) || (isGetAllListCustomers)) &&
                        ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.Order.Customer)
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        select new PaymentPeriodModel
                        {
                            PaymentPeriodId = x.PaymentPeriodId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            Paid = x.Paid,
                            PaymentType = x.Order.PaymentType,
                            CustomerName = x.Order.Customer.CustomerName,
                            MyOfficeName = x.Order.MyOffice.OfficeName,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName)
                        };
            double total = 0;
            try
            {
                total = Convert.ToDouble(model.Sum(p => p.Paid));
            }
            catch
            {
            }
            Session["TotalCustomerPaid"] = String.Format("{0:0,0}", total);
            var gridModel = new GridModel<PaymentPeriodModel>
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
