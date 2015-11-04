using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using WeddingInvitation.Services.Orders;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Services.Catalog;
using System;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Data;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ReportingController : ControllerBase<ICustomerRepository, Customer>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IExportService _exportService;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IRateMappingRepository _rateMappingRepository;
        private readonly IExtraFeeRepository _extraFeeRepository;
        private readonly ICustomerRepository _customerRepository;
        public ReportingController(IUnitOfWork unitOfWork, ICustomerRepository repository,
            IOrderRepository orderRepository, IMyOfficeRepository myOfficeRepository,
            IProductRepository productRepository, IExportService exportService,
            IExpenseRepository expenseRepository, IRateMappingRepository rateMappingRepository,
            IExtraFeeRepository extraFeeRepository, ICustomerRepository customerRepository)
            : base(repository, unitOfWork)
        {
            _orderRepository = orderRepository;
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _exportService = exportService;
            _expenseRepository = expenseRepository;
            _rateMappingRepository = rateMappingRepository;
            _extraFeeRepository = extraFeeRepository;
            _customerRepository = customerRepository;
        }

        public ActionResult Index()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var products = _productRepository.Search("").ToList();
            var model = new OrderModel
                {
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
            var listOffices = new int[1];
            var isGetAllOffices = true;
            var listProducts = new int[1];
            var isGetAllProduct = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllOffices = false;
            }
            if (filterModel.ListProducts != null && filterModel.ListProducts.Count() > 0)
            {
                listProducts = filterModel.ListProducts;
                isGetAllProduct = false;
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
            var model = _orderRepository.Search("")
                .Where(p => ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)) &&
                (listOffices.Contains(p.MyOfficeId) || isGetAllOffices) &&
                (p.OrderDetails.Select(o => o.ProductId).Any(t => listProducts.Contains(t)) || isGetAllProduct))
                .Include(p => p.MyOffice).Include(p => p.Customer);
            Session["QueryDownloadOrderReporting"] = model;
            Session["OrderReportingFrom"] = isAllFromDate ? "" : fromDate.ToString("dd/MM/yyyy");
            Session["OrderReportingTo"] = isAllToDate ? "" : toDate.ToString("dd/MM/yyyy"); ;
            Session["OfficeReporting"] = listOffices;
            Session["ProductReporting"] = listProducts;
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
                    Paid = x.Paid
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Download()
        {
            var query = (IQueryable<Order>)Session["QueryDownloadOrderReporting"];
            var from = (string)Session["OrderReportingFrom"];
            var to = (string)Session["OrderReportingTo"];
            var offices = (int[])Session["OfficeReporting"];
            var products = (int[])Session["ProductReporting"];
            var fileName = string.Format("Reporting_{0}", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            return PdfResult(_exportService.DownloadOrderReporting(query.ToList(), from, to, offices, products), fileName);
        }

        #region Doanh thu

        public ActionResult Revenue()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices
            };
            return View(model);
        }
        [GridAction]
        public ActionResult OrderGridModel(ReportFilterModel filterModel)
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
            var model = _orderRepository.Search("")
                .Where(p => (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                    ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)) && p.Paid)
                .Include(p => p.MyOffice).Include(p => p.Customer);
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
                    Paid = x.Paid
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [GridAction]
        public ActionResult ExpenseGridModel(ReportFilterModel filterModel)
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
            var model = _expenseRepository.Search("")
                .Where(p => (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) && 
                    ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)));

            var gridModel = new GridModel<ExpenseModel>
            {
                Data = model.Select(x => new ExpenseModel
                {
                    ExpenseId = x.ExpenseId,
                    CreateDate = x.CreateDate,
                    Note = x.Note,
                    Cost = x.Cost,
                    ExpenseName = x.ExpenseName
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult GetRevenue(ReportFilterModel filterModel)
        {
            decimal doanhThu = 0;
            decimal loiNhuan = 0;
            decimal giaVon = 0;
            decimal chiPhi = 0;
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
            var orders = _orderRepository.Search("")
                .Where(p => (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                    ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)) && p.Paid).Include(p => p.OrderDetails).ToList();
            foreach (var item in orders)
            {
                doanhThu += item.TotalCost;
            }
            //giá gốc
            foreach (var item in orders)
            {
                var donHangTheoGiaVon = GetOrgiranalCostForAnOrder(item);
                giaVon += donHangTheoGiaVon;
            }
            //chi phí
            var expenses = _expenseRepository.Search("")
                .Where(p => (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                    ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).ToList();
            foreach (var item in expenses)
            {
                chiPhi += item.Cost;
            }
            loiNhuan = doanhThu - giaVon - chiPhi;

            return Json(new { DoanhThu = String.Format("{0:0,0}", doanhThu), LoiNhuan = String.Format("{0:0,0}", loiNhuan), GiaVon = String.Format("{0:0,0}", giaVon), ChiPhi = String.Format("{0:0,0}", chiPhi) });
        }

        public decimal GetOrgiranalCostForAnOrder(Order order)
        {
            decimal totalCost = 0;
            //Caculate all order details
            foreach (var orderDetailModel in order.OrderDetails)
            {
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
                var rateMapping = _rateMappingRepository.Search("").FirstOrDefault(p => p.MyOfficeId == order.MyOfficeId && p.ProductId == orderDetailModel.ProductId);
                if (rateMapping != null)
                {
                    totalCost += amount * rateMapping.OriginalPrice;
                    if (orderDetailModel.PrintIncludeImage && product.Printable)
                    {
                        totalCost += amount * rateMapping.OriginalPrintingIncludeImagePrice;
                    }
                    if (orderDetailModel.PrintWithoutImage && product.Printable)
                    {
                        totalCost += amount * rateMapping.OriginalPrintingWithoutImagePrice;
                    }
                }
                else
                {
                    totalCost += amount * product.DefaultOriginalPrice;
                    if (orderDetailModel.PrintIncludeImage && product.Printable)
                    {
                        totalCost += amount * product.DefaultOriginalPrintingIncludeImagePrice;
                    }
                    if (orderDetailModel.PrintWithoutImage && product.Printable)
                    {
                        totalCost += amount * product.DefaultOriginalPrintingWithoutImagePrice;
                    }
                }
                //Add extra fee
                if (product.Printable)
                {
                    var extraFee = _extraFeeRepository.GetAll().FirstOrDefault(p => p.AmountFrom < amount && amount < p.AmountTo && p.IsDeleted == false);
                    if (extraFee != null)
                    {
                        totalCost += extraFee.Cost;
                    }
                }
            }
            return totalCost;
        }

        #endregion

        #region Sản phẩm bán ra
        public ActionResult Sale()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var products = _productRepository.Search("").ToList();
            var customers = _customerRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices,
                Customers = customers,
                Products = products
            };
            return View(model);
        }

        [GridAction]
        public ActionResult SaleGridModel(ReportFilterModel filterModel)
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
            var listProducts = new int[1];
            var isGetAllProduct = true;
            if (filterModel.ListProducts != null && filterModel.ListProducts.Count() > 0)
            {
                listProducts = filterModel.ListProducts;
                isGetAllProduct = false;
            }
            var listCustomers = new int[1];
            var isGetAllListCustomers = true;
            //Filter by deliver to
            if (filterModel.ListCustomers != null && filterModel.ListCustomers.Count() > 0)
            {
                listCustomers = filterModel.ListCustomers;
                isGetAllListCustomers = false;
            }
            var orders = (from x in _orderRepository.Search(filterModel.search).Where(p => 
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                (p.OrderDetails.Select(o => o.ProductId).Any(t => listProducts.Contains(t)) || isGetAllProduct) &&
                (listCustomers.Contains(p.CustomerId) || (isGetAllListCustomers)) &&
                ((p.OrderDetails.Any(o => o.PrintIncludeImage) && filterModel.PrintingIncludeImage) || !filterModel.PrintingIncludeImage) &&
                ((p.OrderDetails.Any(o => o.PrintWithoutImage) && filterModel.PrintingWithoutImage) || !filterModel.PrintingWithoutImage) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate))).Include(p => p.OrderDetails.Select(t => t.Product))
                        select new OrderModel
                        {
                            OrderId = x.OrderId,
                            CreateDate = x.CreateDate,
                            MyOfficeId = x.MyOfficeId,
                            CustomerId =x.CustomerId,
                            OrderDetails = x.OrderDetails,
                            MyOfficeIName = x.MyOffice.OfficeName
                        }).ToList();

            var model = new List<ProductInStorageModel>();
            foreach (var item in orders)
            {
                foreach (var item1 in item.OrderDetails)
	            {
                    var product = model.FirstOrDefault(p => p.ProductId == item1.ProductId);
                    if (product != null)
                    {
                        product.Amount += item1.Amount;
                    }
                    else
                    {
                        var newProduct = new ProductInStorageModel
                        {
                            Amount = item1.Amount,
                            ProductId = item1.ProductId,
                            OfficeName = item.MyOfficeIName,
                            ProductName = item1.Product.ProductName
                        };
                        model.Add(newProduct);
                    }
	            }
                
            }
            var gridModel = new GridModel<ProductInStorageModel>
            {
                Data = model.Select(x => new ProductInStorageModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    OfficeName = x.OfficeName,
                    Amount = x.Amount,
                    ProductInStorageId = x.ProductInStorageId

                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        #endregion

    }
}
