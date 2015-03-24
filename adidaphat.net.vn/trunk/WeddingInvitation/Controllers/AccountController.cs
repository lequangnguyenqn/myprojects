using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Data;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Infrastructure;
using WeddingInvitation.Infrastructure.Constant;
using WeddingInvitation.Infrastructure.Mvc;
using WeddingInvitation.Infrastructure.Security;
using WeddingInvitation.Infrastructure.Services;
using WeddingInvitation.Infrastructure.Unity;
using WeddingInvitation.Models;
using WeddingInvitation.Services;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Users;
using PasswordHelper = WeddingInvitation.Infrastructure.Security.PasswordHelper;

namespace WeddingInvitation.Controllers
{
    public class AccountController : ControllerBase<IUserRepository, User>
    {
        #region Fields

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMailServerService _mailServerService;

        #endregion

        #region Constructor

        public AccountController(IUserRepository repository, IUnitOfWork unitOfWork,
                                 IRoleRepository roleRepository,IMailServerService mailServerService)
            : base(repository, unitOfWork)
        {
            _userRepository = repository;
            _roleRepository = roleRepository;
            _mailServerService = mailServerService;
        }

        #endregion

        #region Admin Actions

        /// <summary>
        /// Edit Account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AccountDetails()
        {
            if (WorkContext.CurrentUserId == 0)
                return RedirectToAction("LogOff", "Account", new { ReturnUrl = Request.RawUrl });
            var entity = _userRepository.GetById(WorkContext.CurrentUserId);
            var model = new AccountDetailsModel
            {
                UserId = entity.UserId,
                Address1 = entity.Address1,
                Address2 = entity.Address2,
                CompanyName = entity.CompanyName,
                ContactName = entity.FirstName,
                ContactNumber = entity.ContactNumber,
                CountryRegion = entity.CountryRegion,
                EmailAddress = entity.EmailAddress,
                PostCodeZip = entity.PostCodeZip,
                TownCity = entity.TownCity,
                Password = entity.Password,
                ConfirmPassword = entity.Password,
            };
            //Get list country
            var listCountry = CountryList.GetCountryList("US");
            ViewBag.ListCountry = listCountry;
            return View(model);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult AccountDetails(AccountDetailsModel userModel)
        {
            if (WorkContext.CurrentUserId == 0)
                return RedirectToAction("LogOff", "Account", new { ReturnUrl = Request.RawUrl });
            if (ModelState.IsValid && userModel.UserId > 0)
            {
                var entityUpdate = _userRepository.GetById(userModel.UserId);
                entityUpdate.Address1 = userModel.Address1;
                entityUpdate.Address2 = userModel.Address2;
                entityUpdate.FirstName = userModel.ContactName;
                entityUpdate.ContactNumber = userModel.ContactNumber;
                entityUpdate.CountryRegion = userModel.CountryRegion;
                entityUpdate.EmailAddress = userModel.EmailAddress;
                entityUpdate.PostCodeZip = userModel.PostCodeZip;
                entityUpdate.TownCity = userModel.TownCity;
                using (UnitOfWork)
                {
                    var modelEdit = _userRepository.Update(entityUpdate);
                }
            }
            //Get list country
            var listCountry = CountryList.GetCountryList("US");
            ViewBag.ListCountry = listCountry;

            return View(userModel);
        }
        
        //
        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            if (WorkContext.CurrentUserId == 0)
                return RedirectToAction("LogOff", "Account", new { ReturnUrl = Request.RawUrl });
            var entity = _userRepository.GetById(WorkContext.CurrentUserId);
            var model = new ChangePasswordModel { NeedChangePassWord = entity.NeedChangePassWord};
            ViewBag.Success = "";
            return View(model);
        }

        //
        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (WorkContext.CurrentUserId == 0)
                return RedirectToAction("LogOff", "Account", new { ReturnUrl = Request.RawUrl });
            if (ModelState.IsValid)
            {
                var entity = _userRepository.GetById(WorkContext.CurrentUserId);
                string hashedOldPassword = PasswordHelper.CreatePasswordHash(model.OldPassword, entity.PasswordSalt);
                if (hashedOldPassword.ToLower() != entity.Password.ToLower())
                {
                    ModelState.AddModelError("", "Current Password is incorrect");
                    return View(model);
                }
                string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
                string hashedPassword = PasswordHelper.CreatePasswordHash(model.NewPassword, passwordSalt);
                entity.Password = hashedPassword;
                entity.PasswordSalt = passwordSalt;
                entity.NeedChangePassWord = false;
                using (UnitOfWork)
                {
                    _userRepository.Update(entity);
                }
            }
            ViewBag.Success = "Password changed successfully";
            return View(model);
        }

        [Authorize]
        public ActionResult CompleteFromPaypal()
        {
            return Content("Complete");
        }

