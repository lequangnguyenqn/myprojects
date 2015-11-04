using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string ProductName { get; set; }
        [StringLength(128)]
        public string Desciption { get; set; }
        [Required(ErrorMessage = "Mã SP là bắt buộc nhập.")]
        [StringLength(128)]
        public string ProductCode { get; set; }
        public decimal DefaultOriginalPrice { get; set; }
        public decimal DefaultPrice { get; set; }
        public string DefaultPriceDisplay { get { return String.Format("{0:0,0}", DefaultPrice); } }
        public decimal DefaultPrintingIncludeImagePrice { get; set; }
        public string DefaultPrintingIncludeImagePriceDisplay { get { return String.Format("{0:0,0}", DefaultPrintingIncludeImagePrice); } }
        public decimal DefaultOriginalPrintingIncludeImagePrice { get; set; }
        public decimal DefaultPrintingWithoutImagePrice { get; set; }
        public string DefaultPrintingWithoutImagePriceDisplay { get { return String.Format("{0:0,0}", DefaultPrintingWithoutImagePrice); } }
        public decimal DefaultOriginalPrintingWithoutImagePrice { get; set; }
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
        public bool Printable { get; set; }
    }
}