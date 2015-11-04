using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeddingInvitation.Services.Infrastructure;
using WeddingInvitation.Services.Users;
using WeddingInvitation.Infrastructure.Unity;
using WeddingInvitation.Core.Models.Users;
using WeddingInvitation.Services;
using WeddingInvitation.Data.Mapping.Users;
using Microsoft.Practices.Unity;

namespace WeddingInvitation.Infrastructure.Security
{
    public sealed class RoleProvider : System.Web.Security.RoleProvider
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private string _applicationName;
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public RoleProvider()
        {
            _roleRepository = MvcUnityContainer.Container.Resolve<IRoleRepository>();
            _userRepository = MvcUnityContainer.Container.Resolve<IUserRepository>();
            _unitOfWork = MvcUnityContainer.Container.Resolve<IUnitOfWork>();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (_unitOfWork)
            {
                foreach (string username in usernames)
                {
                    User user = _userRepository.GetUserByUsername(username);
                    foreach (string roleName in roleNames)
                    {
                        if (RoleExists(roleName) == false)
                        {
                            throw new Exception("Role name doesn't exist");
                        }
                        user.Roles.Add(_roleRepository.GetRoleByName(roleName));
                    }
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            if (roleName.Contains(","))
            {
                throw new Exception("Role names cannot contain commas");
            }

            if (RoleExists(roleName) == true)
            {
                throw new Exception("Role name already exists");
            }

            Role role = new Role() { RoleName = roleName };
            using (_unitOfWork)
            {
                _roleRepository.Insert(role);
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool result = false;
            if (RoleExists(roleName) == false)
            {
                throw new Exception("Role does not exist");
            }

            if (throwOnPopulatedRole && GetUsersInRole(roleName).Length > 0)
            {
                throw new Exception("Can not delete a populated role");
            }

            Role role = _roleRepository.GetRoleByName(roleName);
            if (role != null)
            {
                using (_unitOfWork)
                {
                    _roleRepository.Delete(role);
                    result = true;
                }
            }

            return result;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            Role role = _roleRepository.GetRoleByName(roleName);
            return (from p in role.Users
                    where p.Username.ToLower() == usernameToMatch.ToLower()
                    select p.Username).ToArray();
        }

        public override string[] GetAllRoles()
        {
            var result = from p in _roleRepository.GetAll()
                         select p.RoleName;
            
            return result.ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            User user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                return (from p in user.Roles
                        select p.RoleName).ToArray();
            }
            return new string[] {};
        }

        public override string[] GetUsersInRole(string roleName)
        {
            Role role = _roleRepository.GetRoleByName(roleName);
            if (role != null)
            {
                return (from p in role.Users
                        select p.Username).ToArray();
            }
            return new string[] {};
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool result = false;
            Role role = _roleRepository.GetRoleByName(roleName);
            if (role != null)
            {
                result = role.Users.Where(p => p.Username.ToLower() == username).FirstOrDefault() != null;
            }
            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (_unitOfWork)
            {
                foreach (string username in usernames)
                {
                    User user = _userRepository.GetUserByUsername(username);
                    foreach (string roleName in roleNames)
                    {
                        var roles = user.Roles.Where(p => p.RoleName.ToLower() == roleName.ToLower()).ToList();
                        foreach (var role in roles)
                        {
                            user.Roles.Remove(role);
                        }
                    }
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            return _roleRepository.GetRoleByName(roleName) != null;
        }
    }
}
