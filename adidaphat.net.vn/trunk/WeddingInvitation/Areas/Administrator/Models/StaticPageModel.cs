using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class StaticPageModel
    {
        public int StaticPageId { get; set; }
        [StringLength(128, ErrorMessage = "Mô tả tối đa 128 kí tự.")]
        public string Description { get; set; }
        public string Content { get; set; }
    }
}