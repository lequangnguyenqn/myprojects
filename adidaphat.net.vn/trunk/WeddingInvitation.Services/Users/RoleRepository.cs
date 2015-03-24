using System.Linq;
using WeddingInvitation.Data.Mapping.Users;
using WeddingInvitation.Services.Infrastructure;

namespace WeddingInvitation.Services.Users
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IQueryable<Role> Search(string text)
        {
            return string.IsNullOrEmpty(text)
                       ? GetAll()
                       : GetAll()
                       .Where(p => (
                        p.RoleName.Contains(text) ||
                        p.Description.Contains(text)));
        }

        public Role GetRoleByName(string roleName)
        {
            return GetAll().Where(p => p.RoleName.ToLower() == roleName.ToLower()).FirstOrDefault();
        }
    }
}
