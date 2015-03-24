using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    public class CustomerLogoController : ControllerBase<ICustomerLogoRepository, CustomerLogo>
    {
        public CustomerLogoController(IUnitOfWork unitOfWork, ICustomerLogoRepository repository)
            : base(repository, unitOfWork)
        {
        }

        //
        // GET: /Administrator/CustomerLogo/
        [SuperAdminAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var model = Repository.Search(search);

            var gridModel = new GridModel<CustomerLogoModel>
            {
                Data = model.Select(x => new CustomerLogoModel
                                             {
                                                 CustomerLogoId = x.CustomerLogoId,
                                                 CustomerName = x.CustomerName,
                                                 LogoUrl = x.LogoUrl
                                             })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        [SuperAdminAuthorize]
        public ActionResult Create()
        {
            var model = new CustomerLogoModel();
            return View(model);
        }
        [SuperAdminAuthorize]
        public virtual ActionResult Edit(int id)
        {
            var entity = Repository.GetById(id);
            var model = new CustomerLogoModel()
            {
                CustomerLogoId = entity.CustomerLogoId,
                CustomerName = entity.CustomerName,
                LogoUrl = entity.LogoUrl,
                HomePageUrl = entity.HomePageUrl
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(CustomerLogoModel customerModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", customerModel);
            }
            if (customerModel.CustomerLogoId <= 0) //Create News
            {
                var customer = new CustomerLogo()
                {
                    CustomerName = customerModel.CustomerName,
                    LogoUrl = customerModel.LogoUrl,
                    HomePageUrl = customerModel.HomePageUrl
                };
                using (UnitOfWork)
                {
                    Repository.Insert(customer);
                }
            }
            else //Edit user
            {
                var customer = Repository.GetById(customerModel.CustomerLogoId);
                customer.CustomerName = customerModel.CustomerName;
                customer.LogoUrl = customerModel.LogoUrl;
                customer.HomePageUrl = customerModel.HomePageUrl;
                using (UnitOfWork)
                {
                    Repository.Update(customer);
                }
            }

            //Save success
            SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Logo khách hàng"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    Repository.Delete(id);
                }
                this.SetSuccessNotification("Logo khách hàng đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Logo khách hàng này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }

        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public string Upload(HttpPostedFileBase fileData)
        {
            //Save image
            var fileName = this.Server.MapPath("~/Uploads/" + System.IO.Path.GetFileName(fileData.FileName));
            fileData.SaveAs(fileName);
            return "ok";
        }
    }
}
