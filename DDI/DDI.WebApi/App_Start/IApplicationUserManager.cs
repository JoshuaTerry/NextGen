using System.Linq;
using System.Threading.Tasks;
using DDI.WebApi.Models;
using Microsoft.AspNet.Identity;

namespace DDI.WebApi
{
    public interface IApplicationUserManager
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByIdAsync(string userId);
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> RemovePasswordAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        IQueryable<ApplicationUser> Users { get; }
        void Dispose();
    }
}