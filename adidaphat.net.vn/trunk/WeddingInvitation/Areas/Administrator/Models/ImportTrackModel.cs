using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ImportTrackModel
    {
        public int ImportTrackId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public string ImportUserName { get; set; }
        public bool IsDeleted { get; set; }
        public List<Product> Products { get; set; }
        public int StorageId { get; set; }
        public List<Storage> Storages { get; set; }
        public string ApproveFromManagerName { get; set; }
        public string ApproveFromStorageStaffName { get; set; }
        public int ApproveFromManagerId { get; set; }
        public int ApproveFromStorageStaffId { get; set; }
        public int? Mode { get; set; }
    }
}