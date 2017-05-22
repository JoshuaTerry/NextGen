using DDI.Shared;
using DDI.Shared.Models.Client.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.Security
{
    public class RoleService : ServiceBase<Role>, IRoleStore<Role, Guid>,
                                                  IQueryableRoleStore<Role, Guid>
    {

        public RoleService(IUnitOfWork uow) : base(uow) { }

        #region IQueryableRoleStore Implementation

        IQueryable<Role> IQueryableRoleStore<Role, Guid>.Roles => UnitOfWork.GetRepository<Role>().Entities;

        #endregion  

        private void CheckRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
        }
        #region IRoleStore Implementation
        Task IRoleStore<Role, Guid>.CreateAsync(Role role)
        {
            CheckRole(role);
            base.Add(role);

            return Task.FromResult<object>(null);
        }

        Task IRoleStore<Role, Guid>.DeleteAsync(Role role)
        {
            CheckRole(role);
            base.Delete(role);

            return Task.FromResult<object>(null);
        }

        void IDisposable.Dispose()
        {
             
        }

        Task<Role> IRoleStore<Role, Guid>.FindByIdAsync(Guid roleId)
        {
            var response = base.GetById(roleId);
            return Task.FromResult<Role>(response.Data);
        }

        Task<Role> IRoleStore<Role, Guid>.FindByNameAsync(string roleName)
        {
            var response = base.GetWhereExpression(r => r.Name == roleName);
            return Task.FromResult<Role>(response.Data);
        }

        Task IRoleStore<Role, Guid>.UpdateAsync(Role role)
        {
            CheckRole(role);
            base.Update(role);

            return Task.FromResult<object>(null);
        }
        #endregion
    }
}