        [Authorize]
        public ActionResult NotifyFromPaypal()
        {
            return Content("NotifyFromPaypal");
        }
        #endregion

        #region Common actions

        public ActionResult ForgotPassWord()
        {
            return View(new ForgotPasswordModel
            {
                Email = ""
            });
        }

        [HttpPost]
        public ActionResult ForgotPassWord(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                var strError = "";
                //var lstError = new List<ErrorViewModel>();
                var Keys = ModelState.Keys.ToList();
                var Values = ModelState.Values.ToList();
                //Collect all errors
                for (int i = 0; i < Keys.Count; i++)
                {
                    if (Values[i].Errors != null && Values[i].Errors.Count > 0)
                    {
                        strError += Values[i].Errors.FirstOrDefault().ErrorMessage;
                        break;
                    }
                }
                return Json(strError);
            }
            else
            {
                var newPassWord = StringHelper.GenerateRandomString(6, new Random());
                //Update User
                var user = _userRepository.GetUserByUsername(model.Email);
                if (user != null)
                {
                    using (UnitOfWork)
                    {
                        string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
                        string hashedPassword = PasswordHelper.CreatePasswordHash(newPassWord, passwordSalt);
                        user.Password = hashedPassword;
                        user.PasswordSalt = passwordSalt;
                        user.NeedChangePassWord = true;
                        user.NeedRemindChangePassword = true;
                        _userRepository.Update(user);
                    }
                }
                else
                {
                    return Json("This user not existed in system.");
                }
                //Send Mail
                // 0: New password
                // 1: Url login
                // 2: Url image sign in
                // 3: Url host
                string mess = "";
                string dataPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/EmailTemplates");
                IniFile ini = new IniFile(dataPath + "\\SendMailForgotPassWord.ini");
                var title = ini.GetStringValue("Title");
                var subject = ini.GetStringValue("Subject");
                var content = ini.GetStringValue("Content");

                object[] obj = new object[5];
                obj[0] = newPassWord;
                obj[1] = WebHelpers.GetUrlHost();
                obj[2] = WebHelpers.GetUrlHost() + "/Content/images/SignIn.png";
                obj[3] = WebHelpers.GetUrlHost();
                _mailServerService.SendByServer(model.Email, subject, string.Format(content, obj), null, ref mess);
            }
            return Json("true");
        }

        //
        // GET: /Account/LogOn
        public ActionResult LogOn(string ReturnUrl)
        {
            return View(new LogOnModel
            {
                UserName = "",
                Password = "",
                RememberMe = false,
            });
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult Logon(LogOnModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName.Trim(), model.Password) == true)
                {
                    //Save cookie
                    SaveCookie(model.UserName.Trim(), model.Password, model.RememberMe);
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var user = _userRepository.GetById(WorkContext.CurrentUserId);
                    if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                        && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                    {
                        var rolesArray = Roles.GetRolesForUser();
                        WorkContext.Roles = rolesArray;
                        //Check permision
                        if (rolesArray.Contains(RoleType.Administrator))
                        {
                            WorkContext.IsSuperAdmin = true;
                        }
                        else
                        {
                            WorkContext.IsSuperAdmin = false;
                        }
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        var rolesArray = Roles.GetRolesForUser();
                        WorkContext.Roles = rolesArray;
                        //Check permision
                        if (rolesArray.Contains(RoleType.Administrator))
                        {
                            WorkContext.IsSuperAdmin = true;
                        }
                        else
                        {
                            WorkContext.IsSuperAdmin = false;
                        }
                        return RedirectToAction("Index", "Home", new { area = "Administrator" });
                    }
                }
                else
                {
                    var user = _userRepository.GetAll().FirstOrDefault(t => t.EmailAddress.ToLower() == model.UserName.ToLower());
                    if (user != null && user.IsLockedOut == true)
                    {
                        ModelState.AddModelError("", "Your account has been disabled, please contact administrator.");
                        return Json(new
                        {
                            resultcode = false,
                            resulttext = "Your account has been disabled, please contact administrator at brierlytom@gmail.com"
                        });
                    }
                    else
                    {
                        ModelState.AddModelError("", "User name hoặc password chưa đúng.");
                    }
                }
            }
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff(string returnUrl)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            if (!string.IsNullOrEmpty(returnUrl))
                RedirectToAction("LogOn", "Account", new { ReturnUrl = returnUrl });
            return RedirectToAction("LogOn", "Account");
        }

        //
        // GET: /Account/Register
        public ActionResult Register(string hashEmail)
        {
            var model = new AccountDetailsModel { EmailAddress = "", Password = "" };

            //Get list country
            var listCountry = CountryList.GetCountryList("default");
            ViewBag.ListCountry = listCountry;
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(AccountDetailsModel model)
        {
            if (!ModelState.IsValid)
                return View();
            //Insert new user
            string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
            string hashedPassword = PasswordHelper.CreatePasswordHash(model.Password, passwordSalt);
            var userEntity = new User
            {
                Address1 = model.Address1,
                Address2 = model.Address2,
                FirstName = model.ContactName,
                ContactNumber = model.ContactNumber,
                CountryRegion = model.CountryRegion,
                EmailAddress = model.EmailAddress,
                PostCodeZip = model.PostCodeZip,
                TownCity = model.TownCity,
                Username = model.EmailAddress,
                Password = hashedPassword,
                PasswordSalt = passwordSalt,
                IsApproved = true,
                IsLockedOut = false,
                IsOnline = false,
                CreateDate = DateTime.Now,
                Roles = new List<Role>(),
            };
            
            //Add Admin role
            userEntity.Roles.Add(_roleRepository.GetAll().Where(p => p.RoleName == RoleType.KinhDoanh).FirstOrDefault());
            //Add User
            using (UnitOfWork)
            {
                _userRepository.Insert(userEntity);
            }

            return RedirectToAction("LogOn", "Account");
        }

        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult AccountActivation(string hashEmail)
        {
            var email = Escaping.FromBase64String(hashEmail);
            var user = _userRepository.GetAll().Where(p => p.EmailAddress.ToLower() == email.ToLower()).FirstOrDefault();
            if (user != null)
                return RedirectToAction("LogOff", "Account", new { ReturnUrl = Request.RawUrl });
            else
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Invite", "Account", new { hashEmail = hashEmail });
            }
        }

        #endregion

        #region External Services

        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public string Upload(HttpPostedFileBase fileData)
        {
            //Resize and crop image
            var imageOgirinal = Image.FromStream(fileData.InputStream, true, true);
            var imageResize = ImageHelpers.HardResizeImage(230, 98, imageOgirinal);
            //Save image
            var fileName = this.Server.MapPath("~/Uploads/" + System.IO.Path.GetFileName(fileData.FileName));
            imageResize.Save(fileName);
            return "ok";
        }

        /// <summary>
        /// Upload UserImage
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public string UploadUserImage(HttpPostedFileBase fileData)
        {
            //Resize and crop image
            var imageOgirinal = Image.FromStream(fileData.InputStream, true, true);
            var imageResize = ImageHelpers.HardResizeImage(100, 100, imageOgirinal);
            //Save image
            var fileName = this.Server.MapPath("~/Uploads/UserImages/" + System.IO.Path.GetFileName(fileData.FileName));
            imageResize.Save(fileName);
            return "ok";
        }

        /// <summary>
        /// Render table colors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FetchColors()
        {
            var count = 0;
            var sb = new StringBuilder();
            sb.Append("<table><tbody><tr>");

            foreach (var color in Enum.GetNames(typeof(KnownColor)))
            {
                var colorValue = ColorTranslator.FromHtml(color);
                var html = string.Format("#{0:X2}{1:X2}{2:X2}",
                                    colorValue.R, colorValue.G, colorValue.B);
                sb.AppendFormat("<td bgcolor=\"{0}\">&nbsp;</td>", html);
                if (count < 20)
                {
                    count++;
                }
                else
                {
                    sb.Append("</tr><tr>");
                    count = 0;
                }
            }
            sb.Append("</tbody></table>");
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Check exists email
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult IsEmailAvailable(string EmailAddress, int UserId)
        {
            var userData = _userRepository.GetAll().FirstOrDefault(t => t.EmailAddress.ToLower() == EmailAddress.ToLower());
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
                    var user = _userRepository.GetById(UserId);
                    if (userData.EmailAddress == user.EmailAddress)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region Private methods

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        private void SaveCookie(string email, string password, bool rememberMe)
        {
            try
            {
                if (Membership.ValidateUser(email, password) == true)
                {
                    FormsAuthenticationTicket ticket = null;
                    if (rememberMe == true)
                    {
                        ticket = new FormsAuthenticationTicket(1, email, DateTime.Now, DateTime.Now.AddYears(1),
                           rememberMe, email, FormsAuthentication.FormsCookiePath);
                    }
                    else
                    {
                        ticket = new FormsAuthenticationTicket(1, email, DateTime.Now,
                            DateTime.Now.AddMinutes(this.Session.Timeout),
                            rememberMe, email, FormsAuthentication.FormsCookiePath);
                    }

                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    // Create an Identity object
                    FormsIdentity id = new FormsIdentity(ticket);

                    // This principal will flow throughout the request.
                    var roles = System.Web.Security.Roles.GetRolesForUser(email);
                    GenericPrincipal principal = new GenericPrincipal(id, roles);

                    // Attach the new principal object to the current HttpContext object
                    System.Web.HttpContext.Current.User = principal;
                }
            }
            catch (Exception ex)
            {
                Session["loginError"] = "error";
            }
        }

        #endregion

    }
}
