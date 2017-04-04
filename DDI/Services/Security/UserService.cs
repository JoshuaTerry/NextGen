using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Security
{
    public class UserService : ServiceBase<User>, IUserStore<User, Guid>,
                                                  IUserRoleStore<User, Guid>,
                                                  IUserEmailStore<User, Guid>,
                                                  IUserPasswordStore<User, Guid>,
                                                  IQueryableUserStore<User, Guid>
    {
        private readonly IUnitOfWork _uow; 
        public UserService()
        {
            _uow = new UnitOfWorkEF();
        }

        public IDataResponse AddDefaultBusinessUnitToUser(Guid id, Guid defaultbuid)
        {
            var result = _uow.GetById<User>(id);
            var defaultbu = _uow.GetById<BusinessUnit>(defaultbuid);

            if (!result.BusinessUnits.Contains(defaultbu))
            {
                result.BusinessUnits.Add(defaultbu);
            }

            result.DefaultBusinessUnitId = defaultbuid;

            _uow.SaveChanges();

            DataResponse<User> response = new DataResponse<User>
            {
                Data = result,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse AddBusinessUnitToUser(Guid id, Guid buid)
        {
            var result = _uow.GetById<User>(id);
            var bu = _uow.GetById<BusinessUnit>(buid);

            if (!result.BusinessUnits.Contains(bu))
            {
                result.BusinessUnits.Add(bu);
                _uow.SaveChanges();
            }

            IDataResponse response = new DataResponse<User>
            {
                Data = result,
                IsSuccessful = true
            };

            return response;
        }

        private void CheckUser(User user)
        {
            if (user == null)
            {                
                throw new ArgumentNullException("User");
            }
        }

        private Role GetRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("RoleName can not be empty.");

            var role = _uow.GetRepository<Role>().Entities.FirstOrDefault(r => r.Name == roleName);

            if (role == null)
                throw new ArgumentNullException($"No role with the name {roleName} was found.");

            return role;
        }

        private List<Role> GetRoles(User user)
        {
            CheckUser(user);
            var roles = _uow.GetRepository<Role>().Entities.Where(r => user.Roles.Select(ur => ur.RoleId).Contains(r.Id)).ToList();
            return roles;
        }

        #region IQueryableUserStore Implementation
        IQueryable<User> IQueryableUserStore<User, Guid>.Users
        {
            get
            {
                return _uow.GetRepository<User>().Entities;
            }
        }
        #endregion

        #region IUserRoleStore Implementation

        Task IUserRoleStore<User, Guid>.AddToRoleAsync(User user, string roleName)
        {
            CheckUser(user);

            var role = GetRole(roleName);

            _uow.GetRepository<UserRole>().Insert(new UserRole { UserId = user.Id, RoleId = role.Id });
            _uow.SaveChanges();

            return Task.FromResult<object>(null);
        }

        Task<IList<string>> IUserRoleStore<User, Guid>.GetRolesAsync(User user)
        {
            CheckUser(user);

            var roles = _uow.GetRepository<Role>().Entities.Where(r => user.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToList();
            return Task.FromResult<IList<string>>(roles);
        }

        Task<bool> IUserRoleStore<User, Guid>.IsInRoleAsync(User user, string roleName)
        {
            CheckUser(user);

            var role = GetRole(roleName); 
            bool result = user.Roles.Any(ur => ur.RoleId == role.Id);

            return Task.FromResult<bool>(result);
        }

        Task IUserRoleStore<User, Guid>.RemoveFromRoleAsync(User user, string roleName)
        {
            CheckUser(user);

            var role = GetRole(roleName);

            var userRole = user.Roles.FirstOrDefault(ur => ur.RoleId == role.Id);
            if (userRole != null)
            {
                user.Roles.Remove(userRole);
                _uow.Update(user);
                _uow.SaveChanges();
            }

            return Task.FromResult<object>(null);
        }
        #endregion

        #region IUserStore Implementation

        Task IUserStore<User, Guid>.CreateAsync(User user)
        {

            CheckUser(user);
            base.Add(user);

            return Task.FromResult<object>(null);
        }

        Task IUserStore<User, Guid>.DeleteAsync(User user)
        {
            CheckUser(user);
            base.Delete(user);

            return Task.FromResult<object>(null);
        }

        void IDisposable.Dispose()
        {
            
        }

        Task<User> IUserStore<User, Guid>.FindByIdAsync(Guid userId)
        {
            var response = base.GetById(userId);
            return Task.FromResult<User>(response.Data);
        }

        Task<User> IUserStore<User, Guid>.FindByNameAsync(string userName)
        {
            var response = base.GetWhereExpression(u => u.UserName == userName);
            return Task.FromResult<User>(response.Data);
        }        

        Task IUserStore<User, Guid>.UpdateAsync(User user)
        {
            CheckUser(user);
            base.Update(user);

            return Task.FromResult<object>(null);
        }
        #endregion

        #region IUserEmailStore
        Task IUserEmailStore<User, Guid>.SetEmailAsync(User user, string email)
        {
            CheckUser(user);

            // Email Business Logic?
            user.Email = email;
            _uow.Update(user);
            _uow.SaveChanges();

            return Task.FromResult<object>(null);
        }

        Task<string> IUserEmailStore<User, Guid>.GetEmailAsync(User user)
        {
            CheckUser(user);

            var email = user.Email;

            return Task.FromResult<string>(email);
        }

        Task<bool> IUserEmailStore<User, Guid>.GetEmailConfirmedAsync(User user)
        {
            CheckUser(user);
            var isEmailConfirmed = user.EmailConfirmed;

            return Task.FromResult<bool>(isEmailConfirmed);
        }

        Task IUserEmailStore<User, Guid>.SetEmailConfirmedAsync(User user, bool confirmed)
        {
            CheckUser(user);
            user.EmailConfirmed = confirmed; 
            _uow.SaveChanges();

            return Task.FromResult<object>(null);
        }

        Task<User> IUserEmailStore<User, Guid>.FindByEmailAsync(string email)
        {
            var user = _uow.GetRepository<User>().Entities.FirstOrDefault(u => u.Email == email);

            return Task.FromResult<User>(user);
        }

        #endregion

        #region IUserPasswordStore
        Task IUserPasswordStore<User, Guid>.SetPasswordHashAsync(User user, string passwordHash)
        {
            CheckUser(user);
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        Task<string> IUserPasswordStore<User, Guid>.GetPasswordHashAsync(User user)
        {
            CheckUser(user);

            return Task.FromResult<string>(user.PasswordHash);
        }

        Task<bool> IUserPasswordStore<User, Guid>.HasPasswordAsync(User user)
        {
            CheckUser(user);
            return Task.FromResult<bool>(!String.IsNullOrEmpty(user.PasswordHash));
        }
        #endregion
    }
}
