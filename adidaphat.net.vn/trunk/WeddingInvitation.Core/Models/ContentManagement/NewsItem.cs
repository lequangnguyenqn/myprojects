using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WeddingInvitation.Core.Models.ContentManagement
{
    public class NewsItem
    {
        [Key]
        public int NewsItemId { get; set; }
        [StringLength(128)]
        public virtual string Title { get; set; }
        [StringLength(128)]
        public virtual string ImageUrl { get; set; }
        [StringLength(512)]
        public virtual string Short { get; set; }
        [AllowHtml]
        public virtual string Full { get; set; }
        public virtual bool Published { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        [ForeignKey("Category")]
        public int NewsCategoryItemId { get; set; }
        public virtual NewsCategoryItem Category{ get; set; }
        public bool IsDeleted { get; set; }
    }
}
