using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [SuperAdminAuthorize]
    public class CategoryController : ControllerBase<ICategoryRepository, Category>
    {

        public CategoryController(IUnitOfWork unitOfWork, ICategoryRepository repository)
            : base(repository, unitOfWork)
        {
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var model = Repository.Search(search);

            var gridModel = new GridModel<CategoryModel>
            {
                Data = model.Select(x => new CategoryModel
                {
                    CategoryId = x.CategoryId,
                    CategoryCode= x.CategoryCode,
                    CategoryName = x.CategoryName
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var model = new CategoryModel();
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var entity = Repository.GetById(id);
            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                CategoryCode = entity.CategoryCode,
                CategoryName = entity.CategoryName,
                Desciption = entity.Desciption
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(CategoryModel myOfficeModel)
        {
            if (myOfficeModel.CategoryId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", myOfficeModel);
                }
                var myOffice = new Category()
                {
                    IsDeleted = false,
                    CategoryCode = myOfficeModel.CategoryCode,
                    CategoryName= myOfficeModel.CategoryName,
                    Desciption = myOfficeModel.Desciption,
                    Products = new List<Product>()
                };
                using (UnitOfWork)
                {
                    Repository.Insert(myOffice);
                }
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", myOfficeModel);
                }

                var myOffice = Repository.GetById(myOfficeModel.CategoryId);
                myOffice.CategoryCode = myOfficeModel.CategoryCode;
                myOffice.CategoryName = myOfficeModel.CategoryName;
                myOffice.Desciption = myOfficeModel.Desciption;
                using (UnitOfWork)
                {
                    Repository.Update(myOffice);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Loại sản phẩm"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    var entity = Repository.GetById(id);
                    entity.CategoryCode = entity.CategoryCode + "_DELETE";
                    entity.IsDeleted = true;
                }
                this.SetSuccessNotification("Loại sản phẩm đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Loại sản phẩm này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
