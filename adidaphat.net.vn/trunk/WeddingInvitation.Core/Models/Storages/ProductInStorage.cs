using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Core.Models.Storages
{
    public class ProductInStorage
    {
        [Key]
        public int ProductInStorageId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("Storage")]
        public int StorageId { get; set; }
        public virtual Storage Storage { get; set; }
        public int Amount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
