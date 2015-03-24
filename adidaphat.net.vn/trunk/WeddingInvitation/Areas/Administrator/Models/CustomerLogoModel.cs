using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class CustomerLogoModel
    {
        public int CustomerLogoId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128)]
        public string CustomerName { get; set; }
        [StringLength(128)]
        public string LogoUrl { get; set; }
        [StringLength(128)]
        public string HomePageUrl { get; set; }
    }
}