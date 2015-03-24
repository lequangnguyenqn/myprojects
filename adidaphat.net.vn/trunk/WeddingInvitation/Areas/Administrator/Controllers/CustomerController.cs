using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Data;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class CustomerController : ControllerBase<ICustomerRepository, Customer>
    {
        private readonly IMyOfficeRepository _myOfficeRepository;

        public CustomerController(IUnitOfWork unitOfWork, ICustomerRepository repository,
            IMyOfficeRepository myOfficeRepository)
            : base(repository, unitOfWork)
        {
            _myOfficeRepository = myOfficeRepository;
        }

        //
        // GET: /Administrator/Customer/

        public ActionResult Index(int? mode)
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new OrderModel
            {
                MyOffices = offices,
                Mode = mode
            };
            return View(model);
        }

        [GridAction]
        public ActionResult GridModel(ReportFilterModel filterModel)
        {
            var listOffices = new int[1];
            var isGetAllListOffices = true;
            //Filter by deliver to
            if (filterModel.ListOffices != null && filterModel.ListOffices.Count() > 0)
            {
                listOffices = filterModel.ListOffices;
                isGetAllListOffices = false;
            }
            var model = Repository.Search(filterModel.search).Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId) &&
                (listOffices.Contains(p.MyOfficeId) || (isGetAllListOffices && WorkContext.MyOffices.Contains(p.MyOfficeId)))
                && ((!WorkContext.Roles.Contains(RoleType.Administrator) && !p.HideWithNormalUser) || WorkContext.Roles.Contains(RoleType.Administrator))).OrderByDescending(p => p.CustomerId);

            var gridModel = new GridModel<CustomerModel>
            {
                Data = model.Select(x => new CustomerModel
                                             {
                                                 Address = x.Address,
                                                 CustomerId = x.CustomerId,
                                                 CustomerName = x.CustomerName,
                                                 PhoneNumber = x.PhoneNumber,
                                                 Fax = x.Fax,
                                                 CustomerCode = x.CustomerCode
                                             })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var model = new CustomerModel{MyOffices = offices};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var entity = Repository.GetById(id);
            var model = new CustomerModel
            {
                Address = entity.Address,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                Fax = entity.Fax,
                PhoneNumber = entity.PhoneNumber,
                CustomerCode = entity.CustomerCode,
                DiscountPercent = entity.DiscountPercent,
                UseSpecialRateTable = entity.UseSpecialRateTable,
                CustomerShortName = entity.CustomerShortName,
                MyOffices = offices,
                MyOfficeId = entity.MyOfficeId,
                Note = entity.Note,
                HideWithNormalUser = entity.HideWithNormalUser,
                CellPhoneNumber = entity.CellPhoneNumber
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(CustomerModel customerModel)
        {
            if (!ModelState.IsValid)
            {
                var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                customerModel.MyOffices = offices;
                return View("Create", customerModel);
            }
            //Check existed
            var modelExisted = Repository.GetAll().FirstOrDefault(p => p.PhoneNumber == customerModel.PhoneNumber);
            if ((modelExisted != null && modelExisted.CustomerId != customerModel.CustomerId) || (modelExisted != null && customerModel.CustomerId <= 0))
            {
                var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
                customerModel.MyOffices = offices;
                SetErrorNotification("Mã khách hàng này đã tồn tại trong hệ thống.");
                return View("Create", customerModel);
            }
            if (customerModel.CustomerId <= 0) //Create News
            {
                var customer = new Customer
                {
                    IsDeleted = false,
                    Address = customerModel.Address,
                    CustomerName = customerModel.CustomerName,
                    Fax = customerModel.Fax,
                    PhoneNumber = customerModel.PhoneNumber,
                    CustomerCode = customerModel.CustomerCode,
                    DiscountPercent = customerModel.DiscountPercent,
                    UseSpecialRateTable = customerModel.UseSpecialRateTable,
                    CustomerShortName = customerModel.CustomerShortName,
                    MyOfficeId = customerModel.MyOfficeId,
                    Note = customerModel.Note,
                };
                using (UnitOfWork)
                {
                    Repository.Insert(customer);
                }
                SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Khách hàng"));
                return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
            }
            else //Edit user
            {
                var customer = Repository.GetById(customerModel.CustomerId);
                customer.Address = customerModel.Address;
                customer.CustomerName = customerModel.CustomerName;
                customer.Fax = customerModel.Fax;
                customer.PhoneNumber = customerModel.PhoneNumber;
                customer.CustomerCode = customerModel.CustomerCode;
                customer.DiscountPercent = customerModel.DiscountPercent;
                customer.UseSpecialRateTable = customerModel.UseSpecialRateTable;
                customer.CustomerShortName = customerModel.CustomerShortName;
                customer.MyOfficeId = customerModel.MyOfficeId;
                customer.Note = customerModel.Note;
                customer.HideWithNormalUser = customerModel.HideWithNormalUser;
                if (customer.UseSpecialRateTable)
                {
                    CreateRateTables(customerModel.CustomerId);
                }
                using (UnitOfWork)
                {
                    Repository.Update(customer);
                }
                SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Khách hàng"));
            }
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    entity.IsDeleted = true;
                    entity.CustomerCode = entity.CustomerCode + "_Deleted";
                }
                SetSuccessNotification("Khách hàng đã được xóa thành công.");
            }
            catch
            {
                SetErrorNotification("Khách hàng này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        private void CreateRateTables(int customerId)
        {
        }

        public JsonResult GetCustomerNote(int? customerId)
        {
            var entity = Repository.GetById(customerId);
            if (entity != null)
            {
                return Json(entity.Note, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Detail(int id)
        {
            var offices = _myOfficeRepository.Search("").Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).ToList();
            var entity = Repository.GetById(id);
            var model = new CustomerModel
            {
                Address = entity.Address,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                Fax = entity.Fax,
                PhoneNumber = entity.PhoneNumber,
                CustomerCode = entity.CustomerCode,
                DiscountPercent = entity.DiscountPercent,
                UseSpecialRateTable = entity.UseSpecialRateTable,
                CustomerShortName = entity.CustomerShortName,
                MyOffices = offices,
                MyOfficeId = entity.MyOfficeId,
                Note = entity.Note,
                HideWithNormalUser = entity.HideWithNormalUser,
                CellPhoneNumber = entity.CellPhoneNumber
            };
            return View(model);
        }
    }
}
