using DDI.Shared.Models.Client.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using DDI.Shared;
using DDI.Shared.Caching;

namespace DDI.Data.Helpers
{
    public static class EntityFrameworkHelpers
    {
        /// <summary>
        /// Get the User entity associated with the current thread.
        /// </summary>
        public static User GetCurrentUser()
        {
            return GetCurrentUser(new Repository<User>());
        }

        /// <summary>
        /// Get the User entity associated with the current thread.
        /// </summary>
        public static User GetCurrentUser(Repository<User> repository)
        {
            User user = GetCurrentUser((userId) => repository.Entities.FirstOrDefault(u => u.UserName == userId));
            return repository.Attach(user);
        }

        /// <summary>
        /// Get the User entity associated with the current thread.
        /// </summary>
        public static User GetCurrentUser(IUnitOfWork unitOfWork)
        {
            User user = GetCurrentUser((userId) => unitOfWork.FirstOrDefault<User>(p => p.UserName == userId));
            return unitOfWork.Attach(user);
        }

        /// <summary>
        /// Private logic for getting a cached user.
        /// </summary>
        private static User GetCurrentUser(Func<string,User> userFunction)
        {
            var userId = Thread.CurrentPrincipal?.Identity.Name;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                return CacheHelper.GetEntry("User_" + userId, () => userFunction(userId));
            }

            return null;
        }

        /// <summary>
        /// Specifies the related objects to include in query results.
        /// </summary>
        /// <typeparam name="T">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of navigation property being included.</typeparam>
        /// <param name="source">The source IQueryable on which to call Include</param>
        /// <param name="path">A lambda expression representing the path to include.</param>
        /// <returns>A new IQueryable<T> with the defined query path.</returns>
        public static IQueryable<T> IncludePath<T, TProperty>(IQueryable<T> source, System.Linq.Expressions.Expression<Func<T, TProperty>> path) where T : class
        {
            return source.Include(path);
        }
    }
}