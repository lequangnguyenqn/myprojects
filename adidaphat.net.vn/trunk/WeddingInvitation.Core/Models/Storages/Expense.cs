using System;
using System.ComponentModel.DataAnnotations;
using WeddingInvitation.Core.Models.Settings;

namespace WeddingInvitation.Core.Models.Storages
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        [ForeignKey("MyOffice")]
        public int MyOfficeId { get; set; }
        public virtual MyOffice MyOffice { get; set; }
        public int ExpenseType { get; set; }
        public int DepartmentType { get; set; }
        public bool IsDeleted { get; set; }
        public int LargePackage { get; set; }
        public int SmallPackage { get; set; }
        public int? SalaryOfEmployeeId { get; set; }
        public int? ReceiveUserId { get; set; }

        public bool ApproveFromManager { get; set; }
        public int? ApproveFromManagerId { get; set; }
        public DateTime? ApproveFromManagerAt { get; set; }
    }
}
