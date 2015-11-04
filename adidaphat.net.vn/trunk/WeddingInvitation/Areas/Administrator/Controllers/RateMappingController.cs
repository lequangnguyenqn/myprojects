using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class RateMappingController : ControllerBase<IRateMappingRepository, RateMapping>
    {
        private readonly IStateProvinceRepository _stateProvinceRepository;
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IProductRepository _productRepository;

        public RateMappingController(IUnitOfWork unitOfWork, IRateMappingRepository repository,
                                    IStateProvinceRepository stateProvinceRepository, IMyOfficeRepository myOfficeRepository,
            IProductRepository productRepository)
            : base(repository, unitOfWork)
        {
            _stateProvinceRepository = stateProvinceRepository;
            _myOfficeRepository = myOfficeRepository;
            _productRepository = productRepository;
        }

        //
        // GET: /Administrator/RateMapping/

        public ActionResult Index(int? mode)
        {
            return View(mode);
        }

        [GridAction]
        public ActionResult GridModel(string search)
        {
            var query = from x in Repository.Search(search).Where(p => WorkContext.MyOffices.Contains(p.MyOfficeId)).Include(p => p.MyOffice).Include(p => p.Product)
                        select new RateMappingModel
                        {
                            RateMappingId = x.RateMappingId,
                            MyOfficeName = x.MyOffice.OfficeName,
                            ProductName = x.Product.ProductName,
                            Price = x.Price,
                            PrintingIncludeImagePrice = x.PrintingIncludeImagePrice,
                            PrintingWithoutImagePrice = x.PrintingWithoutImagePrice
                        };

            var gridModel = new GridModel<RateMappingModel>
            {
                Data = query
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var products = _productRepository.Search("").ToList();
            var model = new RateMappingModel()
            {
                MyOffices = offices,
                Products = products
            };

            return View(model);
        }

        public virtual ActionResult Edit(int id)
        {
            var offices = _myOfficeRepository.Search("").ToList();
            var products = _productRepository.Search("").ToList();
            var entity = Repository.GetById(id);
            var model = new RateMappingModel()
            {
                Products = products,
                MyOffices = offices,
                Price = entity.Price,
                PrintingIncludeImagePrice = entity.PrintingIncludeImagePrice,
                PrintingWithoutImagePrice = entity.PrintingWithoutImagePrice,
                MyOfficeId = entity.MyOfficeId,
                ProductId = entity.ProductId,
                RateMappingId = entity.RateMappingId,
            };
            return View("Edit", model);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(RateMappingModel rateMappingModel)
        {
            if (rateMappingModel.RateMappingId <= 0) //Create News
            {
                if (string.IsNullOrEmpty(rateMappingModel.ProductSelected))
                {
                    this.SetErrorNotification(string.Format("Bạn chưa chọn sản phẩm."));
                    return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
                }
                var products = rateMappingModel.ProductSelected.Split(',').Select(int.Parse).ToList();
                foreach (var i in products)
                {
                    var modelExisted = Repository.GetAll().FirstOrDefault(p => p.MyOfficeId == rateMappingModel.MyOfficeId && p.ProductId == i);
                    if ((modelExisted != null && modelExisted.RateMappingId != rateMappingModel.RateMappingId) || (modelExisted != null && rateMappingModel.RateMappingId <= 0))
                    {
                        var product = _productRepository.GetById(i);
                        this.SetErrorNotification(string.Format("Sản phẩm {0} đã được định giá.",product.ProductName));
                        return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
                    }
                }
                using (UnitOfWork)
                {
                    foreach (var productId in products)
                    {
                        var entity = new RateMapping()
                        {
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ProductId = productId,
                            MyOfficeId = rateMappingModel.MyOfficeId,
                            Price = rateMappingModel.Price,
                            PrintingIncludeImagePrice = rateMappingModel.PrintingIncludeImagePrice,
                            PrintingWithoutImagePrice = rateMappingModel.PrintingWithoutImagePrice
                        };

                        Repository.Insert(entity);
                    }
                }

                SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Định giá"));
                return RedirectToAction("Index", new { area = "Administrator", mode = 1 });
            }
            else //Edit user
            {
                //Check existed
                var modelExisted = Repository.GetAll().FirstOrDefault(p => p.MyOfficeId == rateMappingModel.MyOfficeId && p.ProductId == rateMappingModel.ProductId);
                if ((modelExisted != null && modelExisted.RateMappingId != rateMappingModel.RateMappingId) || (modelExisted != null && rateMappingModel.RateMappingId <= 0))
                {
                    this.SetErrorNotification("Sản phẩm này đã được định giá.");
                    var offices = _myOfficeRepository.Search("").ToList();
                    var products = _productRepository.Search("").ToList();
                    rateMappingModel.MyOffices = offices;
                    rateMappingModel.Products = products;
                    return View("Edit", rateMappingModel);
                }
                var entity = Repository.GetById(rateMappingModel.RateMappingId);
                entity.Price = rateMappingModel.Price;
                entity.PrintingIncludeImagePrice = rateMappingModel.PrintingIncludeImagePrice;
                entity.PrintingWithoutImagePrice = rateMappingModel.PrintingWithoutImagePrice;
                entity.MyOfficeId = rateMappingModel.MyOfficeId;
                entity.ProductId = rateMappingModel.ProductId;
                using (UnitOfWork)
                {
                    Repository.Update(entity);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Định giá"));
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
                this.SetSuccessNotification("Định giá đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Định giá này không thể xóa, vì đã được sử dụng!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
