
namespace WeddingInvitation.Infrastructure.Security
{
    public interface IPermissionService
    {
        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string roleName);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roles"></param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string roleName, string[] roles);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="roleNames"></param>
        /// <param name="roles"></param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string[] roleNames, string[] roles);
    }
}
