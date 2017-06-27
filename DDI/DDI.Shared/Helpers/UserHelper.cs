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
        public static User GetCurrentUser()
        {
            return GetCurrentUser(Factory.CreateRepository<User>());
        }

        /// <summary>
        /// Get the User entity associated with the current thread.
        /// </summary>
        public static User GetCurrentUser(IRepository<User> repository)
        {
            User user = GetCurrentUser((userId) => repository.Entities.FirstOrDefault(u => u.UserName == userId));
            return (user != null ? repository.Attach(user) : null);
        }

        /// <summary>
        /// Get the User entity associated with the current thread.
        /// </summary>
        public static User GetCurrentUser(IUnitOfWork unitOfWork)
        {
            User user = GetCurrentUser((userId) => unitOfWork.FirstOrDefault<User>(p => p.UserName == userId));
            return (user != null ? unitOfWork.Attach(user) : null);
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
