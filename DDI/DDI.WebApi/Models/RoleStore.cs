using DDI.Services;
using DDI.Shared.Models.Client.Security;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace DDI.WebApi.Providers
{
    public class RoleStore : IRoleStore<Role, Guid>
    {
        private ServiceBase<Role> _service;

        public RoleStore() { }

        public RoleStore(ServiceBase<Role> service)
        {
            this._service = service;
        }

        public Task CreateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _service.Add(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _service.Delete(role);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Role> FindByIdAsync(Guid roleId)
        {
            var response = _service.GetById(roleId);
            return Task.FromResult<Role>(response.Data);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            var response = _service.GetWhereExpression(r => r.Name == roleName);
            return Task.FromResult<Role>(response.Data);
        }

        public Task UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            _service.Update(role);

            return Task.FromResult<object>(null);
        }
    }
}