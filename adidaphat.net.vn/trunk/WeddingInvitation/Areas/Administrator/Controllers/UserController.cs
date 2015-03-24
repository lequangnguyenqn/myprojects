using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using WeddingInvitation.Areas.Administrator.Models;
using WeddingInvitation.Core.Models.Settings;
using WeddingInvitation.Core.Models.Storages;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Data;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Settings;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;
using PasswordHelper = WeddingInvitation.Infrastructure.Security.PasswordHelper;

namespace WeddingInvitation.Areas.Administrator.Controllers
{
    public class UserController : ControllerBase<IUserRepository, User>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMyOfficeRepository _myOfficeRepository;
        private readonly IStorageRepository _storageRepository;

        public UserController(IUnitOfWork unitOfWork,IUserRepository repository,
                              IRoleRepository roleRepository, IMyOfficeRepository myOfficeRepository,
                              IStorageRepository storageRepository)
            : base(repository, unitOfWork)
        {
            _roleRepository = roleRepository;
            _myOfficeRepository = myOfficeRepository;
            _storageRepository = storageRepository;
        }

        //
        // GET: /Administrator/User/
        [SuperAdminAuthorize]
        public ActionResult Index()
        {
            return View();
        }


        [GridAction]
        public ActionResult GridModel(string search)
        {
            var user = Repository.Search(search);

            var gridModel = new GridModel<UserModel>
            {
                Data = user.Select(x => new UserModel
                                            {
                                                UserId = x.UserId,
                                                Email = x.EmailAddress,
                                                FirstName = x.FirstName,
                                                LastName = x.LastName,
                                                IsLockedOut = x.IsLockedOut,
                                                CreateDate = x.CreateDate.Value,
                                                RoleName = x.Roles.FirstOrDefault().RoleName
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
            var listRole = _roleRepository.Search("").ToList();
            listRole = listRole.Where(p => p.RoleId != 9).ToList();
            var listOffice = _myOfficeRepository.Search(string.Empty).ToList();
            var listStorage = _storageRepository.Search(string.Empty).ToList();
            var userModel = new UserModel()
            {
                AvailableUserRoles = listRole,
                AvailableOffices = listOffice,
                AvailableStorages = listStorage,
                IsLockedOut = true,
                AllowLoginFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0),
                AllowLoginTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0),
            };

            return View(userModel);
        }

        [SuperAdminAuthorize]
        public virtual ActionResult Edit(int id)
        {
            var listRole = _roleRepository.Search("").ToList();
            listRole = listRole.Where(p => p.RoleId != 9).ToList();
            var listOffice = _myOfficeRepository.Search(string.Empty).ToList();
            var listStorage = _storageRepository.Search(string.Empty).ToList();
            var user = Repository.GetById(id);
            var userModel = new UserModel()
            {
                AvailableUserRoles = listRole,
                AvailableOffices = listOffice,
                AvailableStorages = listStorage,
                UserId = user.UserId,
                Email = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsLockedOut = !user.IsLockedOut,
                RoleId  = user.Roles.FirstOrDefault().RoleId,
                Password ="123456",
                ConfirmPassword ="123456",
                BelongOffices = user.MyOffices.Select(p => p.MyOfficeId).ToList(),
                BelongStorages = user.Storages.Select(p => p.StorageId).ToList(),
                AllowLoginFrom =user.AllowLoginFrom,
                AllowLoginTo = user.AllowLoginTo,
                Salary = user.Salary,
                DeliveryInDay = user.Roles.Any(p => p.RoleName == RoleType.DuyetDonHangTrongNgay)
            };
            return View("Edit", userModel);
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Save(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                var listRole = _roleRepository.GetAll().ToList();
                listRole = listRole.Where(p => p.RoleId != 9).ToList();
                var listOffice = _myOfficeRepository.Search(string.Empty).ToList();
                var listStorage = _storageRepository.Search(string.Empty).ToList();
                userModel.AvailableOffices = listOffice;
                userModel.AvailableUserRoles = listRole;
                userModel.AvailableStorages = listStorage;
                return View("Create", userModel);
            }
            //Check existed
            var modelExisted = Repository.GetAll().FirstOrDefault(p => p.EmailAddress == userModel.Email);
            if ((modelExisted != null && modelExisted.UserId != userModel.UserId) || (modelExisted != null && userModel.UserId <= 0))
            {
                var listRole = _roleRepository.GetAll().ToList();
                listRole = listRole.Where(p => p.RoleId != 9).ToList();
                userModel.AvailableUserRoles = listRole;
                var listOffice = _myOfficeRepository.Search(string.Empty).ToList();
                userModel.AvailableOffices = listOffice;
                var listStorage = _storageRepository.Search(string.Empty).ToList();
                userModel.AvailableStorages = listStorage;
                this.SetErrorNotification("Email này đã tồn tại trong hệ thống.");
                return View("Create", userModel);
            }
            string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
            if (userModel.UserId <= 0) //Create User
            {
                var user = new User()
                {
                    EmailAddress = userModel.Email,
                    Username = userModel.Email,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Password = PasswordHelper.CreatePasswordHash(userModel.Password, passwordSalt),
                    IsApproved = true,
                    IsLockedOut = !userModel.IsLockedOut,
                    CreateDate = DateTime.UtcNow,
                    LastActivityDate = DateTime.UtcNow,
                    PasswordSalt = passwordSalt,
                    Roles = new List<Role>(),
                    MyOffices = new List<MyOffice>(),
                    Storages = new List<Storage>(),
                    AllowLoginFrom = userModel.AllowLoginFrom,
                    AllowLoginTo = userModel.AllowLoginTo,
                    Salary = userModel.Salary
                };
                foreach (var belongOffice in userModel.BelongOffices)
                {
                    user.MyOffices.Add(_myOfficeRepository.GetById(belongOffice));
                }
                foreach (var storage in userModel.BelongStorages)
                {
                    user.Storages.Add(_storageRepository.GetById(storage));
                }
                
                user.Roles.Add(_roleRepository.GetById(userModel.RoleId));
                if (userModel.DeliveryInDay)
                {
                    user.Roles.Add(_roleRepository.GetAll().FirstOrDefault(p => p.RoleName == RoleType.DuyetDonHangTrongNgay));
                }
                using (UnitOfWork)
                {
                    Repository.Insert(user);
                }
            }
            else //Edit user
            {
                var userEdit = Repository.GetAll().Where(p => p.UserId == userModel.UserId).Include(p => p.Storages).Include(p => p.MyOffices).FirstOrDefault();
                userEdit.EmailAddress = userModel.Email;
                userEdit.Username = userModel.Email;
                userEdit.FirstName = userModel.FirstName;
                userEdit.LastName = userModel.LastName;
                userEdit.IsLockedOut = !userModel.IsLockedOut;
                userEdit.AllowLoginFrom = userModel.AllowLoginFrom;
                userEdit.AllowLoginTo = userModel.AllowLoginTo;
                userEdit.Salary = userModel.Salary;
                if (userEdit.Roles.FirstOrDefault().RoleId != userModel.RoleId)
                {
                    userEdit.Roles = new List<Role>();
                    userEdit.Roles.Add(_roleRepository.GetById(userModel.RoleId));
                }
                if (userModel.DeliveryInDay)
                {
                    userEdit.Roles.Add(_roleRepository.GetAll().FirstOrDefault(p => p.RoleName == RoleType.DuyetDonHangTrongNgay));
                }
                userEdit.MyOffices.Clear();
                foreach (var belongOffice in userModel.BelongOffices)
                {
                    userEdit.MyOffices.Add(_myOfficeRepository.GetById(belongOffice));
                }
                userEdit.Storages.Clear();
                foreach (var storage in userModel.BelongStorages)
                {
                    userEdit.Storages.Add(_storageRepository.GetById(storage));
                }
                using (UnitOfWork)
                {
                    Repository.Update(userEdit);
                }
            }

            //Save success
            this.SetSuccessNotification(string.Format("{0} đã được lưu thành công.", "Nhân viên"));
            return RedirectToAction("Index", new { area = "Administrator" });
        }

        /// <summary>
        /// Check exists email
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult IsEmailAvailable(string Email, int UserId)
        {
            var userData = Repository.GetAll().FirstOrDefault(t => t.EmailAddress.ToLower() == Email.ToLower());
            if (UserId <= 0) //Case Add
            {
                if (userData == null)
                    return Json(true, JsonRequestBehavior.AllowGet);
            }
            else //case Edit
            {
                if (userData == null)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                {
                    var user = Repository.GetById(UserId);
                    if (userData.EmailAddress == user.EmailAddress)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword(int id)
        {
            var entity = Repository.GetById(id);
            var model = new ChangePasswordModel { UserId = id};
            return View(model);
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = Repository.GetById(model.UserId);
                //string hashedOldPassword = PasswordHelper.CreatePasswordHash(model.OldPassword, entity.PasswordSalt);
                //if (hashedOldPassword.ToLower() != entity.Password.ToLower())
                //{
                //    ModelState.AddModelError("", "Current Password is incorrect");
                //    return View(model);
                //}
                string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
                string hashedPassword = PasswordHelper.CreatePasswordHash(model.NewPassword, passwordSalt);
                entity.Password = hashedPassword;
                entity.PasswordSalt = passwordSalt;
                using (UnitOfWork)
                {
                    Repository.Update(entity);
                }
            }
            //Save success
            this.SetSuccessNotification("Mật khẩu đã được đổi thành công.");
            return View(model);
        }

        [SuperAdminAuthorize]
        public ActionResult Delete(int id)
        {
            try
            {
                using (UnitOfWork)
                {
                    Repository.Delete(id);
                }
                this.SetSuccessNotification("Nhân viên đã được xóa thành công.");
            }
            catch
            {
                this.SetErrorNotification("Nhân viên này không thể xóa, vì đã được sử dụng!!");
            }
            return RedirectToAction("index", new { area = "Administrator" });
        }
    }
}
