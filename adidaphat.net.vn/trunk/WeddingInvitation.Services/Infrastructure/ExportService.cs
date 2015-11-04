using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Services.Orders;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Users;
using iTextSharp.text.pdf.draw;

namespace WeddingInvitation.Services.Infrastructure
{
    public class ExportService : IExportService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShippingServiceRepository _shippingServiceRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IOrderDeliveryPackageRepository _orderDeliveryPackageRepository;
        private readonly IUserRepository _userRepository;
        public ExportService(IOrderRepository orderRepository,IShippingServiceRepository shippingServiceRepository,
            IProductRepository productRepository, IMyOfficeRepository myOfficeRepository,
            IOrderDeliveryPackageRepository orderDeliveryPackageRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _shippingServiceRepository = shippingServiceRepository;
            _productRepository = productRepository;
            _myOfficeRepository = myOfficeRepository;
            _orderDeliveryPackageRepository = orderDeliveryPackageRepository;
            _userRepository = userRepository;
        }

        public MemoryStream DownloadDeliveryPackage(ReportFilter reportFilter)
        {
            var pdfData = new MemoryStream();
            var document = new Document(PageSize.A4, 35, 35, 25, 50);
            var pdfWriter = PdfWriter.GetInstance(document, pdfData);
            pdfWriter.ViewerPreferences = PdfWriter.PageLayoutOneColumn;

            // Our custom Header and Footer is done using Event Handler
            var pageEventHandler = new TwoColumnHeaderFooter();
            pdfWriter.PageEvent = pageEventHandler;

            // Define the page header
            //pageEventHandler.Title = title;
            //pageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
            //pageEventHandler.HeaderLeft = "Group";
            //pageEventHandler.HeaderRight = "1";
            var filePath = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Content\\Fonts", "times.ttf");
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, true);
            var fontBold = new Font(baseFont, 11, Font.BOLD, BaseColor.BLACK);
            Font defaultFont = new Font(baseFont, 14, Font.NORMAL, BaseColor.BLACK);

            document.Open();

            //Add header
            //var myCompany = new Chunk("VPDD CTY TNHH TM VC TRUNG THÀNH", defaultFont);
            //document.Add(myCompany);
            //document.Add(new Phrase("\n"));
            //var myCompanyAddress = new Chunk("Đ/C: SỐ 17 NGÁCH 43 NGÕ 64 NG.LƯƠNG BẰNG - ĐỐNG ĐA - HN", defaultFont);
            //document.Add(myCompanyAddress);
            //document.Add(new Phrase("\n"));
            //var myCompanyContact = new Chunk("SĐT: 04.3514.9969          FAX: 04.3573.9119", defaultFont);
            //document.Add(myCompanyContact);
            //document.Add(new Paragraph("\r\n"));
            //var myTitle = new Paragraph("DANH SÁCH ĐƠN HÀNG CẦN GIAO", new Font(baseFont, 22, Font.BOLD, BaseColor.BLACK));
            //myTitle.Alignment = Element.ALIGN_CENTER;
            //document.Add(myTitle);
            //document.Add(new Paragraph("\r\n"));

            //From - To
            Rectangle pageSize = document.PageSize;
            //Report date
            //var dateReport = new Chunk(" NGÀY: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), defaultFont);
            //document.Add(dateReport);
            //document.Add(new Paragraph("\r\n"));

