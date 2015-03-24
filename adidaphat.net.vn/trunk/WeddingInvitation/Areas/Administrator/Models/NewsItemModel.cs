using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.ContentManagement;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class NewsItemModel
    {
        public int NewsItemId { get; set; }
        [Required(ErrorMessage = "Tiêu đề bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Chiều dài tối đa là 128 kí tự.")]
        public virtual string Title { get; set; }
        [StringLength(512, ErrorMessage = "Chiều dài tối đa là 256 kí tự.")]
        public virtual string Short { get; set; }
        public virtual string Full { get; set; }
        public int NewsCategoryItemId { get; set; }
        public string NewsCategoryItemName { get; set; }
        public DateTime CreateDate { get; set; }
        public List<NewsCategoryItem> Categories { get; set; }
    }
}