using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Core.Models.Storages
{
    public class ImportDetail
    {
        [Key]
        public int ImportDetailId { get; set; }
        public bool IsDeleted { get; set; }
        public int Amount { get; set; }
        [ForeignKey("ImportTrack")]
        public int ImportTrackId { get; set; }
        public virtual ImportTrack ImportTrack { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Note { get; set; }
    }
}
