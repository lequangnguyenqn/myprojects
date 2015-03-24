
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Catalog
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string ProductName { get; set; }
        [StringLength(128)]
        public string Desciption { get; set; }
        [StringLength(128)]
        public string ProductCode { get; set; }
        public decimal DefaultOriginalPrice { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal DefaultOriginalPrintingIncludeImagePrice { get; set; }
        public decimal DefaultPrintingIncludeImagePrice { get; set; }
        public decimal DefaultPrintingWithoutImagePrice { get; set; }
        public decimal DefaultOriginalPrintingWithoutImagePrice { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public bool Printable { get; set; }
    }
}
