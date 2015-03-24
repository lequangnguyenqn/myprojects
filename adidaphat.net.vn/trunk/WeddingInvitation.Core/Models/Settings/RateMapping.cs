using System;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Core.Models.Settings
{
    public class RateMapping
    {
        [Key]
        public int RateMappingId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("MyOffice")]
        public int MyOfficeId { get; set; }
        public virtual MyOffice MyOffice { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal OriginalPrintingIncludeImagePrice { get; set; }
        public decimal PrintingIncludeImagePrice { get; set; }
        public decimal PrintingWithoutImagePrice { get; set; }
        public decimal OriginalPrintingWithoutImagePrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
