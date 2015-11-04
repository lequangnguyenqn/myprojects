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
    public class OrderDeliveryPackageController : ControllerBase<IOrderDeliveryPackageRepository, OrderDeliveryPackage>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderDeliveryPackageController(IUnitOfWork unitOfWork, IOrderDeliveryPackageRepository repository,
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
                        select new OrderDeliveryPackageModel
                        {
                            OrderDeliveryPackageId = x.OrderDeliveryPackageId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            ShippingFee = x.ShippingFee,
                            MyOfficeName = x.MyOffice.OfficeName
                        };
            double total = 0;
            try
            {
                total = Convert.ToDouble(model.Sum(p => p.ShippingFee));
            }
            catch
            {
            }
            Session["TotalShippingFee"] = String.Format("{0:0,0}", total);
            var gridModel = new GridModel<OrderDeliveryPackageModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        public ActionResult GetTotalShippingFee()
        {
            return Json(Session["TotalShippingFee"]);
        }
    }
}
