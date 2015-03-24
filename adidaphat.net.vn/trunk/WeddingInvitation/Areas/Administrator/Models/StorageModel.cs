using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class StorageModel
    {
        public int StorageId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string StorageName { get; set; }
        [StringLength(128)]
        public string Desciption { get; set; }
    }
}