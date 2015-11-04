using System;
using System.Collections.Generic;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Core.Models.Users;

namespace WeddingInvitation.Areas.Administrator.Models
{
    public class ExpenseModel
    {
        public int ExpenseId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Note { get; set; }
        public int CreateUserId { get; set; }
        public int MyOfficeId { get; set; }
        public List<MyOffice> MyOffices { get; set; }
        public string ExpenseName { get; set; }
        public decimal Cost { get; set; }
        public string CostDisplay { get { return String.Format("{0:0,0}", Cost); } }
        public int ExpenseType { get; set; }
        public string ExpenseTypeDisplay
        {
            get
            {
                switch (ExpenseType)
                {
                    case (int)ExpenseTypes.EnumExpenseTypes.Salary:
                        return "Tiền lương";
                    case (int)ExpenseTypes.EnumExpenseTypes.Shipping:
                        return "Tiền cước xe gởi tỉnh";
                    case (int)ExpenseTypes.EnumExpenseTypes.Rent:
                        return "Tiền thuê nhà";
                    case (int)ExpenseTypes.EnumExpenseTypes.PhoneAndInternet:
                        return "Tiền điện thoại/internet";
                    case (int)ExpenseTypes.EnumExpenseTypes.Delivery:
                        return "Tiền chuyển phát hàng";
                    case (int)ExpenseTypes.EnumExpenseTypes.Gas:
                        return "Tiền xăng";
                    case (int)ExpenseTypes.EnumExpenseTypes.Repairs:
                        return "Tiền sửa xe";
                    case (int)ExpenseTypes.EnumExpenseTypes.Incurred:
                        return "Phí phát sinh";
                    default:
                        return "";
                }
            }
        }
        public int LargePackage { get; set; }
        public int SmallPackage { get; set; }
        public int? SalaryOfEmployeeId { get; set; }
        public List<User> Employees { get; set; }
        public int? ReceiveUserId { get; set; }
        public string ReceiveUserName { get; set; }
        public int DepartmentType { get; set; }
        public string DepartmentTypeDisplay
        {
            get
            {
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.Accounting)
                {
                    return "Kế toán";
                }
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.DeliveryMagager)
                {
                    return "Bộ phận giao hàng";
                }
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.DeliveryStaff)
                {
                    return "Giao hàng tỉnh";
                }
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.Manager)
                {
                    return "Quản lý chung";
                }
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.Printer)
                {
                    return "Bộ phận in";
                }
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.Sale)
                {
                    return "Bô phận kinh doanh";
                }
                if (DepartmentType == (int)DepartmentTypes.EnumDepartmentTypes.Storage)
                {
                    return "Bộ phận kho";
                }
                return "";
            }
        }
        public string ApproveFromManagerName { get; set; }
        public string MyOfficeName { get; set; }
    }
}