using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int MyOfficeId { get; set; }
        public string MyOfficeIName { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        //So nha - Ten duong
        public string CustomerAddress { get; set; }
        //Quan Huyen
        public string CellPhoneNumber { get; set; }
        //Chanh xe
        public string CustomerCode { get; set; }
        //Xa Phuong
        public string Fax { get; set; }
        public List<Customer> Customers { get; set; }
        public int? ShippingServiceId { get; set; }
        public string ShippingServiceName { get; set; }
        public string ShippingServicePhone { get; set; }
        public DateTime ShippingServiceAt { get; set; }
        public List<ShippingService> ShippingServices { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int PaymentType { get; set; }
        public int Status { get; set; }
        public int? BackId { get; set; }
        public string StatusDisplay
        {
            get
            {
                if (Stoppping)
                {
                    return "Tạm ngưng";
                }
                else if (Status == (int)OrderStatus.Imported)
                {
                    return "Nhập hệ thống";
                }
                else if (Status == (int)OrderStatus.ReadyInStorage)
                {
                    return "Kho đã duyệt";
                }
                else if (Status == (int)OrderStatus.Printed)
                {
                    return "Đã in";
                }
                else if (Status == (int)OrderStatus.Printing)
                {
                    return "Đang in";
                }
                else if (Status == (int)OrderStatus.BeDelivered && Paid)
                {
                    return "Đã giao & thu hộ";
                }
                else if (Status == (int)OrderStatus.BeDelivered)
                {
                    return "Đã giao";
                }
                else if (Status == (int)OrderStatus.ReadyInStateprovince)
                {
                    return "Đã giao đi tỉnh";
                }
                else if (Paid)
                {
                    return "Đã thu hộ";
                }
                return string.Empty;
            }
        }
        public bool Stoppping { get; set; }
        public string StatusStoppingDisplay
        {
            get
            {
                if (Stoppping)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public bool Printable { get; set; }
        public string PrintableDisplay
        {
            get
            {
                if (Printable)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public string RawProductDisplay
        {
            get
            {
                if (!Printable)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public string StatusPrintingDisplay
        {
            get
            {
                if (Status == (int)OrderStatus.Printing)
                {
                    return "Hoàn tất";
                }
                return "Bắt đầu in";
            }
        }
        public string PaymentTypeDisplay
        {
            get
            {
                if (PaymentType == (int)PaymentTypes.InShippingPlace)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public string TranferDisplay
        {
            get
            {
                if (PaymentType == (int)PaymentTypes.Tranfer)
                {
                    return "Content/Images/checkbox_checked.png";
                }
                return "Content/Images/checkbox_unchecked.png";
            }
        }
        public bool Paid { get; set; }
        public decimal TotalCost { get; set; }
        public string TotalCostDisplay { get { return String.Format("{0:0,0}", TotalCost); } }
        public decimal TotalPaid { get; set; }
        public decimal NeedPaid { get; set; }
        public string NeedPaidDisplay { get { return String.Format("{0:0,0}", TotalCost - TotalPaid); } }
        public decimal ExtraFee { get; set; }
        public string ExtraFeeDisplay { get { return String.Format("{0:0,0}", ExtraFee); } }
        public bool WaitForPrint { get; set; }
        public bool ShowOnTop { get; set; }
        public string PrintUserName { get; set; }
        public string DeliveredUserName { get; set; }
        public List<Product> Products { get; set; }
        public bool HaveShippingFee { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public bool Finished { get; set; }
        public int? Mode { get; set; }
        public DateTime? DeliveredDate { get; set; }
    }

    public enum BackToPage
    {
        UploadFile = 1,
        Printing = 2
    }
}