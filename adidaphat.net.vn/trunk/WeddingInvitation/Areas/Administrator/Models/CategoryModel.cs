using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string CategoryName { get; set; }
        [StringLength(128)]
        public string Desciption { get; set; }
        [Required(ErrorMessage = "Mã là bắt buộc nhập.")]
        [StringLength(128)]
        public string CategoryCode { get; set; }
    }
}