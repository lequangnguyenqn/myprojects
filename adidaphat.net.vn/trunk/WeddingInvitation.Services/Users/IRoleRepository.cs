using System.Linq;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Users
{
    public interface IRoleRepository : IRepository<Role>
    {
        IQueryable<Role> Search(string text);
        Role GetRoleByName(string roleName);
    }
}
