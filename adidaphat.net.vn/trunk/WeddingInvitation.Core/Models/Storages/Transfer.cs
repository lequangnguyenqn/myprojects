using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Storages
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }
        public ICollection<TransferItem> TransferItems { get; set; }
        public int ToStorageId { get; set; }
        public int FromStorageId { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public string Note { get; set; }
        public bool ApproveFromManager { get; set; }
        public int? ApproveFromManagerId { get; set; }
        public DateTime? ApproveFromManagerAt { get; set; }
        public bool ApproveFromStorageStaff { get; set; }
        public int? ApproveFromStorageStaffId { get; set; }
        public DateTime? ApproveFromStorageStaffAt { get; set; }
        public bool ApproveFromGeneralDeliveryMan { get; set; }
        public int? ApproveFromGeneralDeliveryManId { get; set; }
        public DateTime? ApproveFromGeneralDeliveryManAt { get; set; }
        public bool ApproveFromReceiveStorageStaff { get; set; }
        public int? ApproveFromReceiveStorageStaffId { get; set; }
        public DateTime? ApproveFromReceiveStorageStaffAt { get; set; }

    }
}
