using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web.Mvc;
using WeddingInvitation.Data;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Infrastructure.Services;
using WeddingInvitation.Infrastructure.Unity;
using WeddingInvitation.Services.Catalog;
using WeddingInvitation.Services.ContentManagement;
using WeddingInvitation.Services.Customers;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Orders;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;
namespace WeddingInvitation
{
    public class UnityConfiguration
    {
        public static void Setup()
        {
            // register object lifetime
            MvcUnityContainer.Container = new UnityContainer()
                // Dbcontext
                .RegisterTypeForHttpContext<DbContext, WeddingInvitationContext>()
                .RegisterTypeForHttpContext<IUnitOfWork, UnitOfWork>()
                // Repositories
                .RegisterTypeForHttpContext<IRoleRepository, RoleRepository>()
                .RegisterTypeForHttpContext<IUserRepository, UserRepository>()
                .RegisterTypeForHttpContext<INewsCategoryItemRepository, NewsCategoryItemRepository>()
                .RegisterTypeForHttpContext<INewsItemRepository, NewsItemRepository>()
                .RegisterTypeForHttpContext<IStaticPageRepository, StaticPageRepository>()
                .RegisterTypeForHttpContext<IMyOfficeRepository, MyOfficeRepository>()
                .RegisterTypeForHttpContext<IRateMappingRepository, RateMappingRepository>()
                .RegisterTypeForHttpContext<IStateProvinceRepository, StateProvinceRepository>()
                .RegisterTypeForHttpContext<ICustomerLogoRepository, CustomerLogoRepository>()
                .RegisterTypeForHttpContext<ICustomerRepository, CustomerRepository>()
                .RegisterTypeForHttpContext<IProductRepository, ProductRepository>()
                .RegisterTypeForHttpContext<ICategoryRepository, CategoryRepository>()
                .RegisterTypeForHttpContext<IOrderDetailRepository, OrderDetailRepository>()
                .RegisterTypeForHttpContext<IOrderRepository, OrderRepository>()
                .RegisterTypeForHttpContext<IShippingServiceRepository, ShippingServiceRepository>()
                .RegisterTypeForHttpContext<IImportTrackRepository, ImportTrackRepository>()
                .RegisterTypeForHttpContext<IExportTrackRepository, ExportTrackRepository>()
                .RegisterTypeForHttpContext<IExpenseRepository, ExpenseRepository>()
                .RegisterTypeForHttpContext<IProductInStorageRepository, ProductInStorageRepository>()
                .RegisterTypeForHttpContext<IStorageRepository, StorageRepository>()
                .RegisterTypeForHttpContext<IExtraFeeRepository, ExtraFeeRepository>()
                .RegisterTypeForHttpContext<IShippingFeeRepository, ShippingFeeRepository>()
                .RegisterTypeForHttpContext<IImportDetailRepository, ImportDetailRepository>()
                .RegisterTypeForHttpContext<IOrderDeliveryPackageRepository, OrderDeliveryPackageRepository>()
                .RegisterTypeForHttpContext<ITransferRepository, TransferRepository>()
                .RegisterTypeForHttpContext<ITransferItemRepository, TransferItemRepository>()
                .RegisterTypeForHttpContext<IPaymentPeriodRepository, PaymentPeriodRepository>()
                .RegisterTypeForHttpContext<IImportDetailRepository, ImportDetailRepository>()
                .RegisterTypeForHttpContext<IExportDetailRepository, ExportDetailRepository>()
                .RegisterTypeForHttpContext<IDebtRepository, DebtRepository>()
                //Services
                .RegisterTypeForHttpContext<IMailServerService, MailServerService>()
                .RegisterTypeForHttpContext<IPermissionService, PermissionService>()
                .RegisterTypeForHttpContext<IExportService, ExportService>()
                .RegisterTypeForHttpContext<ICacheManager, MemoryCacheManager>()
                ;
            
            ControllerBuilder.Current.SetControllerFactory(typeof(UnityControllerFactory));
        }
    }
}