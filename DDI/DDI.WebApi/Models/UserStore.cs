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
    public class UserStore : IUserStore<User, Guid>
    {
        private ServiceBase<User> _service;

        public UserStore() { }

        public UserStore(ServiceBase<User> service)
        {
            this._service = service;
        }

        public Task CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _service.Add(user);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _service.Delete(user);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {

        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            var response = _service.GetById(userId);
            return Task.FromResult<User>(response.Data);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            var response = _service.GetWhereExpression(u => u.UserName == userName);
            return Task.FromResult<User>(response.Data);
        }

        public Task UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _service.Update(user);

            return Task.FromResult<object>(null);
        }
    }
}