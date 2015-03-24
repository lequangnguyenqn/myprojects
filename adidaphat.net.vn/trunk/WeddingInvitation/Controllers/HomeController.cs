using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Services;
using WeddingInvitation.Models;
using WeddingInvitation.Services;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.ContentManagement;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Orders;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;
using System.Data.Entity;

namespace WeddingInvitation.Controllers
{
    
    public class HomeController : ControllerBase<IUserRepository, User>
    {
        private readonly IMailServerService _mailServerService;
        private readonly IStaticPageRepository _staticPageRepository;
        private readonly INewsItemRepository _newsItemRepository;
        private readonly ICustomerLogoRepository _customerLogoRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IShippingServiceRepository _shippingServiceRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductInStorageRepository _productInStorageRepository;
        private readonly IRateMappingRepository _rateMappingRepository;
        private readonly IExtraFeeRepository _extraFeeRepository;
        private readonly ICategoryRepository _categoryRepository;

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

        public HomeController(IUnitOfWork unitOfWork, IMailServerService mailServerService,
                              IUserRepository repository, IStaticPageRepository staticPageRepository,
                              INewsItemRepository newsItemRepository, ICustomerLogoRepository customerLogoRepository
            , IOrderRepository orderRepository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository,
            ICustomerRepository customerRepository, IShippingServiceRepository shippingServiceRepository,
            IStorageRepository storageRepository,IOrderDetailRepository orderDetailRepository, 
            IProductInStorageRepository productInStorageRepository, IRateMappingRepository rateMappingRepository,
            IExtraFeeRepository extraFeeRepository, ICategoryRepository categoryRepository)
            : base(repository, unitOfWork)
        {
            _mailServerService = mailServerService;
            _staticPageRepository = staticPageRepository;
            _newsItemRepository = newsItemRepository;
            _customerLogoRepository = customerLogoRepository;
            _orderRepository = orderRepository;
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _shippingServiceRepository = shippingServiceRepository;
            _storageRepository = storageRepository;
            _orderDetailRepository = orderDetailRepository;
            _productInStorageRepository = productInStorageRepository;
            _rateMappingRepository = rateMappingRepository;
            _extraFeeRepository = extraFeeRepository;
            _categoryRepository = categoryRepository;
        }
        //
        // GET: /Home/
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "Administrator" });
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult Contact()
        {
            const int pageType = (int)EnumStaticPages.Contact;
            var model = _staticPageRepository.GetAll().FirstOrDefault(p => p.Type == pageType);
            model.Content = HttpUtility.HtmlDecode(model.Content);
            return View(model);
        }

        public ActionResult About()
        {
            const int pageType = (int)EnumStaticPages.About;
            var model = _staticPageRepository.GetAll().FirstOrDefault(p => p.Type == pageType);
            model.Content = HttpUtility.HtmlDecode(model.Content);
            return View(model);
        }

        [HttpPost]
        public ActionResult HelpAndSupport(HelpAndSupportModel model)
        {
            //Send email
            var emailSupport = ConfigurationManager.AppSettings["EmailSupport"];
            string dataPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/EmailTemplates");
            /*
            //Send Mail To User
            // 0: SupportRef 
            // 1: Url Host
            string mess = "";
            IniFile ini = new IniFile(dataPath + "\\SendMailConfirmSupportUser.ini");
            var title = ini.GetStringValue("Title");
            var subject = ini.GetStringValue("Subject");
            var content = ini.GetStringValue("Content");
            object[] obj = new object[5];
            //obj[0] = entity.HelpSupportId.ToString();
            obj[1] = WebHelpers.GetUrlHost();
            _mailServerService.SendByServer(model.Email, subject, string.Format(content, obj), null, ref mess);

             */

            //Send Mail Supper Admin
            // 0: Title 
            // 1: Date sumitted 
            // 2: Name 
            // 3: Email Address 
            // 4: Message 
            var messAdmin = "";
            var iniAdmin = new IniFile(dataPath + "\\SendMailSupportToSupperAdmin.ini");
            //var titleAdmin = iniAdmin.GetStringValue("Title");
            var subjectAdmin = iniAdmin.GetStringValue("Subject");
            var contentAdmin = iniAdmin.GetStringValue("Content");
            var objAdmin = new object[5];
            objAdmin[0] = model.Subject;
            objAdmin[1] = DateTime.Now.ToString("dd/MM/yyyy");
            objAdmin[2] = model.Name;
            objAdmin[3] = model.Email;
            objAdmin[4] = model.Message;
            _mailServerService.SendByServer(emailSupport, subjectAdmin, string.Format(contentAdmin, objAdmin), null, ref messAdmin);

            TempData["MessageSuccess"] = "The message has been sent.";
            return RedirectToAction("Contact");
        }

        #region Order
        public ActionResult CreateOrder(bool? blank, int? c, bool? f, bool? d)
        {
            if (!blank.HasValue || blank.Value)
            {
                OrderDetails = new List<OrderDetailModel>();
                OrderSession = new OrderModel();
                OrderDetailSession = new OrderDetailModel();
            }
            if (d.HasValue)
            {
                OrderDetailSession = new OrderDetailModel();
            }
            var offices = _myOfficeRepository.Search("").Where(p=> !p.IsRetailCustomer).ToList();
            var shippingServices = new List<ShippingService>();
            var model = new OrderModel
            {
                MyOffices = offices,
                ShippingServices = shippingServices,
                PaymentType = OrderSession.PaymentType > 0 ? OrderSession.PaymentType : (int)PaymentTypes.InShippingPlace,
                CustomerId = OrderSession.CustomerId,
                CustomerName = OrderSession.CustomerName,
                MyOfficeId = OrderSession.MyOfficeId,
                Note = OrderSession.Note,
                ShippingServiceId = OrderSession.ShippingServiceId,
                ShowOnTop = OrderSession.ShowOnTop,
                WaitForPrint = OrderSession.WaitForPrint,
                HaveShippingFee = OrderSession.HaveShippingFee,
                CustomerPhone = OrderSession.CustomerPhone
            };
            if (c.HasValue)
            {
                var customer = _customerRepository.GetById(c.Value);
                model.CustomerAddress = customer.Address;
                model.CustomerName = customer.CustomerName;
                model.CustomerPhone = customer.PhoneNumber;
                model.CellPhoneNumber = customer.CellPhoneNumber;
                model.CustomerCode = customer.CustomerCode;
                model.Fax = customer.Fax;
                model.CustomerId = customer.CustomerId;
                model.MyOfficeId = customer.MyOfficeId;
                var lastOrder = _orderRepository.Search("").FirstOrDefault(p => p.CustomerId == c.Value);
                if (lastOrder!=null)
                {
                    model.MyOfficeId = lastOrder.MyOfficeId;
                    model.ShippingServiceId = lastOrder.ShippingServiceId;
                    model.PaymentType = lastOrder.PaymentType;
                }
            }
            if (f.HasValue)
            {
                model.Finished = true;
            }
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult SaveOrder(OrderModel model)
        {
            if (model.OrderId <= 0) //Create News
            {
                SaveOrderToSession(model);
                if (!OrderDetails.Any())
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa có sản phẩm nào."));
                    return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                }
                if (model.CustomerId <= 0)
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa có khách hàng."));
                    return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                }
                if (model.PaymentType != (int)PaymentTypes.InOffice && string.IsNullOrEmpty(model.ShippingServiceName))
                {
                    SetErrorNotification(string.Format("Đơn hàng chưa chọn chành xe."));
                    return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                }
                //if (OrderDetails.Any(p => !p.PrintIncludeImage && !p.PrintWithoutImage))
                //{
                //    SetErrorNotification(string.Format("Bạn phải chọn in hình hoặc in không hình cho sản phẩm."));
                //    return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                //}
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
                    WaitForPrint = model.WaitForPrint,
                    ShowOnTop= model.ShowOnTop,
                    OrderDetails = new List<OrderDetail>()
                };
                SetCustomerToOrder(model, entity);
                var myOffice = _myOfficeRepository.GetById(model.MyOfficeId);
                AddOrderDetailToOrder(entity, OrderDetails, myOffice.StorageId);
                //Update product in storage
                foreach (var orderDetailModel in OrderDetails)
                {
                    var productInStorage = _productInStorageRepository.Search("").FirstOrDefault(p => p.ProductId == orderDetailModel.ProductId && p.StorageId == myOffice.StorageId);
                    if (productInStorage != null && productInStorage.Amount > orderDetailModel.Amount)
                    {
                        productInStorage.Amount -= orderDetailModel.Amount;
                    }
                    else
                    {
                        var product = _productRepository.GetById(orderDetailModel.ProductId);
                        SetErrorNotification(string.Format("Sản phẩm {0} đã hết trong kho.", product.ProductName));
                        return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                    }
                }
                using (UnitOfWork)
                {
                    _orderRepository.Insert(entity);
                }
                var customer = _customerRepository.GetById(entity.CustomerId);
                SendEmailConfirm(entity, new Customer { CustomerCode = customer.CustomerCode, PhoneNumber = customer.PhoneNumber, CustomerName = customer.CustomerName }, MakeOrderDetailHtml(entity));
                OrderDetails = new List<OrderDetailModel>();
                OrderSession = new OrderModel();
                //this.SetSuccessNotification(string.Format("{0} đã được tạo thành công.", "Đơn hàng"));
                return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false, f = true });
            }
            OrderDetails = new List<OrderDetailModel>();
            OrderSession = new OrderModel();
            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được tạo thành công. Thông tin đặt hàng đã được gửi đến quý khách, vui lòng xem email để biết thêm chi tiết!", "Đơn hàng"));
            return RedirectToAction("CreateOrder", "Home", new { area = "" });
        }

        private void SendEmailConfirm(Order order, Customer customer, string orderDetailString)
        {
            //Send email
            var emailSupport = ConfigurationManager.AppSettings["EmailConfirmOrder"];
            string dataPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/EmailTemplates");
            /*
            //Send Mail To User
            // 0: SupportRef 
            // 1: Url Host
            string mess = "";
            IniFile ini = new IniFile(dataPath + "\\SendMailConfirmSupportUser.ini");
            var title = ini.GetStringValue("Title");
            var subject = ini.GetStringValue("Subject");
            var content = ini.GetStringValue("Content");
            object[] obj = new object[5];
            //obj[0] = entity.HelpSupportId.ToString();
            obj[1] = WebHelpers.GetUrlHost();
            _mailServerService.SendByServer(model.Email, subject, string.Format(content, obj), null, ref mess);

             */

            //Send Mail Supper Admin
            // 0: Title 
            // 1: Date sumitted 
            // 2: Name 
            // 3: Email Address 
            // 4: Message 
            var messAdmin = "";
            var iniAdmin = new IniFile(dataPath + "\\SendMailConfirmOrder.ini");
            //var titleAdmin = iniAdmin.GetStringValue("Title");
            var subjectAdmin = string.Format(iniAdmin.GetStringValue("Subject"),order.OrderId + 1000);
            var contentAdmin = iniAdmin.GetStringValue("Content");
            var objAdmin = new object[6];
            objAdmin[0] = customer.PhoneNumber;
            objAdmin[1] = DateTime.Now.ToString("dd/MM/yyyy");
            objAdmin[2] = customer.CustomerName;
            objAdmin[3] = customer.PhoneNumber;
            objAdmin[4] = string.IsNullOrEmpty(order.Note) ? string.Empty : string.Format("<p><span style=\"font-weight:bold;\">Ghi chú: </span> <span style=\"color:red;font-weight:bold;\">{0}</span></p>",order.Note);
            objAdmin[5] = orderDetailString;
            List<string> files = new List<string>();
            foreach (var orderDetail in order.OrderDetails)
            {
                var product = _productRepository.GetById(orderDetail.ProductId);
                if (!string.IsNullOrEmpty(orderDetail.FilePathBia) && product.Printable)
                {
                    files.Add(Server.MapPath(orderDetail.FilePathBia));
                }
                if (!string.IsNullOrEmpty(orderDetail.FilePathRuot) && product.Printable)
                {
                    files.Add(Server.MapPath(orderDetail.FilePathRuot));
                }
            }
            _mailServerService.SendByServer(customer.CustomerCode, subjectAdmin,string.Format(contentAdmin, objAdmin), files.ToArray(), ref messAdmin);
        }

        private string MakeOrderDetailHtml(Order order)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table border=\"0\" style=\"width:100%;\">");

            sb.AppendLine(string.Format("<tr style=\"background-color:{0};text-align:center;\">", "rgb(231,231,231)"));
            sb.AppendLine("<th>STT</th>");
            sb.AppendLine("<th>Tên</th>");
            sb.AppendLine("<th>In không hình</th>");
            sb.AppendLine("<th>In có hình cô dâu, chú rể</th>");
            sb.AppendLine("<th>Số lượng</th>");
            sb.AppendLine("<th>Đơn giá</th>");
            sb.AppendLine("<th>Phụ thu</th>");
            sb.AppendLine("<th>Thành tiền</th>");
            sb.AppendLine("</tr>");
            var entity = _orderRepository.GetAll().Where(p => p.OrderId == order.OrderId).Include(p => p.OrderDetails.Select(o =>o.Product.Category)).FirstOrDefault();
            var listOrderDetails = entity.OrderDetails.Select(p => new OrderDetailModel
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
                FilePathRuot = p.FilePathRuot
            }).ToList();
            FillCostForOrderDetail(listOrderDetails);
            var model = new List<OrderDetailModel>();
            foreach (var item in OrderDetails)
            {
                if (!model.Any(p => p.CategoryId == item.CategoryId))
                {
                    var orderDetails = OrderDetails.Where(p => p.CategoryId == item.CategoryId).ToList();
                    foreach (var item1 in orderDetails)
                    {
                        if (!model.Any(p => p.GroupId == item1.GroupId))
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
            var no = 1;
            foreach (var item in model)
            {
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: center;\">{0}</td>", no));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: left;\">{0}</td>", item.ProductName));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: center;\">{0}</td>", item.PrintWithoutImage ? "X" : ""));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: center;\">{0}</td>", item.PrintIncludeImage ? "X" : ""));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", item.AmountDisplayToCustomer));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", item.PriceDisplay));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", item.ExtraFeeDisplay));
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", item.TotalCostDisplay));
                sb.AppendLine(string.Format("<tr style=\"background-color: {0};text-align: center;\">", "rgb(231,231,231)"));
                sb.AppendLine("</tr>");
                no++;
            }
            //Phí dịch vụ
            if (order.ExtraFee > 0)
            {
                sb.AppendLine(
                    string.Format(
                        "<tr style=\"text-align:right;\"><td colspan=\"5\">&nbsp;</td><td colspan=\"2\" style=\"background-color:rgb(231,231,231);padding:0.6em 0.4 em;\"><strong>{0}</strong></td> <td style=\"background-color:rgb(231,231,231);padding:0.6em 0.4 em;\"><strong>{1}</strong></td></tr>",
                        "Phụ thu", String.Format("{0:0,0}", order.ExtraFee)));
            }
            //total
            sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td colspan=\"5\">&nbsp;</td><td colspan=\"2\" style=\"background-color:rgb(231,231,231);padding:0.6em 0.4 em;\"><strong>{0}</strong></td> <td style=\"background-color: rgb(231,231,231);padding:0.6em 0.4 em;\"><strong>{1}</strong></td></tr>", "Thành tiền", String.Format("{0:0,0}", order.TotalCost)));
            sb.AppendLine("</table>");
            var result = sb.ToString();
            return result;
        }

        private void FillCostForOrderDetail(List<OrderDetailModel> orderDetails)
        {
            //Caculate all order details
            foreach (var orderDetailModel in orderDetails)
            {
                decimal defaultPrice;
                decimal totalCost;
                decimal extraFeeCost;
                CaculateCostForAnProduct(orderDetailModel, out defaultPrice, out totalCost, out extraFeeCost);
                orderDetailModel.ExtraFee = extraFeeCost;
                orderDetailModel.TotalCost = totalCost;
                orderDetailModel.Price = defaultPrice;
            }
        }

        private void CaculateCostForAnProduct(OrderDetailModel orderDetailModel, out decimal defaultPrice, out decimal totalCost, out decimal extraFeeCost)
        {
            defaultPrice = 0;
            totalCost = 0;
            extraFeeCost = 0;
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
        }

        private void SaveOrderToSession(OrderModel model)
        {
            OrderSession.CustomerPhone = model.CustomerPhone;
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

        private void AddOrderDetailToOrder(Order entity, IEnumerable<OrderDetailModel> listBeAdded, int storageId)
        {
            foreach (var orderDetailModel in listBeAdded)
            {
                entity.OrderDetails.Add(new OrderDetail
                {
                    Amount = orderDetailModel.Amount,
                    CreateDate = DateTime.Now,
                    StorageId = storageId,
                    ProductId = orderDetailModel.ProductId,
                    TotalCost = orderDetailModel.TotalCost,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = orderDetailModel.Note,
                    FilePath = orderDetailModel.FilePath,
                    IsDeleted = false,
                    UpdateUserId = orderDetailModel.GroupId,
                    PrintIncludeImage = orderDetailModel.PrintIncludeImage,
                    PrintWithoutImage = orderDetailModel.PrintWithoutImage,
                    FilePathBia = orderDetailModel.FilePathBia,
                    FilePathRuot = orderDetailModel.FilePathRuot,
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
                if (customer.CustomerShortName != model.ShippingServiceName || customer.MyOfficeId!=model.MyOfficeId)
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

        public ActionResult DeleteOrderDetail(int id)
        {
            try
            {
                var model = OrderDetails.Where(p => p.OrderDetailId == id).FirstOrDefault();
                if (model.PriceByCategory > 0)
                {
                    OrderDetails = OrderDetails.Where(p => p.GroupId != model.GroupId).ToList();
                }
                else
                {
                    OrderDetails = OrderDetails.Where(p => p.OrderDetailId != id).ToList();
                }
                SetSuccessNotification("Sản phẩm đã được bỏ khỏi đơn hàng.");
                return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
            }
            catch
            {
                this.SetErrorNotification("Sản phẩm này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
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
            return Json(new { TotalCost = totalCost, ExtraFee = extraFeeCost });
        }

        [GridAction]
        public ActionResult GridModelOrderDetail(int? orderId)
        {
            foreach (var item in OrderDetails)
            {
                if (item.Printable)
                {
                    item.DownloadBiaActionLink = Url.Action("DownloadFileBia", "Home", new { area = "" });
                    item.DownloadRuotActionLink = Url.Action("DownloadFileRuot", "Home", new { area = "" });
                    item.DownloadImagePath = Url.Content("~/Content/Images/download_16_16.png");
                    item.UploadImagePath = Url.Content("~/Content/Images/upload_16_16.png");
                }
            }
            var model = new List<OrderDetailModel>();
            foreach (var item in OrderDetails)
            {
                if (!model.Any(p => p.CategoryId == item.CategoryId))
                {
                    var orderDetails = OrderDetails.Where(p => p.CategoryId == item.CategoryId).ToList();
                    foreach (var item1 in orderDetails)
                    {
                        if (!model.Any(p => p.GroupId == item1.GroupId))
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
            var gridModel = new GridModel<OrderDetailModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult CreateOrderDetail()
        {
            var categories = _categoryRepository.Search("").ToList();
            var model = new OrderDetailModel
            {
                Categories = categories, 
                OrderId = 0,
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
                List<Product> products;
                if (model.ProductIds.FirstOrDefault() == -1 || model.ProductIds.FirstOrDefault() == -2)
                {
                    products = _productRepository.Search("").Where(p => p.CategoryId == model.CategoryId).ToList();
                }
                else
                {
                    products = _productRepository.Search("").Where(p => model.ProductIds.Contains(p.ProductId)).ToList();
                }
                //if (!model.PrintIncludeImage && !model.PrintWithoutImage && products.Any(p => p.Printable) && model.ProductIds.FirstOrDefault() != -1)
                //{
                //    SetErrorNotification(string.Format("Bạn phải chọn in hình hoặc in không hình cho sản phẩm."));
                //    return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                //}
                //if (string.IsNullOrEmpty(model.FilePathBia) && string.IsNullOrEmpty(model.FilePathRuot) && products.Any(p => p.Printable) && model.ProductIds.FirstOrDefault() != -1)
                //{
                //    SetErrorNotification(string.Format("Bạn chưa up file hình cho sản phẩm."));
                //    return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });
                //}
                if (model.Amount > 500)
                {
                    model.Amount += 5;
                }
                else
                {
                    model.Amount += 3;
                }
                Random rnd = new Random();
                int groupId = 0;
                if (OrderDetails.Any())
                {
                    groupId = OrderDetails.Max(p => p.GroupId);
                }
                foreach (var item in products)
                {
                    int orderDetailId = rnd.Next(-9999, -1);
                    OrderDetails.Add(new OrderDetailModel
                    {
                        Amount = model.Amount,
                        FilePathBia = model.FilePathBia,
                        FilePathRuot = model.FilePathRuot,
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
                        CategoryId= model.CategoryId,
                        GroupId = groupId + 1
                    });
                }
                FillCostForOrderDetail(OrderDetails);
            }
            OrderDetailSession = new OrderDetailModel();
            this.SetSuccessNotification(string.Format("{0} đã được thêm vào đơn hàng thành công.", "Sản phẩm"));
            return RedirectToAction("CreateOrder", "Home", new { area = "", blank = false });

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
            OrderDetailSession.ProductId = model.ProductIds.Any() ? model.ProductIds.FirstOrDefault() : 0;
        }

        public FileResult DownloadFileBia(int id)
        {
            string filePath;
            var orderDetail = OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);
            if (orderDetail == null)
            {
                filePath = _orderDetailRepository.GetById(id).FilePathBia;
            }
            else
            {
                filePath = orderDetail.FilePathBia;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(filePath));
            string fileName = Path.GetFileName(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadFileRuot(int id)
        {
            string filePath;
            var orderDetail = OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);
            if (orderDetail == null)
            {
                filePath = _orderDetailRepository.GetById(id).FilePathRuot;
            }
            else
            {
                filePath = orderDetail.FilePathRuot;
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(filePath));
            string fileName = Path.GetFileName(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult GetProducts(int? categoryId)
        {
            var products = _productRepository.Search("").Where(p => p.CategoryId == categoryId).Select(p => new ProductOption { Id = p.ProductId, Name = p.ProductName, Printable = p.Printable, RoughDraft = false }).ToList();
            
            var product = products.FirstOrDefault(p => p.Id == OrderDetailSession.ProductId);
            if (product != null)
            {
                product.Checked = "checked=checked";
                if (categoryId > 0)
                {
                    products.Add(new ProductOption { Id = -1, Name = "Bộ", Printable = true, RoughDraft = false });
                    products.Add(new ProductOption { Id = -2, Name = "Phôi", Printable = false, RoughDraft = true });
                }
            }
            else
            {
                if (categoryId > 0)
                {
                    products.Add(new ProductOption { Id = -1, Name = "Bộ", Printable = true, Checked = "checked=checked", RoughDraft = false });
                    products.Add(new ProductOption { Id = -2, Name = "Phôi", Printable = false, RoughDraft = true });
                }
            }
            return Json(products);
        }

        public ActionResult CheckProductAmount(int? categoryId, int amount)
        {
            var products = _productRepository.Search("").Where(p => p.CategoryId == categoryId).Select(p => p.ProductId).ToList();
            if (products != null && products.Any())
            {
                return Json(_productInStorageRepository.GetAll().Where(p => products.Contains(p.ProductId) && p.Amount < amount).Any());
            }
            return Json(false);
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

        public JsonResult GetCustomerNote(int? customerId)
        {
            var entity = _customerRepository.GetById(customerId);
            if (entity != null)
            {
                return Json(entity.Note, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerInfo(string phoneNumber)
        {
            var entity = _customerRepository.Search("").FirstOrDefault(p => p.PhoneNumber == phoneNumber);
            if (entity != null)
            {
                OrderSession.CustomerPhone = phoneNumber;
                var paymentType = 0;
                var order = _orderRepository.Search("").Where(p => p.CustomerId == entity.CustomerId).OrderByDescending(p => p.OrderId).FirstOrDefault();
                if(order!=null)
                {
                    paymentType=order.PaymentType;
                }
                return Json(new { entity.CustomerId, ShippingServiceName = entity.CustomerShortName, PaymentType = paymentType, MyOfficeId = entity.MyOfficeId }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateCustomer()
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var model = new CustomerModel { MyOffices = offices };
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult SaveCustomer(CustomerModel customerModel)
        {
            //Check existed
            var modelExisted = _customerRepository.GetAll().FirstOrDefault(p => p.PhoneNumber == customerModel.PhoneNumber);
            if ((modelExisted != null && modelExisted.CustomerId != customerModel.CustomerId) || (modelExisted != null && customerModel.CustomerId <= 0))
            {
                SetErrorNotification("Số điện thoại này đã tồn tại trong hệ thống.");
                return RedirectToAction("CreateOrder", new { area = "", blank = false });
            }
            if (customerModel.CustomerId <= 0) //Create News
            {
                var customer = new Customer
                {
                    IsDeleted = false,
                    Address = customerModel.Address,
                    CustomerName = customerModel.CustomerName,
                    Fax = customerModel.Fax,
                    PhoneNumber = customerModel.PhoneNumber,
                    CustomerCode = customerModel.CustomerCode,
                    DiscountPercent = customerModel.DiscountPercent,
                    UseSpecialRateTable = customerModel.UseSpecialRateTable,
                    CustomerShortName = customerModel.CustomerShortName,
                    MyOfficeId = customerModel.MyOfficeId,
                    Note = customerModel.Note,
                    CellPhoneNumber = customerModel.CellPhoneNumber,
                };
                using (UnitOfWork)
                {
                    _customerRepository.Insert(customer);
                }
                SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Khách hàng"));
                return RedirectToAction("CreateOrder", new { area = "", c = customer.CustomerId, blank = false });
            }
            return RedirectToAction("CreateOrder", new { area = "", blank = false });
        }

        #endregion
    }

    public class ProductOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Printable { get; set; }
        public string Checked { get; set; }
        public bool RoughDraft { get; set; }
    }
}
