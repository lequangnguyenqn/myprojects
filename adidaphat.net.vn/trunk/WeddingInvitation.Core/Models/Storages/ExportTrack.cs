using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Orders;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Core.Models.Storages
{
    public class ExportTrack
    {
        [Key]
        public int ExportTrackId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        [ForeignKey("ReceivedUser")]
        public int ReceivedUserId { get; set; }
        public virtual User ReceivedUser { get; set; }
        public bool IsDeleted { get; set; }
        public int MyOfficeId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public ICollection<ExportDetail> ExportDetails { get; set; }
    }
}
