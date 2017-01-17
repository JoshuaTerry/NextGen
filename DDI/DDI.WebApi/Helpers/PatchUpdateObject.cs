using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Web;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;
using DDI.WebApi.Models;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Helpers
{
    public class PatchUpdate<T>
        where T : class
    {
        private DbContext _context;


        public PatchUpdate()
            : this(new DomainContext())
        {
        }

        public PatchUpdate(DbContext context)
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

        private Type ConvertToType<T>(string property)
        {
            Type classType = typeof(T);

            var propertyType = classType.GetProperty(property).PropertyType;

            return propertyType;
        }

        //public virtual int UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        //{
        //    return UpdateChangedProperties(id, propertyValues, action);
        //}

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