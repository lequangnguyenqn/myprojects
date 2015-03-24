
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Catalog
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string CategoryName { get; set; }
        [StringLength(128)]
        public string Desciption { get; set; }
        [StringLength(128)]
        public string CategoryCode { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
