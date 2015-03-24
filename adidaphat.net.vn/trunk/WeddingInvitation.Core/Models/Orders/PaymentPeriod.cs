using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Core.Models.Orders
{
    public class PaymentPeriod
    {
        [Key]
        public int PaymentPeriodId { get; set; }
        public decimal Paid { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        [ForeignKey("CreateUser")]
        public int CreateUserId { get; set; }
        public virtual User CreateUser { get; set; }
        public bool IsDeleted { get; set; }
        public int MyOfficeId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public bool ApproveFromManager { get; set; }
        public int? ApproveFromManagerId { get; set; }
        public DateTime? ApproveFromManagerAt { get; set; }
    }
}
