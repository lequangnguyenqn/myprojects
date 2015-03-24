using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Storages;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    [Authorize]
    public class ProductInStorageController : ControllerBase<IProductInStorageRepository, ProductInStorage>
    {
        private readonly IProductRepository _productRepository;

        private readonly IStorageRepository _storageRepository;
        public ProductInStorageController(IUnitOfWork unitOfWork, IProductInStorageRepository repository,
            IProductRepository productRepository, IStorageRepository storageRepository)
            : base(repository, unitOfWork)
        {
            _productRepository = productRepository;
            _storageRepository = storageRepository;
        }

        //
        // GET: /Administrator/MyOffice/

        public ActionResult Index(IndexProductInStorageModel productInStorageModel)
        {
            var products = _productRepository.Search("").ToList();
            var storages = _storageRepository.Search("").Where(p => WorkContext.MyStorages.Contains(p.StorageId)).ToList();
            var model = new IndexProductInStorageModel
            {
                Products = products,
                Storages = storages, 
                GroupByProduct = productInStorageModel.GroupByProduct,
                GroupByStorage = productInStorageModel.GroupByStorage,
                ProductsSelected = productInStorageModel.ProductsSelected,
                StoragesSelected = productInStorageModel.StoragesSelected
            };
            int[] productsArr = new int[0];
            if (!string.IsNullOrEmpty(productInStorageModel.ProductsSelected))
            {
                productsArr = productInStorageModel.ProductsSelected.Split(',').Select(int.Parse).ToArray();
            }
            int[] storagesArr = new int[0];
            if (!string.IsNullOrEmpty(productInStorageModel.StoragesSelected))
            {
                storagesArr = productInStorageModel.StoragesSelected.Split(',').Select(int.Parse).ToArray();
            }
            var filter = new ReportFilterModel
            {
                ListProducts = productsArr,
                FromListStorages =storagesArr
            };
            model.ProductInStorageModels = GridModel(filter);
            return View(model);
        }

        private List<ProductInStorageModel> GridModel(ReportFilterModel filterModel)
        {
            var listProducts = new int[1];
            var isGetAllListProducts = true;
            //Filter by deliver to
            if (filterModel.ListProducts != null && filterModel.ListProducts.Count() > 0)
            {
                listProducts = filterModel.ListProducts;
                isGetAllListProducts = false;
            }
            var listFromListStorages = new int[1];
            var isGetAllFromListStorages = true;
            //Filter by deliver to
            if (filterModel.FromListStorages != null && filterModel.FromListStorages.Count() > 0)
            {
                listFromListStorages = filterModel.FromListStorages;
                isGetAllFromListStorages = false;
            }
            var model = from x in Repository.Search(filterModel.search).Where(p =>
                        (listProducts.Contains(p.ProductId) || (isGetAllListProducts)) &&
                        (listFromListStorages.Contains(p.StorageId) || (isGetAllFromListStorages && WorkContext.MyStorages.Contains(p.StorageId))))
                        select new ProductInStorageModel
                        {
                            StorageId = x.StorageId,
                            ProductId = x.ProductId,
                            ProductName = x.Product.ProductName,
                            StorageName = x.Storage.StorageName,
                            Amount = x.Amount,
                            ProductInStorageId = x.ProductInStorageId
                        };

            return model.ToList();
        }

    }
}
