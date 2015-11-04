using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ExpenseController : ControllerBase<IExpenseRepository, Expense>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ExpenseController(IUnitOfWork unitOfWork, IExpenseRepository repository,
            IMyOfficeRepository myOfficeRepository, IProductRepository productRepository,
            IUserRepository userRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            var myOffices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var employees = _userRepository.Search("").ToList();
            var model = new ExpenseModel
            {
                MyOffices = myOffices,
                Employees = employees
            };
            return View(model);
        }

        [GridAction]
        public ActionResult GridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            //Filter by date
            if (filterModel.FromDate.HasValue)
            {
                fromDate = filterModel.FromDate.Value.Date;
                isAllFromDate = false;
            }
            if (filterModel.ToDate.HasValue)
            {
                toDate = filterModel.ToDate.Value.Date.AddDays(1);
                isAllToDate = false;
            }
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listDepartmentTypes = new int[1];
            var isGetAllListDepartmentTypes = true;
            //Filter by deliver to
            if (filterModel.ListDepartmentTypes != null && filterModel.ListDepartmentTypes.Count() > 0)
            {
                listDepartmentTypes = filterModel.ListDepartmentTypes;
                isGetAllListDepartmentTypes = false;
            }
            var listUsers = new int[1];
            var isGetAllListUsers = true;
            //Filter by deliver to
            if (filterModel.ListUsers != null && filterModel.ListUsers.Count() > 0)
            {
                listUsers = filterModel.ListUsers;
                isGetAllListUsers = false;
            }
            var listExpenseTypes = new int[1];
            var isGetAllListExpenseTypes = true;
            //Filter by deliver to
            if (filterModel.ListExpenseTypes != null && filterModel.ListExpenseTypes.Count() > 0)
            {
                listExpenseTypes = filterModel.ListExpenseTypes;
                isGetAllListExpenseTypes = false;
            }
            var model = from x in Repository.Search(filterModel.search).Where(p => 
                        (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                        (listDepartmentTypes.Contains(p.DepartmentType) || (isGetAllListDepartmentTypes)) &&
                        (listUsers.Contains(p.ReceiveUserId.Value) || (isGetAllListUsers)) &&
                        (listExpenseTypes.Contains(p.ExpenseType) || (isGetAllListExpenseTypes)) &&
                        ((p.CreateDate >= fromDate || isAllFromDate) && (p.CreateDate <= toDate || isAllToDate)))
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join t in _userRepository.GetAll() on x.ReceiveUserId equals t.UserId into t1
                        from t2 in t1.DefaultIfEmpty()
                        select new ExpenseModel
                        {
                            ExpenseId = x.ExpenseId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            Cost = x.Cost,
                            ExpenseName = x.ExpenseName,
                            ReceiveUserId = x.ReceiveUserId,
                            DepartmentType = x.DepartmentType,
                            ExpenseType =x.ExpenseType,
                            MyOfficeName = x.MyOffice.OfficeName,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            ReceiveUserName = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                        };
            double total = 0;
            try
            {
                total = Convert.ToDouble(model.Sum(p => p.Cost));
            }
            catch
            {
            }
            Session["TotalExpense"] = String.Format("{0:0,0}", total);
            var gridModel = new GridModel<ExpenseModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult GetTotalExpense()
        {
            return Json(Session["TotalExpense"]);
        }

        public ActionResult Manager()
        {
            var myOffices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var employees = _userRepository.Search("").ToList();
            var model = new ExpenseModel
            {
                MyOffices = myOffices,
                Employees = employees
            };
            return View(model);
        }

        [GridAction]
        public ActionResult ManagerGridModel(ReportFilterModel filterModel)
        {
            //Default get all
            var fromDate = DateTime.Today;
            var isAllFromDate = true;
            var toDate = DateTime.Today;
            var isAllToDate = true;
            //Filter by date
            if (filterModel.FromDate.HasValue)
            {
                fromDate = filterModel.FromDate.Value.Date;
                isAllFromDate = false;
            }
            if (filterModel.ToDate.HasValue)
            {
                toDate = filterModel.ToDate.Value.Date.AddDays(1);
                isAllToDate = false;
            }
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var listDepartmentTypes = new int[1];
            var isGetAllListDepartmentTypes = true;
            //Filter by deliver to
            if (filterModel.ListDepartmentTypes != null && filterModel.ListDepartmentTypes.Count() > 0)
            {
                listDepartmentTypes = filterModel.ListDepartmentTypes;
                isGetAllListDepartmentTypes = false;
            }
            var listUsers = new int[1];
            var isGetAllListUsers = true;
            //Filter by deliver to
            if (filterModel.ListUsers != null && filterModel.ListUsers.Count() > 0)
            {
                listUsers = filterModel.ListUsers;
                isGetAllListUsers = false;
            }
            var listExpenseTypes = new int[1];
            var isGetAllListExpenseTypes = true;
            //Filter by deliver to
            if (filterModel.ListExpenseTypes != null && filterModel.ListExpenseTypes.Count() > 0)
            {
                listExpenseTypes = filterModel.ListExpenseTypes;
                isGetAllListExpenseTypes = false;
            }
            var model = from x in Repository.Search(filterModel.search).Where(p => p.ApproveFromManager &&
                        (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId))) &&
                        (listDepartmentTypes.Contains(p.DepartmentType) || (isGetAllListOffices)) &&
                        (listUsers.Contains(p.ReceiveUserId.Value) || (isGetAllListUsers)) &&
                        (listExpenseTypes.Contains(p.ExpenseType) || (isGetAllListExpenseTypes)))
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join t in _userRepository.GetAll() on x.ReceiveUserId equals t.UserId into t1
                        from t2 in t1.DefaultIfEmpty()
                        select new ExpenseModel
                        {
                            ExpenseId = x.ExpenseId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            Cost = x.Cost,
                            ExpenseName = x.ExpenseName,
                            ReceiveUserId = x.ReceiveUserId,
                            DepartmentType = x.DepartmentType,
                            ExpenseType = x.ExpenseType,
                            MyOfficeName = x.MyOffice.OfficeName,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            ReceiveUserName = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                        };
            double total = 0;
            try
            {
                total = Convert.ToDouble(model.Sum(p => p.Cost));
            }
            catch
            {
            }
            Session["TotalExpense"] = String.Format("{0:0,0}", total);
            var gridModel = new GridModel<ExpenseModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var users = _userRepository.Search("").ToList();
            var model = new ExpenseModel { MyOffices = offices, Employees = users,ExpenseType = (int)ExpenseTypes.EnumExpenseTypes.Incurred};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var users = _userRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new ExpenseModel()
            {
                ExpenseId = entity.ExpenseId,
                Note = entity.Note,
                MyOfficeId = entity.MyOfficeId,
                MyOffices = offices,
                Cost = entity.Cost,
                ExpenseName = entity.ExpenseName,
                Employees =users,
                DepartmentType =entity.DepartmentType,
                SalaryOfEmployeeId = entity.SalaryOfEmployeeId.HasValue ? entity.SalaryOfEmployeeId.Value : 0,
                ReceiveUserId =entity.ReceiveUserId,
                ExpenseType = entity.ExpenseType
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ExpenseModel myOfficeModel)
        {
            if (myOfficeModel.ExpenseId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                    var users = _userRepository.Search("").ToList();
                    myOfficeModel.MyOffices = offices;
                    myOfficeModel.Employees = users;
                    return View("Create", myOfficeModel);
                }
                var myOffice = new Expense
                {
                    IsDeleted = false,
                    CreateDate = DateTime.Now,
                    CreateUserId = WorkContext.CurrentUserId,
                    Note = myOfficeModel.Note,
                    MyOfficeId = myOfficeModel.MyOfficeId,
                    ExpenseName = myOfficeModel.ExpenseName,
                    Cost = myOfficeModel.Cost,
                    ReceiveUserId = myOfficeModel.ReceiveUserId,
                    DepartmentType = myOfficeModel.DepartmentType,
                    SalaryOfEmployeeId = myOfficeModel.SalaryOfEmployeeId > 0 ? myOfficeModel.SalaryOfEmployeeId : 0,
                    ExpenseType = myOfficeModel.ExpenseType
                };
                using (UnitOfWork)
                {
                    Repository.Insert(myOffice);
                }
                this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Chi phí"));
                return RedirectToAction("Create", new { area = "Administrator" });
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    var offices = _myOfficeRepository.Search("").ToList();
                    var users = _userRepository.Search("").ToList();
                    myOfficeModel.MyOffices = offices;
                    myOfficeModel.Employees = users;
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.ExpenseId);
                myOffice.Note = myOfficeModel.Note;
                myOffice.MyOfficeId = myOfficeModel.MyOfficeId;
                myOffice.ExpenseName = myOfficeModel.ExpenseName;
                myOffice.Cost = myOfficeModel.Cost;
                myOffice.ExpenseType = myOfficeModel.ExpenseType;
                myOffice.DepartmentType = myOfficeModel.DepartmentType;
                myOffice.SalaryOfEmployeeId = myOfficeModel.SalaryOfEmployeeId > 0 ? myOfficeModel.SalaryOfEmployeeId : 0;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Chi phí"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var entity = Repository.GetById(id);
                using (UnitOfWork)
                {
                    
                    entity.IsDeleted = true;
                }
                this.SetSuccessNotification("Chi phí đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Chi phí này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("ManagerApprove", "Expense", new { area = "Administrator" });
        }

        public ActionResult GetCost(int? expenseType, decimal? largePackage, decimal? smallPackage, int? userId)
        {
            decimal totalCost = 0;
            if (expenseType == (int)ExpenseTypes.EnumExpenseTypes.Salary)
            {
                var user = _userRepository.GetById(userId);
                totalCost = user.Salary;
                return Json(totalCost);
            }
            if (expenseType == (int)ExpenseTypes.EnumExpenseTypes.Shipping)
            {
                if (largePackage.HasValue)
                {
                    totalCost += largePackage.Value * 13000;
                }
                if (smallPackage.HasValue)
                {
                    totalCost += smallPackage.Value * 9000;
                }
                return Json(totalCost);
            }
            return Json(totalCost);
        }

        #region Quản Lý xác nhận chi tiền
        public ActionResult ManagerApprove()
        {
            return View();
        }

        [GridAction]
        public ActionResult ManagerApproveGridModel(string search)
        {
            var model = from x in Repository.Search(search).Where(p => !p.ApproveFromManager)
                        join m in _userRepository.GetAll() on x.ApproveFromManagerId equals m.UserId into m1
                        from m2 in m1.DefaultIfEmpty()
                        join t in _userRepository.GetAll() on x.ReceiveUserId equals t.UserId into t1
                        from t2 in t1.DefaultIfEmpty()
                        select new ExpenseModel
                        {
                            ExpenseId = x.ExpenseId,
                            CreateDate = x.CreateDate,
                            Note = x.Note,
                            Cost = x.Cost,
                            ExpenseName = x.ExpenseName,
                            ReceiveUserId = x.ReceiveUserId,
                            DepartmentType = x.DepartmentType,
                            ExpenseType =x.ExpenseType,
                            MyOfficeName = x.MyOffice.OfficeName,
                            ApproveFromManagerName = (m2 == null ? String.Empty : m2.FirstName + " " + m2.LastName),
                            ReceiveUserName = (t2 == null ? String.Empty : t2.FirstName + " " + t2.LastName),
                        };

            var gridModel = new GridModel<ExpenseModel>
            {
                Data = model
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult ManagerApproveDone(int id)
        {
            var entity = Repository.GetById(id);
            using (UnitOfWork)
            {
                entity.ApproveFromManager = true;
                entity.ApproveFromManagerAt = DateTime.Now;
                entity.ApproveFromManagerId = WorkContext.CurrentUserId;
                Repository.Update(entity);
            }
            this.SetSuccessNotification("Đã xác nhận thành công!");
            return RedirectToAction("ManagerApprove", "Expense", new { area = "Administrator" });
        }
        #endregion
    }
}
