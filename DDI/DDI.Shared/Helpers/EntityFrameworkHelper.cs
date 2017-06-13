using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using DDI.Shared.Models;

namespace DDI.Data.Helpers
{
    public static class EntityFrameworkHelper
    {
        /// <summary>
        /// Get the table name for an entity.
        /// </summary>
        public static string GetTableName<T>() where T : IEntity
        {
            var tableAttribute = typeof(T).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            if (tableAttribute != null)
            {
                return tableAttribute.Name;
            }

            return typeof(T).Name;
        }
    }
}