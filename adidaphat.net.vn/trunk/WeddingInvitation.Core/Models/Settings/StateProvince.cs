using System.ComponentModel.DataAnnotations;

namespace WeddingInvitation.Core.Models.Settings
{
    public class StateProvince
    {
        [Key]
        public int StateProvinceId { get; set; }
        [Required]
        [StringLength(128)]
        public string StateProvinceName { get; set; }
        [StringLength(24)]
        public string StateProvinceCode { get; set; }
        public bool IsDeleted { get; set; }
        public int MyOfficeId { get; set; }
        public bool IsMain { get; set; }
    }
}
