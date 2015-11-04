using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Core.Models.Storages
{
    public class ExportDetail
    {
        [Key]
        public int ExportDetailId { get; set; }
        public bool IsDeleted { get; set; }
        public int Amount { get; set; }
        [ForeignKey("ExportTrack")]
        public int ExportTrackId { get; set; }
        public virtual ExportTrack ExportTrack { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int StorageId { get; set; }
    }
}
