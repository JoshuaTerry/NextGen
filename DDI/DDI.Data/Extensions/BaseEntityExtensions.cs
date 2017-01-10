using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Models;
using DDI.Data.Models.Client.CRM;
using DDI.Shared;
using RelationshipType = System.Data.Entity.Core.Metadata.Edm.RelationshipType;

namespace DDI.Data.Extensions
{
    public static class BaseEntityExtensions
    {

//        public static dynamic AddLinks<T>(this IEnumerable<T> entities) where T : IEntity
//        {
//            var list = new List<ExpandoObject>();
//            foreach (var item in entities)
//            {
//                Debug.WriteLine(item.GetType());
//                list.Add(item.AddLinks());
//            }
//
//            return list;
//        }
//
//        public static dynamic AddLinks(this Func<BaseEntity> entity)
//        {
//            return entity.Target.AddLinks();
//        }

        public static dynamic ToPartialObject<T>(this List<T> entities, string listOfFields, bool shouldAddLinks) where T: BaseEntity
        {
            var list = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                Debug.WriteLine(item.GetType());
                list.Add(item.ToPartialObject(listOfFields, shouldAddLinks));
            }
            
            return list;
        }

        public static dynamic AddLinks(this object entity)
        {
            dynamic entityWithLinks = entity;
            Debug.WriteLine(entity.GetType());
            if (entity is IEnumerable)
            {
                var list = new List<ExpandoObject>();
                IEnumerable enumerable = entity as IEnumerable;
                foreach (var item in enumerable)
                {
                    list.Add(item.AddLinks());
                }

                return list;
            }
            else if (entity is Constituent)
            {
                entityWithLinks = ((Constituent) entity).AddLinks();
            }
            return entityWithLinks;
        }
//
//        public static dynamic AddLinks(this BaseEntity entity)
//        {
//            return entity;
//        }
//        public static dynamic AddLinks<T>(this T entity) where T: Constituent, IEntity
//        {
//            IDictionary<string, object> constituentWithLinks = entity.ToDynamic();
//            // Add spouse link?
//            var links = new List<HATEOASLink>
//            {
//                new HATEOASLink()
//                {
//                    Href = $"api/v1/constituents/{entity.Id}",
//                    Relationship = "self",
//                    Method = "GET"
//                }
//            };
//            constituentWithLinks.Add("Links", links);
//
//
//            return constituentWithLinks;
//        }
        public static dynamic ToDynamic<T>(this T value, List<string> fieldsToInclude ) where T: BaseEntity
        {
//            if (value is IEnumerable)
//            {
//                var list = new List<ExpandoObject>();
//                IEnumerable enumerable = value as IEnumerable;
//                foreach (var item in enumerable)
//                {
//                    list.Add(item.ToDynamic(fieldsToInclude));
//                }
//
//                return list;
//            }

            IDictionary<string, object> expando = new ExpandoObject();

            if (!fieldsToInclude.Any())
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                {
                    expando.Add(property.Name, property.GetValue(value));
                }
            }
            else
            {
                foreach (var field in fieldsToInclude)
                {
                    var actualField = value.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    var fieldValue = actualField.GetValue(value, null);

                    expando.Add(actualField.Name, fieldValue);
                }
            }

            return expando;
        }
    }
}
