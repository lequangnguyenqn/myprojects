using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Orders;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Users;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class DebtController : ControllerBase<IDebtRepository, Debt>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public DebtController(IUnitOfWork unitOfWork, IDebtRepository repository,
            IMyOfficeRepository myOfficeRepository, IUserRepository userRepository, IOrderRepository orderRepository,
            ICustomerRepository customerRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
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
            var model = from x in Repository.Search("").Where(p =>
                        (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                        ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        select new DebtModel
                        {
                            DebtId = x.DebtId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            FromDate = x.FromDate,
                            ToDate= x.ToDate,
                            Total =x.Total,
                            Paid =x.Paid,
                            PaidLeft = x.Total -x.Paid,
                            MyOfficeName = x.MyOffice.OfficeName
                        };

            var gridModel = new GridModel<DebtModel>
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
            var model = new DebtModel
            {
                MyOffices = offices,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1),
                ToDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(DebtModel model)
        {
            if (model.DebtId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    model.MyOffices = offices;
                    return View("CreateDebt", model);
                }

                var entity = new Debt
                {
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = model.Note,
                    MyOfficeId = model.MyOfficeId,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    Total = model.Total,
                    Paid = model.Paid,
                };

                using (UnitOfWork)
                {
                    Repository.Insert(entity);
                }
                this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Công nợ"));
            }
            return RedirectToAction("Create", new { area = "Administrator" });
        }

        public ActionResult GetCost(int? myOfficeId, DateTime? fromDate, DateTime? toDate)
        {
            if (!myOfficeId.HasValue || !fromDate.HasValue || !toDate.HasValue)
            {
                return Json(new { Total = 0, Paid = 0, PaidLeft = 0 });
            }
            toDate = toDate.Value.AddDays(1);
            var model = (from x in _orderRepository.Search("").Where(p =>
                        (myOfficeId == p.MyOfficeId) &&
                        (p.CreateDate >= fromDate && p.CreateDate <= toDate))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            TotalCost = x.TotalCost,
                            TotalPaid = x.TotalPaid
                        }).ToList();
            var total = model.Sum(p => p.TotalCost);
            var paid = model.Sum(p => p.TotalPaid);
            return Json(new { Total = total, Paid = paid, PaidLeft = total - paid });
        }

        #region Quản Lý xác nhận công nợ cuối tháng /cuối kỳ theo chi nhánh
        public ActionResult ManagerApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult ManagerApproveGridModel(string search)
        {
            var model = from x in Repository.Search("").Where(p => !p.ApproveFromManager)
                        select new DebtModel
                        {
                            DebtId = x.DebtId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate,
                            Total = x.Total,
                            Paid = x.Paid,
                            PaidLeft = x.Total - x.Paid,
                            MyOfficeName = x.MyOffice.OfficeName
                        };

            var gridModel = new GridModel<DebtModel>
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
            return RedirectToAction("ManagerApprove", "Debt", new { area = "Administrator" });
        }
        #endregion
    }
}
