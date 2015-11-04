using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Users;
using System.Web.Mvc;

namespace WeddingInvitation.Data.Mapping.Users
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        [StringLength(50)]
        [AllowHtml]
        public string RoleName { get; set; }
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
