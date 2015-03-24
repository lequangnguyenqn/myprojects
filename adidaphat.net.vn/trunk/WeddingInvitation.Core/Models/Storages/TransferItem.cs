using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Core.Models.Storages
{
    public class TransferItem
    {
        [Key]
        public int TransferItemId { get; set; }
        [ForeignKey("Transfer")]
        public int TransferId { get; set; }
        public virtual Transfer Transfer { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Amount { get; set; }
        public bool IsDeleted { get; set; }
        public string Note { get; set; }
    }
}
