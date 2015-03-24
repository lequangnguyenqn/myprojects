using System;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Core.Models.Orders
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        public DateTime CreateDate { get; set; }
        public string FilePath { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public decimal TotalCost { get; set; }
        public bool PrintIncludeImage { get; set; }
        public bool PrintWithoutImage { get; set; }
        public int Amount { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int StorageId { get; set; }
        public string FilePathBia { get; set; }
        public string FilePathRuot { get; set; }
    }
}
