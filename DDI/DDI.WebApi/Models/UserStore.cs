using DDI.Services;
using DDI.Shared.Models.Client.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DDI.WebApi.Providers
{
    //UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>
    /*: IUserLoginStore<TUser, TKey>, 
    IUserClaimStore<TUser, TKey>, 
    IUserRoleStore<TUser, TKey>, 
    IUserPasswordStore<TUser, TKey>, 
    IUserSecurityStampStore<TUser, TKey>, 
    IQueryableUserStore<TUser, TKey>, 
    IUserEmailStore<TUser, TKey>, 
    IUserPhoneNumberStore<TUser, TKey>, 
    IUserTwoFactorStore<TUser, TKey>, 
    IUserLockoutStore<TUser, TKey>, 
    IUserStore<TUser, TKey>, IDisposable */
    public class UserStore : IUserStore<User, Guid>,
                             IUserRoleStore<User, Guid>
    {
        private ServiceBase<User> _userService;
        private ServiceBase<Role> _roleService;

        public UserStore() { }

        public UserStore(ServiceBase<User> service)
        {
            this._userService = service;
        } 

        public Task CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _userService.Add(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _userService.Delete(user);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {

        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            var response = _userService.GetById(userId);
            return Task.FromResult<User>(response.Data);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var response = _userService.GetWhereExpression(u => u.UserName == userName);
            return Task.FromResult<User>(response.Data);
        }

        public Task UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _userService.Update(user);

            return Task.FromResult<object>(null);
        }

        /* UserRoles */
        public Task AddToRoleAsync(User user, string roleName)
        {
            var role = _roleService.GetWhereExpression(r => r.Name == roleName).Data;

            if (role == null)
                throw new InvalidOperationException($"Role: {roleName} does not exist.");

            var userRole = new UserRole() { UserId = user.Id, RoleId = role.Id };
            user.Roles.Add(userRole);
            _userService.Update(user);
            
            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }
        /* UserRoles */
       
    }
}