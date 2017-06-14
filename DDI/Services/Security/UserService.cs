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
using System.Linq.Expressions;
using DDI.Shared.Extensions;
using DDI.Services.General;

namespace DDI.Services.Security
{
    public class UserService : ServiceBase<User>, IUserStore<User, Guid>,
                                                  IUserRoleStore<User, Guid>,
                                                  IUserEmailStore<User, Guid>,
                                                  IUserPasswordStore<User, Guid>,
                                                  IQueryableUserStore<User, Guid>
    {
        public UserService(IUnitOfWork uow) : base(uow)
        {
            IncludesForSingle = new Expression<Func<User, object>>[]
            {
               u => u.Roles
            };
        }

        public IDataResponse AddDefaultBusinessUnitToUser(Guid id, Guid defaultbuid)
        {
            var result = UnitOfWork.GetById<User>(id, u => u.BusinessUnits);
            var defaultbu = UnitOfWork.GetById<BusinessUnit>(defaultbuid);

            if (!result.BusinessUnits.Contains(defaultbu))
            {
                result.BusinessUnits.Add(defaultbu);
            }

            result.DefaultBusinessUnitId = defaultbuid;

           UnitOfWork.SaveChanges();

            DataResponse<User> response = new DataResponse<User>
            {
                Data = result,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse AddBusinessUnitToUser(Guid id, Guid buid)
        {
            var result = UnitOfWork.GetById<User>(id, r => r.BusinessUnits);
            var bu = UnitOfWork.GetById<BusinessUnit>(buid);

            if (!result.BusinessUnits.Contains(bu))
            {
                result.BusinessUnits.Add(bu);
                UnitOfWork.SaveChanges();
            }

            IDataResponse response = new DataResponse<User>
            {
                Data = result,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse SyncBusinessUnitsToUser(Guid id, List<Guid> buids)
        {
            User result = UnitOfWork.GetById<User>(id, r => r.BusinessUnits);

            List<Guid> currentBusinessUnitIds = new List<Guid>();
            //remove any business units that should not be there
            foreach (BusinessUnit bu in result.BusinessUnits)
            {
                currentBusinessUnitIds.Add(bu.Id);

            }

            foreach (Guid cbuId in currentBusinessUnitIds)
            { 
                if (!buids.Contains(cbuId))
                {
                    BusinessUnit bu = UnitOfWork.GetById<BusinessUnit>(cbuId);
                    result.BusinessUnits.Remove(bu);
                }
            }

            //add any business units that are not there already
            foreach (Guid buid in buids)
            {
                BusinessUnit bu = UnitOfWork.GetById<BusinessUnit>(buid);
                if (!result.BusinessUnits.Contains(bu))
                {
                    result.BusinessUnits.Add(bu);
                }
            }

            UnitOfWork.SaveChanges();

            IDataResponse response = new DataResponse<User>
            {
                Data = result,
                IsSuccessful = true
            };

            return response;
        }

        public IDataResponse AddGroupToUser(Guid id, Guid groupId)
        {
            GroupService groupService = new GroupService(UnitOfWork);
            groupService.AddUserToGroup(id, groupId);

            IDataResponse response = new DataResponse<Guid>
            {
                Data = id,
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

            var role = UnitOfWork.FirstOrDefault<Role>(r => r.Name == roleName);

            if (role == null)
                throw new ArgumentNullException($"No role with the name {roleName} was found.");

            return role;
        }

        private List<Role> GetRoles(User user)
        {
            CheckUser(user);
            var roles = UnitOfWork.Where<Role>(r => user.Roles.Select(ur => ur.RoleId).Contains(r.Id)).ToList();
            return roles;
        }

        public User GetUserByUsername(string username)
        {
            var user = UnitOfWork.FirstOrDefault<User>(u => u.UserName == username);
            return user;
        }

        #region IQueryableUserStore Implementation
        IQueryable<User> IQueryableUserStore<User, Guid>.Users => UnitOfWork.GetEntities<User>();
        #endregion

        #region IUserRoleStore Implementation

        Task IUserRoleStore<User, Guid>.AddToRoleAsync(User user, string roleName)
        {
            CheckUser(user);

            var role = GetRole(roleName);

            UnitOfWork.Insert(new UserRole { UserId = user.Id, RoleId = role.Id });
            UnitOfWork.SaveChanges();

            return Task.FromResult<object>(null);
        }

        Task<IList<string>> IUserRoleStore<User, Guid>.GetRolesAsync(User user)
        {
            CheckUser(user);
            var roleIds = user.Roles.Select(ur => ur.RoleId).ToList();              
            var roles = UnitOfWork.Where<Role>(r => roleIds.Contains(r.Id)).Select(r=>r.Name).ToList();
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
                UnitOfWork.Update(user);
                UnitOfWork.SaveChanges();
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
            UnitOfWork.Update(user);
            UnitOfWork.SaveChanges();

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
            UnitOfWork.SaveChanges();

            return Task.FromResult<object>(null);
        }

        Task<User> IUserEmailStore<User, Guid>.FindByEmailAsync(string email)
        {
            var user = UnitOfWork.FirstOrDefault<User>(u => u.Email == email);

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
