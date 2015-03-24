using System;
using System.Collections.Generic;
using System.Linq;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Users
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsername(string username);
        IQueryable<User> Search(string text);
        IEnumerable<User> GetUsersOnline();
        IEnumerable<User> GetUsersOnline(string text);
        void UpdateLastActivityDate(string username);
        void UpdateLastActivityDate(string username, DateTime lastActivityDate);
        DateTime GetOnlineTimeWindow();
    }
}
