using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class StateProvinceModel
    {
        public int StateProvinceId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        [StringLength(128, ErrorMessage = "Tên tỉnh thành tối đa là 128 kí tự.")]
        public string StateProvinceName { get; set; }
        [StringLength(24, ErrorMessage = "Mã tối đa là 24 kí tự.")]
        public string StateProvinceCode { get; set; }
        public string RegionName { get; set; }
        public int RegionId { get; set; }
        public int MyOfficeId { get; set; }
        public List<MyOffice> Offices { get; set; }
        public bool IsMain { get; set; }
    }
}