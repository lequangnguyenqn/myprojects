using System;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ReportFilterModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int[] ListOffices { get; set; }
        public int[] ListProducts { get; set; }
        public int[] FromListStorages { get; set; }
        public int[] ToListStorages { get; set; }
        public string search { get; set; }
        public int[] ListDepartmentTypes { get; set; }
        public int[] ListExpenseTypes { get; set; }
        public int[] ListUsers { get; set; }
        public int[] ListCustomers { get; set; }
        public bool Tranfer { get; set; }
        public bool InShippingPlace { get; set; }
        public bool PrintingIncludeImage { get; set; }
        public bool PrintingWithoutImage { get; set; }
    }
}