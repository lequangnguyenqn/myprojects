using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class RateMappingModel
    {
        public int RateMappingId { get; set; }
        public int ProductId { get; set; }
        public string ProductSelected { get; set; }
        public List<Product> Products { get; set; }
        public string ProductName { get; set; }
        public int MyOfficeId { get; set; }
        public string MyOfficeName { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public decimal Price { get; set; }
        public string PriceDisplay { get { return String.Format("{0:0,0}", Price); } }
        public decimal PrintingIncludeImagePrice { get; set; }
        public decimal PrintingWithoutImagePrice { get; set; }
        public string PrintingIncludeImagePriceDisplay { get { return String.Format("{0:0,0}", PrintingIncludeImagePrice); } }
        public string PrintingWithoutImagePriceDisplay { get { return String.Format("{0:0,0}", PrintingIncludeImagePrice); } }
        
    }
}