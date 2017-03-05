using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDI.WebApi.Models;
using Microsoft.AspNet.Identity;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;

namespace DDI.WebApi
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User> FindByIdAsync(string userId);
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> RemovePasswordAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IList<string>> GetRolesAsync(string id);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        IQueryable<User> Users { get; }
        void Dispose();
    }
}