using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Core.Models.Orders
{
    public class Debt
    {
        [Key]
        public int DebtId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Note { get; set; }
        [ForeignKey("CreateUser")]
        public int CreateUserId { get; set; }
        public virtual User CreateUser { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("MyOffice")]
        public int MyOfficeId { get; set; }
        public virtual MyOffice MyOffice { get; set; }
        public decimal Total { get; set; }
        public decimal Paid { get; set; }

        public bool ApproveFromManager { get; set; }
        public int? ApproveFromManagerId { get; set; }
        public DateTime? ApproveFromManagerAt { get; set; }
    }
}
