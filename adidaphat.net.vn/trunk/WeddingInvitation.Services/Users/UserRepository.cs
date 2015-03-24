using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Users
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IQueryable<User> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                                ? GetAll().Where(p => p.IsDeleted == false).OrderByDescending(p => p.UserId)
                                : GetAll()
                                .Where(p => p.IsDeleted == false && (
                                 p.FirstName.Contains(text) || p.LastName.Contains(text) || p.EmailAddress.Contains(text))).OrderByDescending(p => p.UserId);
        }

        public User GetUserByUsername(string username)
        {
            return GetAll().Where(p => p.Username.ToLower() == username.ToLower() || p.EmailAddress.ToLower() == username.ToLower()).Include(p => p.Roles).FirstOrDefault();
        }

        public IEnumerable<User> GetUsersOnline()
        {
            DateTime compareTime = GetOnlineTimeWindow();
            return GetAll().Where(p => p.LastActivityDate > compareTime).AsEnumerable();
        }

        public IEnumerable<User> GetUsersOnline(string text)
        {
            DateTime compareTime = GetOnlineTimeWindow();
            return string.IsNullOrEmpty(text)
                   ? GetAll().Where(p => p.LastActivityDate > compareTime).AsEnumerable()
                   : GetAll().Where(p => p.LastActivityDate > compareTime && (p.Address1.Contains(text) || p.FirstName.Contains(text))).AsEnumerable();
        }

        public void UpdateLastActivityDate(string username)
        {
            UpdateLastActivityDate(username, DateTime.Now);
        }

        public void UpdateLastActivityDate(string username, DateTime lastActivityDate)
        {
            using (_unitOfWork)
            {
                User user = this.GetUserByUsername(username);
                if (user != null)
                {
                    DateTime compareTime = GetOnlineTimeWindow();
                    user.LastActivityDate = lastActivityDate;
                    user.IsOnline = user.LastActivityDate > compareTime;
                }
            }
        }

        public DateTime GetOnlineTimeWindow()
        {
            //TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.HttpContext.Current.Session.Timeout, 0);
            return DateTime.Now.Subtract(onlineSpan);
        }
    }
}