            //Add rows
            var no = 1;
            var model = GetDeliveryPackage(reportFilter);
            foreach (var item in model)
            {

                var line1 = new Chunk(string.Format("KHÁCH HÀNG: {0}                 ĐT: {1}",item.CustomerName, item.CustomerPhone), defaultFont);
                document.Add(line1);
                document.Add(new Phrase("\n"));
                var line2 = new Chunk(string.Format("NƠI ĐẾN: {0}", item.CustomerAddress), defaultFont);
                document.Add(line2);
                document.Add(new Phrase("\n"));
                var products = "";
                foreach (var product in item.ProductReportModels)
                {
                    products += string.Format("Mẫu {0}, SL {1};    ",product.CategoryName, product.Amount);
                }
                var line3 = new Chunk(products, defaultFont);
                document.Add(line3);
                document.Add(new Phrase("\n"));
                var line4 = new Chunk(string.Format("Số tiền thu hộ: {0}   (Bằng chữ: {1})", String.Format("{0:0,0}", item.Total), NumberToWords.ChangeCurrencyToWords(item.Total)), defaultFont);
                document.Add(line4);
                document.Add(new Phrase("\n"));
                var line5 = new Chunk(string.Format("Khách hàng đến lấy: {0}                 Chuyển hàng: {1}", item.PaymentType == (int)PaymentTypes.InOffice ? "X" : "", item.PaymentType == (int)PaymentTypes.InShippingPlace ? "X" : ""), defaultFont);
                document.Add(line5);
                document.Add(new Phrase("\n"));
                var line6 = new Chunk(string.Format("Nhà xe: {0}                 ĐT: {1}", item.ShippingServiceName, item.ShippingServicePhone), defaultFont);
                document.Add(line6);
                document.Add(new Phrase("\n"));
                var line7 = new Chunk(string.Format("Đ/c nhà xe: {0}                 Giờ xe chạy: {1}", item.ShippingAddress, (item.StartAt.HasValue ? item.StartAt.Value.ToString("dd/MM/yyyy HH:mm") : "")), defaultFont);
                document.Add(line7);
                document.Add(new Phrase("\n"));
                DottedLineSeparator separator = new DottedLineSeparator();
                //separator.s(59500f / 523f);
                Chunk linebreak = new Chunk(separator);
                document.Add(linebreak);
                document.Add(new Paragraph("\r\n"));
                no++;
            }

            document.Close();

