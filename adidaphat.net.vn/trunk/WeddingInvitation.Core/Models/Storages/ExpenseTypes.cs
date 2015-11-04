using System.Web.Mvc;

namespace WeddingInvitation.Core.Models.Storages
{
    public class ExpenseTypes
    {
        public enum EnumExpenseTypes
        {
            Salary = 1,
            Shipping = 2,
            Rent = 3,
            PhoneAndInternet = 4,
            Incurred = 5,
            Delivery = 6,
            Gas = 7,
            Repairs = 8
        }
        public static SelectList GetExpenseTypes()
        {
            var values = new[]
                            {
                                new { Value = (int)EnumExpenseTypes.Salary, Text = "Tiền lương" },
                                new { Value = (int)EnumExpenseTypes.Shipping, Text = "Tiền cước xe gởi tỉnh" },
                                new { Value = (int)EnumExpenseTypes.Rent, Text = "Tiền thuê nhà" },
                                new { Value = (int)EnumExpenseTypes.PhoneAndInternet, Text = "Tiền điện thoại/internet" },
                                new { Value = (int)EnumExpenseTypes.Delivery, Text = "Tiền chuyển phát hàng" },
                                new { Value = (int)EnumExpenseTypes.Gas, Text = "Tiền xăng" },
                                new { Value = (int)EnumExpenseTypes.Repairs, Text = "Tiền sửa xe " },
                                new { Value = (int)EnumExpenseTypes.Incurred, Text = "Phí phát sinh" }
                            };
            return new SelectList(values, "Value", "Text");
        }
    }

    public class DepartmentTypes
    {
        public enum EnumDepartmentTypes
        {
            Accounting = 1,
            Manager = 2,
            Storage = 3,
            DeliveryMagager = 4,
            DeliveryStaff = 5,
            Sale = 6,
            Printer = 7
        }
        public static SelectList GetExpenseTypes()
        {
            var values = new[]
                            {
                                new { Value = (int)EnumDepartmentTypes.Accounting, Text = "Kế toán" },
                                new { Value = (int)EnumDepartmentTypes.Manager, Text = "Quản lý chung" },
                                new { Value = (int)EnumDepartmentTypes.Storage, Text = "Bộ phận kho" },
                                new { Value = (int)EnumDepartmentTypes.DeliveryMagager, Text = "Bộ phận giao hàng" },
                                new { Value = (int)EnumDepartmentTypes.DeliveryStaff, Text = "Giao hàng tỉnh" },
                                new { Value = (int)EnumDepartmentTypes.Sale, Text = "Bô phận kinh doanh" },
                                new { Value = (int)EnumDepartmentTypes.Printer, Text = "Bộ phận in" },
                            };
            return new SelectList(values, "Value", "Text");
        }
    }
}
