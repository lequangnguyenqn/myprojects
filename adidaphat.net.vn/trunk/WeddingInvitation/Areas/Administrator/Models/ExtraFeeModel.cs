using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ExtraFeeModel
    {
        public int ExtraFeeId { get; set; }
        [Required(ErrorMessage = "Tên là bắt buộc nhập.")]
        public string ExtraFeeName { get; set; }
        public int AmountFrom { get; set; }
        public string AmountFromDisplay { get { return String.Format("{0:0,0}", AmountFrom); } }
        public int AmountTo { get; set; }
        public string AmountToDisplay { get { return String.Format("{0:0,0}", AmountTo); } }
        public decimal Cost { get; set; }
        public string CostDisplay { get { return String.Format("{0:0,0}", Cost); } }
    }
}