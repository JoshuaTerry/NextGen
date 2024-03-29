﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.WebApi.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;

namespace DDI.WebApi.Tests.Fakes
{
    public class FakeUserManager //: IUserManager
    {
        public IQueryable<User> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> AddPasswordAsync(string userId, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> CreateAsync(User user, string password)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            await Task.Delay(1);
            var user = new User
            {
                Id = userId
            };
            var returnObject = new Task<User>(() => user);
            return user;
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> GeneratePasswordResetTokenAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> RemoveLoginAsync(string userId, Microsoft.AspNet.Identity.UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> RemovePasswordAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNet.Identity.IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
