
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Catalog;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ImportDetailModel
    {
        public int ImportDetailId { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Nhập số lượng lớn hơn 0")]
        public int Amount { get; set; }
        public string AmountDisplay { get { return String.Format("{0:0,0}", Amount); } }
        public int ImportTrackId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Note { get; set; }
        public List<Category> Categories { get; set; }
        public int[] ProductIds { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}