using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class TransferModel
    {
        public int TransferId { get; set; }
        public List<Storage> Storages { get; set; }
        public List<Storage> ToStorages { get; set; }
        public int FromStorageId { get; set; }
        public string FromStorageName { get; set; }
        public int ToStorageId { get; set; }
        public string ToStorageName { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string Note { get; set; }
        public string ApproveFromManagerName { get; set; }
        public string ApproveFromStorageStaffName { get; set; }
        public string ApproveFromGeneralDeliveryMan { get; set; }
        public string StorageStaffApproveReceive { get; set; }
        public int ApproveFromManagerId { get; set; }
        public int ApproveFromStorageStaffId { get; set; }
        public int? Mode { get; set; }
    }
}