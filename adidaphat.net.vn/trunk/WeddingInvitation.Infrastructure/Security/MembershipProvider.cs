using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using WeddingInvitation.Data;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Storages;
using WeddingInvitation.Services.Users;
using WeddingInvitation.Infrastructure.Unity;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Services;
using Microsoft.Practices.Unity;
using WeddingInvitation.Services.Settings;

namespace WeddingInvitation.Infrastructure.Security
{
    public sealed class MembershipProvider : System.Web.Security.MembershipProvider
    {
        #region Implement System.Web.Security.MembershipProvider
        private IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMyOfficeRepository _myOfficeRepository;

        public MembershipProvider()
        {
            _userRepository = MvcUnityContainer.Container.Resolve<IUserRepository>();
            _unitOfWork = MvcUnityContainer.Container.Resolve<IUnitOfWork>();
            _myOfficeRepository = MvcUnityContainer.Container.Resolve<IMyOfficeRepository>();
        }

        private string _applicationName;
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        //private bool _enablePasswordReset;
        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        //private bool _enablePasswordRetrieval;
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        //private int _maxInvalidPasswordAttempts;
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        //private int _minRequiredNonAlphanumericCharacters;
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        //private int _minRequiredPasswordLength;
        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        //private int _passwordAttemptWindow;
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        //private MembershipPasswordFormat _passwordFormat;
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        //private string _passwordStrengthRegularExpression;
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        //private bool _requiresUniqueEmail;
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (ValidateUser(username, oldPassword) == false)
            {
                return false;
            }

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, oldPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw args.FailureInformation;
                }
                else
                {
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
                }
            }

            string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
            string hashedPassword = PasswordHelper.CreatePasswordHash(newPassword, passwordSalt);

            User userReturn;
            using (_unitOfWork)
            {
                User user = _userRepository.GetUserByUsername(username);
                user.Password = hashedPassword;
                user.PasswordSalt = passwordSalt;
                userReturn = _userRepository.Update(user);
            }
            return userReturn!=null;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (ValidateUser(username, password) == false)
            {
                return false;
            }

            return false;
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            MembershipUser result = null;
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
            }
            else if (GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
            }
            else
            {
                MembershipUser user = GetUser(username, false);
                if (user == null)
                {
                    string passwordSalt = PasswordHelper.CreatePasswordSalt(PasswordHelper.DEFAULT_SALT_SIZE);
                    string hashedPassword = PasswordHelper.CreatePasswordHash(password, passwordSalt);

                    User userData = new User();
                    //userData.FirstName = passwordQuestion;
                    //userData.LastName = passwordAnswer;
                    userData.Username = username;
                    userData.EmailAddress = email;
                    userData.Password = hashedPassword;
                    userData.PasswordSalt = passwordSalt;
                    userData.IsApproved = isApproved;
                    userData.IsLockedOut = false;
                    userData.IsOnline = false;
                    userData.CreateDate = DateTime.Now;

                    User userReturn;
                    using (_unitOfWork)
                    {
                        userReturn = _userRepository.Insert(userData);
                    }

                    if (userReturn !=null)
                    {
                        status = MembershipCreateStatus.Success;
                    }
                    else
                    {
                        status = MembershipCreateStatus.UserRejected;
                    }

                    result = GetUser(username, false);
                }
                else
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                }
            }

            return result;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            //int rowsAffected = 0;
            User user = _userRepository.GetUserByUsername(username);
            //User userReturn;
            using (_unitOfWork)
            {
                _userRepository.Delete(user);
            }

            if (deleteAllRelatedData == true)
            {
                // Process commands to delete all data for the user in the database.
            }

            return true;
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            return _userRepository.GetUsersOnline().Count();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (userIsOnline == true)
            {
                _userRepository.UpdateLastActivityDate(username);
            }
            User user = _userRepository.GetAll().Where(p => p.Username.ToLower() == username.ToLower()).FirstOrDefault();
            return GetUserFrom(user);
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            User user = _userRepository.GetAll().Where(p => p.IsOnline == userIsOnline).FirstOrDefault();
            return GetUserFrom(user);
        }

        public override string GetUserNameByEmail(string email)
        {
            User user = _userRepository.GetAll().Where(p => p.EmailAddress.ToLower() == email.ToLower()).FirstOrDefault();
            if (user == null)
            {
                return string.Empty;
            }
            return user.Username;
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            _userRepository.UpdateLastActivityDate(user.UserName, user.LastActivityDate);
        }

        public override bool ValidateUser(string username, string password)
        {
            _userRepository = MvcUnityContainer.Container.Resolve<IUserRepository>();
            var officeRepository = MvcUnityContainer.Container.Resolve<IMyOfficeRepository>();
            var storageRepository = MvcUnityContainer.Container.Resolve<IStorageRepository>();
            var userData = _userRepository.GetAll().FirstOrDefault(t => t.EmailAddress.ToLower() == username.ToLower());
            if (userData == null)
            {
                return false;
            }

            if (userData.IsDeleted == true ||  userData.IsApproved == false || userData.IsLockedOut == true 
                || (userData.AllowLoginFrom.HasValue && userData.AllowLoginFrom.Value.TimeOfDay > DateTime.Now.TimeOfDay )
                || (userData.AllowLoginTo.HasValue && userData.AllowLoginTo.Value.TimeOfDay < DateTime.Now.TimeOfDay))
            {
                return false;
            }

            string hashedPassword = PasswordHelper.CreatePasswordHash(password, userData.PasswordSalt);
            bool result = userData.Password.ToLower() == hashedPassword.ToLower();
            if (result == true)
            {
                WorkContext.FullName = string.Format("{0} {1}", userData.FirstName, userData.LastName);
                WorkContext.CurrentUserId = userData.UserId;
                var rolesArray = (from p in userData.Roles
                        select p.RoleName).ToArray();
                //Check permision
                if (rolesArray.Contains(RoleType.Administrator))
                {
                    WorkContext.MyOffices = officeRepository.Search("").Select(p => p.MyOfficeId).ToArray();
                    WorkContext.MyStorages = storageRepository.Search("").Select(p => p.StorageId).ToArray();
                }
                else
                {
                    WorkContext.MyOffices = userData.MyOffices.Select(p => p.MyOfficeId).ToArray();
                    WorkContext.MyStorages = userData.Storages.Select(p => p.StorageId).ToArray();
                }
                userData.LastLogInDate = DateTime.Now;
                _userRepository.UpdateLastActivityDate(username, (DateTime)userData.LastLogInDate);
            }
            return result;
        }

        #endregion      

        private MembershipUser GetUserFrom(User user)
        {
            if (user != null)
            {
                object providerUserKey = System.Guid.Empty;
                string username = user.Username;
                string email = user.EmailAddress;
                string passwordQuestion = string.Empty;
                string comment = string.Empty;
                bool isApproved = user.IsApproved;
                bool isLockedOut = user.IsLockedOut;
                DateTime createDate = user.CreateDate ?? DateTime.MinValue;
                DateTime lastLoginDate = user.LastLogInDate ?? DateTime.MinValue;
                DateTime lastActivityDate = user.LastLogInDate ?? DateTime.MinValue;
                DateTime lastPasswordChangedDate = user.LastPasswordChangedDate ?? DateTime.MinValue;
                DateTime lastLockedOutDate = user.LastLockedOutDate ?? DateTime.MinValue;

                return new MembershipUser(this.Name, username, providerUserKey, email, passwordQuestion, comment, isApproved,
                    isLockedOut, createDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockedOutDate);
            }
            return null;
        }
    }
}