            return pdfData;
        }

        private List<OrderDeliveryPackageReportModel> GetDeliveryPackage(ReportFilter filterModel)
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
            var orderDeliveryPackageIds = (from x in _orderDeliveryPackageRepository.Search("").Where(p =>
                (listOffices.Contains(p.MyOfficeId) || (isGetAllOffices && filterModel.AllowListOffices.Contains(p.MyOfficeId))) &&
                ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                select x.OrderDeliveryPackageId).ToList();
            var orders = _orderRepository.Search("").Where(p => orderDeliveryPackageIds.Contains(p.OrderDeliveryPackageId.Value)).Include(p => p.OrderDetails.Select(t=> t.Product.Category));
            var model = new List<OrderDeliveryPackageReportModel>();
            foreach (var item in orderDeliveryPackageIds)
            {
                var defaultOrder = orders.FirstOrDefault(p => p.OrderDeliveryPackageId == item);
                var defaultShipping = _shippingServiceRepository.GetAll().FirstOrDefault(p => p.ShippingServiceId == defaultOrder.ShippingServiceId);
                var detail = new OrderDeliveryPackageReportModel
                {
                    CustomerName = defaultOrder.Customer.CustomerName,
                    CustomerAddress = defaultOrder.Customer.Address,
                    CustomerPhone = defaultOrder.Customer.PhoneNumber,
                    PaymentType = defaultOrder.PaymentType,
                    ShippingAddress = defaultShipping == null ? "" : defaultShipping.Address,
                    ShippingServiceName = defaultShipping == null ? defaultOrder.Customer.CustomerShortName : defaultShipping.ShippingServiceName,
                    ShippingServicePhone = defaultShipping == null ? "" : defaultShipping.PhoneNumber,
                    Total =orders.Where(p => p.OrderDeliveryPackageId == item).Sum(p=> p.TotalCost),
                    ProductReportModels = new List<ProductReportModel>()
                };
                if (defaultShipping != null)
                {
                    detail.StartAt = defaultShipping.StartAt;
                }
                foreach (var order in orders.Where(p => p.OrderDeliveryPackageId == item))
                {
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        var categoryName = orderDetail.Product.ProductName;
                        var existProduct = detail.ProductReportModels.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
                        if (existProduct != null)
                        {
                            existProduct.Amount += orderDetail.Amount;
                        }
                        else
                        {
                            detail.ProductReportModels.Add(new ProductReportModel { Amount = orderDetail.Amount, CategoryName = categoryName, ProductId = orderDetail.ProductId });
                        }
                    }
                }
                model.Add(detail);
            }
            return model;
        }

        public MemoryStream DownloadOrderReporting(List<Order> orders, string from, string to, int[] offices, int[] products)
        {
            var pdfData = new MemoryStream();
            var document = new Document(PageSize.A4, 35, 35, 25, 50);
            var pdfWriter = PdfWriter.GetInstance(document, pdfData);
            pdfWriter.ViewerPreferences = PdfWriter.PageLayoutOneColumn;

            // Our custom Header and Footer is done using Event Handler
            var pageEventHandler = new TwoColumnHeaderFooter();
            pdfWriter.PageEvent = pageEventHandler;

            // Define the page header
            //pageEventHandler.Title = title;
            //pageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
            //pageEventHandler.HeaderLeft = "Group";
            //pageEventHandler.HeaderRight = "1";
            var filePath = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Content\\Fonts", "times.ttf");
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, true);
            var fontBold = new Font(baseFont, 11, Font.BOLD, BaseColor.BLACK);
            Font defaultFont = new Font(baseFont, 12, Font.NORMAL, BaseColor.BLACK);

            document.Open();

            //Add header
            //var myCompany = new Chunk("VPDD CTY TNHH TM VC TRUNG THÀNH", defaultFont);
            //document.Add(myCompany);
            //document.Add(new Phrase("\n"));
            //var myCompanyAddress = new Chunk("Đ/C: SỐ 17 NGÁCH 43 NGÕ 64 NG.LƯƠNG BẰNG - ĐỐNG ĐA - HN", defaultFont);
            //document.Add(myCompanyAddress);
            //document.Add(new Phrase("\n"));
            //var myCompanyContact = new Chunk("SĐT: 04.3514.9969          FAX: 04.3573.9119", defaultFont);
            //document.Add(myCompanyContact);
            //document.Add(new Paragraph("\r\n"));
            //var myTitle = new Paragraph("DANH SÁCH ĐƠN HÀNG CẦN GIAO", new Font(baseFont, 22, Font.BOLD, BaseColor.BLACK));
            //myTitle.Alignment = Element.ALIGN_CENTER;
            //document.Add(myTitle);
            document.Add(new Paragraph("\r\n"));

            //From - To
            Rectangle pageSize = document.PageSize;

            var headerTable = new PdfPTable(2) { WidthPercentage = 100 };
            headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.SetWidths(new Single[] { 1, 1 });
            if (!string.IsNullOrEmpty(from))
            {
                var headerLeftCell = new PdfPCell(new Phrase(0, "TỪ NGÀY: " + from, defaultFont)) { BorderWidth = 0 };
                headerTable.AddCell(headerLeftCell);
            }
            if (!string.IsNullOrEmpty(to))
            {
                var headerRightCell = new PdfPCell(new Phrase(8, "ĐẾN NGÀY: " + to, defaultFont))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingRight = 5,
                    BorderWidth = 0,
                };
                headerTable.AddCell(headerRightCell);
            }
            document.Add(headerTable);

            //Report date
            if (offices.Count() > 1)
            {
                var officeNames = "";
                var officeEntities = _myOfficeRepository.GetAll().Where(p => products.Contains(p.MyOfficeId)).ToList();
                foreach (var item in officeEntities)
                {
                    officeNames += item.OfficeName + " ;";
                }
                var officeReport = new Chunk(" CHI NHÁNH: " + officeNames, defaultFont);
                document.Add(officeReport);
            }
            
            if (products.Count() > 1 )
            {
                var productNames = "";
                var productEntities = _productRepository.GetAll().Where(p => products.Contains(p.ProductId)).ToList();
                foreach (var item in productEntities)
                {
                    productNames += item.ProductName + " ;";
                }
                var productReport = new Chunk(" SẢN PHẨM: " + productNames, defaultFont);
                document.Add(productReport);
            }
            document.Add(new Paragraph("\r\n"));

            //Add columns
            var tableColumn = new PdfPTable(6) { WidthPercentage = 100 };
            var colWidthPercentages = new Single[] { 7, 9, 31, 23, 15, 15 };
            tableColumn.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tableColumn.SetWidths(colWidthPercentages);
            tableColumn.AddCell(new PdfPCell(new Phrase("STT", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("ID", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("KHÁCH HÀNG", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("CHI NHÁNH", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("NGÀY TẠO", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("THÀNH TIÊN", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            document.Add(tableColumn);

            //Add rows
            var no = 1;
            foreach (var item in orders)
            {
                var tableRow = new PdfPTable(6) { WidthPercentage = 100 };
                tableRow.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tableRow.SetWidths(colWidthPercentages);
                tableRow.AddCell(new PdfPCell(new Phrase(no.ToString(), defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase((item.OrderId + 1000).ToString(), defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(item.Customer.CustomerName, defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(item.MyOffice.OfficeName, defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(item.CreateDate.ToString("dd/MM/yyyy"), defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(String.Format("{0:0,0}", item.TotalCost), defaultFont)));
                document.Add(tableRow);
                no++;
            }

            document.Close();

            return pdfData;
        }
        
        public MemoryStream DownloadPaymentPeriod(IQueryable<Order> model, ReportFilter reportFilter)
        {
            var pdfData = new MemoryStream();
            var document = new Document(PageSize.A4, 35, 35, 25, 50);
            var pdfWriter = PdfWriter.GetInstance(document, pdfData);
            pdfWriter.ViewerPreferences = PdfWriter.PageLayoutOneColumn;

            // Our custom Header and Footer is done using Event Handler
            var pageEventHandler = new TwoColumnHeaderFooter();
            pdfWriter.PageEvent = pageEventHandler;

            // Define the page header
            //pageEventHandler.Title = title;
            //pageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 10, Font.BOLD);
            //pageEventHandler.HeaderLeft = "Group";
            //pageEventHandler.HeaderRight = "1";
            var filePath = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Content\\Fonts", "times.ttf");
            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, true);
            var fontBold = new Font(baseFont, 11, Font.BOLD, BaseColor.BLACK);
            Font defaultFont = new Font(baseFont, 12, Font.NORMAL, BaseColor.BLACK);

            document.Open();

            //Add header
            //var myCompany = new Chunk("VPDD CTY TNHH TM VC TRUNG THÀNH", defaultFont);
            //document.Add(myCompany);
            //document.Add(new Phrase("\n"));
            //var myCompanyAddress = new Chunk("Đ/C: SỐ 17 NGÁCH 43 NGÕ 64 NG.LƯƠNG BẰNG - ĐỐNG ĐA - HN", defaultFont);
            //document.Add(myCompanyAddress);
            //document.Add(new Phrase("\n"));
            //var myCompanyContact = new Chunk("SĐT: 04.3514.9969          FAX: 04.3573.9119", defaultFont);
            //document.Add(myCompanyContact);
            //document.Add(new Paragraph("\r\n"));
            //var myTitle = new Paragraph("DANH SÁCH ĐƠN HÀNG CẦN GIAO", new Font(baseFont, 22, Font.BOLD, BaseColor.BLACK));
            //myTitle.Alignment = Element.ALIGN_CENTER;
            //document.Add(myTitle);
            document.Add(new Paragraph("\r\n"));

            //From - To
            Rectangle pageSize = document.PageSize;

            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            //Filter by date
            if (reportFilter.FromDate.HasValue)
            {
                fromDate = reportFilter.FromDate.Value.Date;
                isAllFromDate = false;
            }
            if (reportFilter.ToDate.HasValue)
            {
                toDate = reportFilter.ToDate.Value.Date.AddDays(1);
                isAllToDate = false;
            }
            //var listOffices = new int[1];
            //var isGetAllListOffices = true;
            ////Filter by deliver to
            //if (reportFilter.ListOffices != null && reportFilter.ListOffices.Count() > 0)
            //{
            //    listOffices = reportFilter.ListOffices;
            //    isGetAllListOffices = false;
            //}
            //var listCustomers = new int[1];
            //var isGetAllListCustomers = true;
            ////Filter by deliver to
            //if (reportFilter.ListCustomers != null && reportFilter.ListCustomers.Count() > 0)
            //{
            //    listCustomers = reportFilter.ListCustomers;
            //    isGetAllListCustomers = false;
            //}

            var headerTable = new PdfPTable(2) { WidthPercentage = 100 };
            headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerTable.SetWidths(new Single[] { 1, 1 });
            if (!isAllFromDate)
            {
                var headerLeftCell = new PdfPCell(new Phrase(0, "TỪ NGÀY: " + fromDate.ToString("dd/MM/yyyy"), defaultFont)) { BorderWidth = 0 };
                headerTable.AddCell(headerLeftCell);
            }
            if (!isAllToDate)
            {
                var headerRightCell = new PdfPCell(new Phrase(8, "ĐẾN NGÀY: " + toDate.ToString("dd/MM/yyyy"), defaultFont))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingRight = 5,
                    BorderWidth = 0,
                };
                headerTable.AddCell(headerRightCell);
            }
            document.Add(headerTable);

            ////Report date
            //if (!isGetAllListOffices)
            //{
            //    var officeNames = "";
            //    var officeEntities = _myOfficeRepository.GetAll().Where(p => listOffices.Contains(p.MyOfficeId)).ToList();
            //    foreach (var item in officeEntities)
            //    {
            //        officeNames += item.OfficeName + " ;";
            //    }
            //    var officeReport = new Chunk(" CHI NHÁNH: " + officeNames, defaultFont);
            //    document.Add(officeReport);
            //}

            //if (products.Count() > 1)
            //{
            //    var productNames = "";
            //    var productEntities = _productRepository.GetAll().Where(p => products.Contains(p.ProductId)).ToList();
            //    foreach (var item in productEntities)
            //    {
            //        productNames += item.ProductName + " ;";
            //    }
            //    var productReport = new Chunk(" SẢN PHẨM: " + productNames, defaultFont);
            //    document.Add(productReport);
            //}
            document.Add(new Paragraph("\r\n"));

            //Add columns
            var tableColumn = new PdfPTable(7) { WidthPercentage = 100 };
            var colWidthPercentages = new Single[] { 7, 9, 31, 24, 15, 7,7 };
            tableColumn.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            tableColumn.SetWidths(colWidthPercentages);
            tableColumn.AddCell(new PdfPCell(new Phrase("STT", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("ID", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("CHI NHÁNH", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("KHÁCH HÀNG", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("SỐ TIỀN", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("CK", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            tableColumn.AddCell(new PdfPCell(new Phrase("THU HỘ", new Font(baseFont, 12, Font.BOLD, BaseColor.BLACK))) { BorderWidth = 2f, Padding = 3, HorizontalAlignment = Element.ALIGN_CENTER });
            document.Add(tableColumn);

            //Add rows
            var no = 1;
            foreach (var item in model.ToList())
            {
                var tableRow = new PdfPTable(7) { WidthPercentage = 100 };
                tableRow.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                tableRow.SetWidths(colWidthPercentages);
                tableRow.AddCell(new PdfPCell(new Phrase(no.ToString(), defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase((item.OrderId + 1000).ToString(), defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(item.MyOffice.OfficeName, defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(item.Customer.CustomerName, defaultFont)));
                tableRow.AddCell(new PdfPCell(new Phrase(String.Format("{0:0,0}", item.TotalCost - item.TotalPaid), defaultFont)));
                var cell1 = new PdfPCell(new Phrase(item.PaymentType == (int)PaymentTypes.Tranfer ? "X" : "", defaultFont));
                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableRow.AddCell(cell1);
                var cell = new PdfPCell(new Phrase(item.PaymentType == (int) PaymentTypes.InShippingPlace ? "X" : "", defaultFont));
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableRow.AddCell(cell);
                document.Add(tableRow);
                no++;
            }

            document.Close();

            return pdfData;
        }
    }
}
