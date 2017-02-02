using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using DDI.Shared.Models.Client.CRM;
using DDI.WebApi.Models;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Helpers
{
    public class PatchUpdateUser<T>
        where T : class
    {
        private DbContext _context;
        
        public PatchUpdateUser()
            : this(new ApplicationDbContext())
        {
        }

        public PatchUpdateUser(DbContext context)
        {
            _context = context;
        }

        public void UpdateUser(Guid id, JObject changes)
        {
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();

            foreach (var pair in changes)
            {
                changedProperties.Add(pair.Key, pair.Value.ToObject(ConvertToType<T>(pair.Key)));
            }

            var item = _context.Set<ApplicationUser>().Single(x => x.Id == id.ToString());

            UpdateChangedProperties(item, changedProperties);
        }

        private Type ConvertToType<TProperty>(string property)
        {
            Type classType = typeof(TProperty);

            var propertyType = classType.GetProperty(property).PropertyType;

            return propertyType;
        }

        public virtual int UpdateChangedProperties(ApplicationUser entity, IDictionary<string, object> propertyValues)
        {
            var entry = _context.Entry(entity);
            DbPropertyValues currentValues = entry.CurrentValues;

            foreach (KeyValuePair<string, object> keyValue in propertyValues)
            {
                currentValues[keyValue.Key] = keyValue.Value;
            }

            return _context.SaveChanges();
        }

    }
}