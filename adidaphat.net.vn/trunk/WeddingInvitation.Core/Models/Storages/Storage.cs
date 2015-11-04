using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Core.Models.Storages
{
    public class Storage
    {
        [Key]
        public int StorageId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string StorageName { get; set; }
        [StringLength(128)]
        public string Desciption { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
