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
    public class ProductController : ControllerBase<IProductRepository, Product>
    {
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IUnitOfWork unitOfWork, IProductRepository repository,
            ICategoryRepository categoryRepository)
            : base(repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
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

            var gridModel = new GridModel<ProductModel>
            {
                Data = model.Select(x => new ProductModel
                {
                    DefaultPrice = x.DefaultPrice,
                    ProductCode = x.ProductCode,
                    ProductName =x.ProductName,
                    ProductId = x.ProductId,
                    DefaultPrintingIncludeImagePrice = x.DefaultPrintingIncludeImagePrice,
                    DefaultPrintingWithoutImagePrice = x.DefaultPrintingWithoutImagePrice,
                })
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var categories = _categoryRepository.Search("").ToList();
            var model = new ProductModel { Categories = categories, Printable = true};
            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var categories = _categoryRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new ProductModel()
            {
                CategoryId = entity.CategoryId,
                DefaultPrice = entity.DefaultPrice,
                DefaultOriginalPrice = entity.DefaultOriginalPrice,
                DefaultOriginalPrintingIncludeImagePrice = entity.DefaultOriginalPrintingIncludeImagePrice,
                DefaultPrintingIncludeImagePrice = entity.DefaultPrintingIncludeImagePrice,
                DefaultOriginalPrintingWithoutImagePrice = entity.DefaultOriginalPrintingWithoutImagePrice,
                DefaultPrintingWithoutImagePrice = entity.DefaultPrintingWithoutImagePrice,
                Desciption = entity.Desciption,
                ProductCode =entity.ProductCode,
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                Categories = categories,
                Printable = entity.Printable
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(ProductModel model)
        {
            if (model.ProductId <= 0) //Create News
            {
                if (!ModelState.IsValid)
                {
                    var categories = _categoryRepository.Search("").ToList();
                    model.Categories = categories;
                    return View("Create", model);
                }
                var entity = new Product()
                {
                    IsDeleted = false,
                    CategoryId = model.CategoryId,
                    DefaultOriginalPrice = model.DefaultOriginalPrice,
                    DefaultOriginalPrintingIncludeImagePrice = model.DefaultOriginalPrintingIncludeImagePrice,
                    DefaultPrice = model.DefaultPrice,
                    DefaultPrintingIncludeImagePrice = model.DefaultPrintingIncludeImagePrice,
                    DefaultPrintingWithoutImagePrice = model.DefaultPrintingWithoutImagePrice,
                    DefaultOriginalPrintingWithoutImagePrice = model.DefaultOriginalPrintingWithoutImagePrice,
                    Desciption = model.Desciption,
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    Printable = model.Printable
                };
                using (UnitOfWork)
                {
                    Repository.Insert(entity);
                }
            }
            else //Edit user
            {
                if (!ModelState.IsValid)
                {
                    var categories = _categoryRepository.Search("").ToList();
                    model.Categories = categories;
                    return View("Edit", model);
                }

                var entity = Repository.GetById(model.ProductId);
                entity.CategoryId = model.CategoryId;
                entity.DefaultPrice = model.DefaultPrice;
                entity.DefaultOriginalPrice = model.DefaultOriginalPrice;
                entity.DefaultOriginalPrintingIncludeImagePrice = model.DefaultOriginalPrintingIncludeImagePrice;
                entity.DefaultPrintingIncludeImagePrice = model.DefaultPrintingIncludeImagePrice;
                entity.DefaultOriginalPrintingWithoutImagePrice = model.DefaultOriginalPrintingWithoutImagePrice;
                entity.DefaultPrintingWithoutImagePrice = model.DefaultPrintingWithoutImagePrice;
                entity.Desciption = model.Desciption;
                entity.ProductCode = model.ProductCode;
                entity.ProductName = model.ProductName;
                entity.Printable = model.Printable;
                using (UnitOfWork)
                {
                    Repository.Update(entity);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Sản phẩm"));
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
                }
                this.SetSuccessNotification("Sản phẩm đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Sản phẩm này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
