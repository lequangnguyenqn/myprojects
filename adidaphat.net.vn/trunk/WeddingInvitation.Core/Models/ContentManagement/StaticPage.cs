using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WeddingInvitation.Core.Models.ContentManagement
{
    public class StaticPage
    {
        [Key]
        public int StaticPageId { get; set; }
        public int Type { get; set; }
        [StringLength(128)]
        public string Description { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
    }
}
