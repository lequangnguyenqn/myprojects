using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Core.Models.Storages
{
    public class ImportTrack
    {
        [Key]
        public int ImportTrackId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        [ForeignKey("CreateUser")]
        public int CreateUserId { get; set; }
        public virtual User CreateUser { get; set; }
        public ICollection<ImportDetail> ImportDetails { get; set; }
        public bool ApproveFromManager { get; set; }
        public int? ApproveFromManagerId { get; set; }
        public DateTime? ApproveFromManagerAt { get; set; }
        public bool ApproveFromStorageStaff { get; set; }
        public int? ApproveFromStorageStaffId { get; set; }
        public DateTime? ApproveFromStorageStaffAt { get; set; }
        public int ToStorageId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
