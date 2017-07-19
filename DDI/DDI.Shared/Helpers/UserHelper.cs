using DDI.Shared.Caching;
using DDI.Shared.Models.Client.Security;
using System;
using System.Linq;
using System.Threading;

namespace DDI.Shared.Helpers
{
    public static class UserHelper
    {
        /// <summary>
        /// Get the User entity associated with the current thread.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to use for retrieving the user.</param>
        /// <param name="dontCache">TRUE to prevent returning a cached user (in case the user is to be attached to another entity.)</param>
        public static User GetCurrentUser(IUnitOfWork unitOfWork, bool dontCache = false)
        {
            if (dontCache)
            {
                var userId = Thread.CurrentPrincipal?.Identity.Name;

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    return unitOfWork.FirstOrDefault<User>(p => p.UserName == userId);
                }
                return null;                        
            }

            User user = GetCurrentUser((userId) => unitOfWork.FirstOrDefault<User>(p => p.UserName == userId));
            return user;
        }

        /// <summary>
        /// Return the display name for the User entity associated with the current thread.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserDisplayName()
        {
            User user = GetCurrentUser(userId =>
            {
                using (var uow = Factory.CreateUnitOfWork())
                {
                    return uow.FirstOrDefault<User>(p => p.UserName == userId);
                }
            });

            if (user != null)
            {
                return user.DisplayName;
            }
            return Thread.CurrentPrincipal?.Identity.Name ?? string.Empty;
        }

        /// <summary>
        /// Private logic for getting a cached user.
        /// </summary>
        private static User GetCurrentUser(Func<string, User> userFunction)
        {
            var userId = Thread.CurrentPrincipal?.Identity.Name;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                return CacheHelper.GetEntry("User_" + userId, () => userFunction(userId));
            }

            return null;
        }


    }
}
