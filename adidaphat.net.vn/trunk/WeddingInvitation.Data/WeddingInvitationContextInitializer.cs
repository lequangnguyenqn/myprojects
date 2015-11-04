using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Core.Models.ContentManagement;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Catalog;
using WeddingInvitation.Core.Models.Customers;
using WeddingInvitation.Core.Models.Storages;

namespace WeddingInvitation.Data
{
    public class WeddingInvitationContextInitializer : DropCreateDatabaseIfModelChanges<WeddingInvitationContext>
    {
        /// <summary>
        /// Dummy data
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(WeddingInvitationContext context)
        {
            base.Seed(context);
            //Fixed Data
            CreateRoles(context);
            CreateSupperAdminUser(context);
            CreateStaticPages(context);
            CreateExtraFee(context);
            CreateStateProvinces(context);
            CreateStorages(context);
            CreateMyOffices(context);
            //Example Data
            CreateUsers(context);
            CreateCategories(context);
            CreateShippingServices(context);
            CreateProductCategories(context);
            CreateProducts(context);
            CreateCustomers(context);
        }

        private void CreateExtraFee(WeddingInvitationContext context)
        {
            var entities = new List<ExtraFee>
                            {
                                new ExtraFee
                                    {
                                        IsDeleted= false,
                                        AmountFrom = 0,
                                        AmountTo = 200,
                                        Cost = 30000,
                                        ExtraFeeName = "Phụ thu 30 nghìn cho đơn hàng từ 0 -> 200 thiệp"
                                    }
                            };

            entities.ForEach(c => context.ExtraFees.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateStorages(WeddingInvitationContext context)
        {
            var entities = new List<Storage>
                            {
                                new Storage
                                    {
                                        IsDeleted= false,
                                        StorageName = "HCM",
                                        Desciption = "HCM"
                                    },
                                    new Storage
                                    {
                                        IsDeleted= false,
                                        StorageName = "Đà Nẵng",
                                        Desciption = "Đà Nẵng"
                                    }
                            };

            entities.ForEach(c => context.Storages.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateCustomers(WeddingInvitationContext context)
        {
            var entities = new List<Customer>
                            {
                                new Customer
                                    {
                                        IsDeleted= false,
                                        CustomerName = "ACB",
                                        CustomerCode = "ACB",
                                        PhoneNumber = "099787878",
                                        MyOfficeId = context.MyOffices.FirstOrDefault().MyOfficeId
                                    },
                                    new Customer
                                    {
                                        IsDeleted= false,
                                        CustomerName = "ANZ",
                                        CustomerCode = "ANZ",
                                        PhoneNumber = "099787878",
                                        MyOfficeId = context.MyOffices.FirstOrDefault().MyOfficeId
                                    },new Customer
                                    {
                                        IsDeleted= false,
                                        CustomerName = "HSBC",
                                        CustomerCode = "HSBC",
                                        PhoneNumber = "099787878",
                                        MyOfficeId = context.MyOffices.FirstOrDefault().MyOfficeId
                                    },new Customer
                                    {
                                        IsDeleted= false,
                                        CustomerName = "BIDV",
                                        CustomerCode = "BIDV",
                                        PhoneNumber = "099787878",
                                        MyOfficeId = context.MyOffices.FirstOrDefault().MyOfficeId
                                    }
                            };

            entities.ForEach(c => context.Customers.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateProducts(WeddingInvitationContext context)
        {
            var entities = new List<Product>
                            {
                                new Product
                                    {
                                        IsDeleted= false,
                                        ProductName ="H7 BIA",
                                        CategoryId = 1,
                                        DefaultPrice = 5000,
                                        DefaultOriginalPrice = 4000,
                                        DefaultPrintingIncludeImagePrice = 300,
                                        DefaultOriginalPrintingIncludeImagePrice = 200,
                                        DefaultPrintingWithoutImagePrice = 200,
                                        DefaultOriginalPrintingWithoutImagePrice = 100,
                                        ProductCode="H7BIA",
                                        Printable = true
                                    },
                                    new Product
                                    {
                                        IsDeleted= false,
                                        ProductName ="H7 RUOT",
                                        CategoryId = 1,
                                        DefaultPrice = 5000,
                                        DefaultOriginalPrice = 4000,
                                        DefaultPrintingIncludeImagePrice = 300,
                                        DefaultOriginalPrintingIncludeImagePrice = 200,
                                        DefaultPrintingWithoutImagePrice = 200,
                                        DefaultOriginalPrintingWithoutImagePrice = 100,
                                        ProductCode="H7RUOT"
                                    },
                                    new Product
                                    {
                                        IsDeleted= false,
                                        ProductName ="H8 BIA",
                                        CategoryId = 2,
                                        DefaultPrice = 7000,
                                        DefaultOriginalPrice = 6000,
                                        DefaultPrintingIncludeImagePrice = 300,
                                        DefaultOriginalPrintingIncludeImagePrice = 200,
                                        DefaultPrintingWithoutImagePrice = 200,
                                        DefaultOriginalPrintingWithoutImagePrice = 100,
                                        ProductCode="H8BIA",
                                        Printable = true
                                    },
                                    new Product
                                    {
                                        IsDeleted= false,
                                        ProductName ="H8 RUOT",
                                        CategoryId = 2,
                                        DefaultPrice = 7000,
                                        DefaultOriginalPrice = 6000,
                                        DefaultPrintingIncludeImagePrice = 300,
                                        DefaultOriginalPrintingIncludeImagePrice = 200,
                                        DefaultPrintingWithoutImagePrice = 200,
                                        DefaultOriginalPrintingWithoutImagePrice = 100,
                                        ProductCode="H8RUOT"
                                    }
                            };
            

            entities.ForEach(c => context.Products.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateProductCategories(WeddingInvitationContext context)
        {
            var entities = new List<Category>
                            {
                                new Category
                                    {
                                        IsDeleted= false,
                                        CategoryCode = "H7",
                                        CategoryName = "H7",
                                        Desciption = "H7"
                                    },
                                    new Category
                                    {
                                        IsDeleted= false,
                                        CategoryCode = "H8",
                                        CategoryName = "H8",
                                        Desciption = "H8"
                                    },
                                    new Category
                                    {
                                        IsDeleted= false,
                                        CategoryCode = "H9",
                                        CategoryName = "H9",
                                        Desciption = "H9"
                                    }
                            };

            entities.ForEach(c => context.Categories.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateShippingServices(WeddingInvitationContext context)
        {
            var entities = new List<ShippingService>
                            {
                                new ShippingService
                                    {
                                        IsDeleted= false,
                                        ShippingServiceName = "TP.HCM",
                                        MyOfficeId = context.MyOffices.FirstOrDefault().MyOfficeId,
                                        PhoneNumber= "098867868",
                                        StartAt = DateTime.Now,
                                        CoachStation = "Bến xe miền đông"
                                    },
                                new ShippingService
                                    {
                                        IsDeleted= false,
                                        ShippingServiceName = "KonTum",
                                        MyOfficeId = context.MyOffices.FirstOrDefault().MyOfficeId,
                                        PhoneNumber= "098867868",
                                        StartAt = DateTime.Now,
                                        CoachStation = "Bến xe miền tây"
                                    },
                            };

            entities.ForEach(c => context.ShippingServices.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateSupperAdminUser(WeddingInvitationContext context)
        {
            string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
            string hashedPassword = PasswordHelper.CreatePasswordHash("123456", passwordSalt);
            //Create user super admin
            var listRoleSuperAdmin = new List<Role> { context.Roles.FirstOrDefault(p => p.RoleName == RoleType.Administrator) };
            context.Users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Le Quang",
                LastName = "Nguyên",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "lequangnguyenqn@gmail.com",
                Username = "brierlytom@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = listRoleSuperAdmin,
            });
            context.SaveChanges(); //save to database
        }

        private void CreateUsers(WeddingInvitationContext context)
        {
            string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
            string hashedPassword = PasswordHelper.CreatePasswordHash("123456", passwordSalt);
            var users = new List<User>();
            //Create list role
            var listRoleAdmin = new List<Role>();
            listRoleAdmin.Add(context.Roles.FirstOrDefault(p => p.RoleName == RoleType.KinhDoanh));
            //Create users
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Kế",
                LastName = "Toán",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "KeToan" + "@gmail.com",
                Username = "KeToan" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.KeToan)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Quản Lý",
                LastName = "In",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "QuanLyIn" + "@gmail.com",
                Username = "QuanLyIn" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.QuanLyIn)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Kinh",
                LastName = "Doanh",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "KinhDoanh" + "@gmail.com",
                Username = "KinhDoanh" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.KinhDoanh)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Bo Phan",
                LastName = "Giao Hang",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "GiaoHang" + "@gmail.com",
                Username = "GiaoHang" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.GiaoHang)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "NV Giao Hàng",
                LastName = "Tỉnh",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "NVGiaoHangTinh" + "@gmail.com",
                Username = "NVGiaoHangTinh" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.NVGiaoHangTinh)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Quản Lý",
                LastName = "Chung",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "QuanLyChung" + "@gmail.com",
                Username = "QuanLyChung" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.QuanLyChung)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.Add(new User
            {
                Address1 = "12 Road Name",
                Address2 = "",
                CompanyName = "Company Name",
                FirstName = "Bộ Phận",
                LastName = "Kho",
                ContactNumber = "01534 123445",
                CountryRegion = "US",
                EmailAddress = "NVKho" + "@gmail.com",
                Username = "NVKho" + "@gmail.com",
                PostCodeZip = "HD7 2DS",
                TownCity = "Huddersfield",
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                IsDeleted = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>
                {
                    context.Roles.FirstOrDefault(p => p.RoleName == RoleType.NVKho)
                },
                MyOffices = new List<MyOffice>(),
                Storages = new List<Storage>()
            });
            users.ForEach(c => context.Users.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateCategories(WeddingInvitationContext context)
        {
            var categories = new List<NewsCategoryItem>
                            {
                                new NewsCategoryItem
                                    {
                                        CategoryName = "Tin tức",
                                        Active = true,
                                        CategoryDescription = "Tin tức",
                                        DisplayOrder = 0,
                                        IsDeleted = false,
                                        Level = 0,
                                    },
                                new NewsCategoryItem
                                    {
                                        CategoryName = "Tuyển dụng",
                                        Active = true,
                                        CategoryDescription = "Tuyển dụng",
                                        DisplayOrder = 0,
                                        IsDeleted = false,
                                        Level = 0,
                                    },
                            };

            categories.ForEach(c => context.NewsCategoryItems.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateStaticPages(WeddingInvitationContext context)
        {
            var pages = new List<StaticPage>
                            {
                                new StaticPage
                                    {
                                        Content = "Nhập nội dung vào đây",
                                        Description = "Trang giới thiệu",
                                        IsDeleted = false,
                                        Type = (int)EnumStaticPages.About,
                                    },
                                new StaticPage
                                    {
                                        Content = "Nhập nội dung vào đây",
                                        Description = "Trang liên hệ",
                                        IsDeleted = false,
                                        Type = (int)EnumStaticPages.Contact,
                                    },
                            };

            pages.ForEach(c => context.StaticPages.Add(c));
            context.SaveChanges(); //save to database
        }

        private void CreateStateProvinces(WeddingInvitationContext context)
        {
            string dataPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");
            using (var reader = new System.IO.StreamReader(dataPath + "\\StateProvinces.txt"))
            {
                do
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line) == false)
                    {
                        string[] data = line.Split(',');
                        var stateProvince = new StateProvince()
                        {
                            IsDeleted = false,
                            StateProvinceName = data[0].Trim(),
                            StateProvinceCode = data[1].Trim(),
                            MyOfficeId = int.Parse(data[2].Trim())
                        };
                        context.StateProvinces.Add(stateProvince);
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }

            context.SaveChanges(); //save to database
        }
        
        private void CreateMyOffices(WeddingInvitationContext context)
        {
            var myOffices = new List<MyOffice>
                            {
                                new MyOffice
                                    {
                                        IsDeleted =false,
                                        Address = "Tp.Hồ Chí Minh",
                                        OfficeName = "Tp.Hồ Chí Minh",
                                        PhoneNumber = "08.3253.6528",
                                        StorageId = 1
                                    },
                                    new MyOffice
                                    {
                                        IsDeleted =false,
                                        Address = "Khách Lẻ - Tp.Hồ Chí Minh",
                                        OfficeName = "Khách Lẻ - Tp.Hồ Chí Minh",
                                        PhoneNumber = "08.3253.6528",
                                        IsRetailCustomer =true,
                                        StorageId = 1
                                    },
                                new MyOffice
                                    {
                                        IsDeleted =false,
                                        Address = "Hà Nội",
                                        OfficeName = "Hà Nội",
                                        PhoneNumber = "04.3236.3265",
                                        StorageId = 2
                                    },
                                    new MyOffice
                                    {
                                        IsDeleted =false,
                                        Address = "Khách Lẻ - Hà Nội",
                                        OfficeName = "Khách Lẻ - Hà Nội",
                                        PhoneNumber = "04.3236.3265",
                                        IsRetailCustomer = true,
                                        StorageId = 2
                                    },
                            };

            myOffices.ForEach(c => context.MyOffices.Add(c));
            context.SaveChanges(); //save to database
        }


        /// <summary>
        /// Random String
        /// </summary>
        /// <param name="size"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        private static string RandomString(int size, Random random)
        {
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < size; i++)
            {
                //26 letters in the alfabet, ascii + 65 for the capital letters
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
            }
            return builder.ToString();
        }

        private void CreateRoles(WeddingInvitationContext context)
        {
            var roles = new List<Role>
                            {
                                new Role
                                    {
                                        RoleName = "Admin",
                                        Description = "Admin"
                                    },
                                new Role
                                    {
                                        RoleName = "Kinh Doanh",
                                        Description = "Kinh Doanh"
                                    },
                                new Role
                                    {
                                        RoleName = "Ke Toan",
                                        Description = "Ke Toan"
                                    },
                                new Role
                                    {
                                        RoleName = "Bo Phan Giao Hang",
                                        Description = "Bo Phan Giao Hang"
                                    },
                                new Role
                                    {
                                        RoleName = "NV Giao Hang Tinh",
                                        Description = "NV Giao Hang Tinh"
                                    },
                                    new Role
                                    {
                                        RoleName = "Bo Phan Kho",
                                        Description = "Bo Phan Kho"
                                    },
                                    new Role
                                    {
                                        RoleName = "Quan Ly In",
                                        Description = "Quan Ly In"
                                    },
                                    new Role
                                    {
                                        RoleName = "Quan Ly Chung",
                                        Description = "Quan Ly Chung"
                                    },
                                    new Role
                                    {
                                        RoleName = "Duyet Don Hang Giao Trong Ngay",
                                        Description = "Duyet Don Hang Giao Trong Ngay"
                                    },
                            };

            roles.ForEach(c => context.Roles.Add(c));
            context.SaveChanges(); //save to database
        }
    }
    public class RoleType
    {
        public const string Administrator = "Admin";
        public const string KinhDoanh = "Kinh Doanh";
        public const string KeToan = "Ke Toan";
        public const string GiaoHang = "Bo Phan Giao Hang";
        public const string NVGiaoHangTinh = "NV Giao Hang Tinh";
        public const string NVKho = "Bo Phan Kho";
        public const string QuanLyIn = "Quan Ly In";
        public const string QuanLyChung = "Quan Ly Chung";
        public const string DuyetDonHangTrongNgay = "Duyet Don Hang Giao Trong Ngay";
    }

    public class PasswordHelper
    {
        public const int DEFAULT_SALT_SIZE = 16;

        public static string CreatePasswordSalt(int size)
        {
            string result = string.Empty;
            System.Security.Cryptography.RNGCryptoServiceProvider provider = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] bytes = new byte[size];
            provider.GetBytes(bytes);
            result = Convert.ToBase64String(bytes);
            return result;
        }

        public static string CreatePasswordHash(string password, string passwordSalt)
        {
            string result = string.Empty;
            string passwordAndSalt = string.Concat(password, passwordSalt);
            byte[] bytes = System.Text.UnicodeEncoding.Unicode.GetBytes(passwordAndSalt);
            System.Security.Cryptography.SHA1 provider = new System.Security.Cryptography.SHA1Managed();
            bytes = provider.ComputeHash(bytes);
            result = Convert.ToBase64String(bytes);
            return result;
        }
    }
}
