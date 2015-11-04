using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Core.Models.Orders
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("MyOffice")]
        public int MyOfficeId { get; set; }
        public virtual MyOffice MyOffice { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int? ShippingServiceId { get; set; }
        public int? RealShippingServiceId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? FileApproval { get; set; }
        public DateTime? PrintDoneAt { get; set; }
        public int? PrintByUserId { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool ShowOnTop { get; set; }
        public int PaymentType { get; set; }
        public int Status { get; set; }
        public bool PaidForDeliveryStaff { get; set; }
        public bool Paid { get; set; }
        public bool WaitForPrint { get; set; }
        public bool Stopping { get; set; }
        public decimal TotalCost { get; set; }
        public decimal ExtraFee { get; set; }
        public decimal TotalPaid { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        [ForeignKey("OrderDeliveryPackage")]
        public int? OrderDeliveryPackageId { get; set; }
        public virtual OrderDeliveryPackage OrderDeliveryPackage { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public int? DeliveredUserId { get; set; }
        public bool ApproveDeliveryFromGeneralDeliveryMan { get; set; }
        public int? ApproveDeliveryFromGeneralDeliveryManId { get; set; }
        public DateTime? ApproveDeliveryFromGeneralDeliveryManAt { get; set; }
        public bool ApproveDeliveryFromStorageStaff { get; set; }
        public int? ApproveDeliveryFromStorageStaffId { get; set; }
        public DateTime? ApproveDeliveryFromStorageStaffAt { get; set; }
        public bool ApproveDeliveryInDay { get; set; }
        public int? ApproveDeliveryInDayBy { get; set; }
        public DateTime? ApproveDeliveryInDayAt { get; set; }

        public bool CancelAHalf { get; set; }
        public bool Cancel { get; set; }
        public bool ApproveCancelFromDelivery { get; set; }
        public int? ApproveCancelFromDeliveryId { get; set; }
        public DateTime? ApproveCancelFromDeliveryAt { get; set; }
        public bool ApproveCancelFromManager { get; set; }
        public int? ApproveCancelFromManagerId { get; set; }
        public DateTime? ApproveCancelFromManagerAt { get; set; }
        public bool ApproveCancelFromPrinter { get; set; }
        public int? ApproveCancelFromPrinterId { get; set; }
        public DateTime? ApproveCancelFromPrinterAt { get; set; }
        public bool ApproveCancelFromStorageStaff { get; set; }
        public int? ApproveCancelFromStorageStaffId { get; set; }
        public DateTime? ApproveCancelFromStorageStaffAt { get; set; }
        public bool ApproveCancelFrombusinessMan { get; set; }
        public int? ApproveCancelFrombusinessManId { get; set; }
        public DateTime? ApproveCancelFrombusinessManAt { get; set; }
    }
}
