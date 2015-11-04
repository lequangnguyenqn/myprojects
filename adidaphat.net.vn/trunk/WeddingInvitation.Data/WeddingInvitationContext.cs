using System.Data.Entity;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Core.Models.Orders;

namespace WeddingInvitation.Data
{
    public class WeddingInvitationContext : DbContext
    {
        //DbSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<NewsCategoryItem> NewsCategoryItems { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<StaticPage> StaticPages { get; set; }
        public DbSet<MyOffice> MyOffices { get; set; }
        public DbSet<RateMapping> RateMappings { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerLogo> CustomerLogos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ImportTrack> ImportTracks { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExportTrack> ExportTracks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShippingService> ShippingServices { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ProductInStorage> ProductInStorages { get; set; }
        public DbSet<ExtraFee> ExtraFees { get; set; }
        public DbSet<ShippingFee> ShippingFees { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferItem> TransferItems { get; set; }
        public DbSet<OrderDeliveryPackage> OrderDeliveryPackages { get; set; }
        public DbSet<PaymentPeriod> PaymentPeriods { get; set; }
        public DbSet<Debt> Debts { get; set; }

        private static object _objLock = new object();
        private static WeddingInvitationContext _current = null;

        /// <summary>
        /// DbContext singleton pattern
        /// </summary>
        public static WeddingInvitationContext Current
        {
            get
            {
                lock (_objLock)
                {
                    if (_current == null)
                    {
                        _current = new WeddingInvitationContext();
                    }
                    return _current;
                }
            }
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public WeddingInvitationContext()
        {
            var config = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"];
            if (config != null)
            {
                this.Database.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        /// <summary>
        /// Mapping data
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
