using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace WeddingInvitation.Core.Models.Settings
{
    public class ExtraFee
    {
        [Key]
        public int ExtraFeeId { get; set; }
        public string ExtraFeeName { get; set; }
        public int AmountFrom { get; set; }
        public int AmountTo { get; set; }
        public decimal Cost { get; set; }
        public bool IsDeleted { get; set; }
    }
}
