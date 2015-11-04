using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.ContentManagement
{
    public class NewsCategoryItem
    {
        [Key]
        public int NewsCategoryItemId { get; set; }
        [Required(ErrorMessage = "Tên danh mục là bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Tên danh mục tối đa 128 kí tự.")]
        public virtual string CategoryName { get; set; }
        [StringLength(128, ErrorMessage = "Mô tả tối đa 128 kí tự.")]
        public virtual string CategoryDescription { get; set; }
        [Display(Name = "Level")]
        [Required]
        public int Level { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual NewsCategoryItem Parent { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        public virtual ICollection<NewsCategoryItem> Children { get; set; }
        public bool IsDeleted { get; set; }
    }
}
