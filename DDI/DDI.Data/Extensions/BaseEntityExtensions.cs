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
        public static dynamic ToPartialObject<T>(this IEnumerable<T> entities, string listOfFields = null, bool shouldAddLinks = false) where T: BaseEntity
        {
            var list = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                list.Add(item.ToPartialObject(listOfFields, shouldAddLinks));
            }
            
            return list;
        }
//
//        public static dynamic AddLinks(this object entity)
//        {
//            dynamic entityWithLinks = entity;
//            Debug.WriteLine(entity.GetType());
//            if (entity is IEnumerable)
//            {
//                var list = new List<ExpandoObject>();
//                IEnumerable enumerable = entity as IEnumerable;
//                foreach (var item in enumerable)
//                {
//                    list.Add(item.AddLinks());
//                }
//
//                return list;
//            }
//            else if (entity is Constituent)
//            {
//                entityWithLinks = ((Constituent) entity).AddLinks();
//            }
//            return entityWithLinks;
//        }
        public static dynamic ToDynamic<T>(this T value, List<string> fieldsToInclude = null ) where T: BaseEntity
        {

            //            IDictionary<string, object> expando = new ExpandoObject();
            //
            //            if (!fieldsToInclude.Any())
            //            {
            //                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            //                {
            //                    expando.Add(property.Name, property.GetValue(value));
            //                }
            //            }
            //            else
            //            {
            //                foreach (var field in fieldsToInclude)
            //                {
            //                    if (field.Contains("."))
            //                    {
            //                        var property = field.Substring(0, field.IndexOf("."));
            //                        var subProperty = field.Substring(field.IndexOf(".")+1,field.Length - field.IndexOf(".")-1);
            //                        var actualField = value.GetType().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //                        var fieldValue = actualField.GetValue(value, null);
            //                        if (fieldValue is IEnumerable<BaseEntity>)
            //                        {
            //                            expando.Add(actualField.Name, (fieldValue as IEnumerable<BaseEntity>).ToPartialObject(subProperty));
            //                        }
            //                        else if (fieldValue is BaseEntity)
            //                        {
            //                            expando.Add(actualField.Name, (fieldValue as BaseEntity).ToPartialObject(subProperty));
            //                        }
            //                    }
            //                    else
            //                    {
            //                        var actualField = value.GetType().GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //                        var fieldValue = actualField.GetValue(value, null);
            //                        expando.Add(actualField.Name, fieldValue);
            //                    }
            //                }
            //            }
            dynamic returnObject = new ExpandoObject();
            Type type = value.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = type.GetProperties(flags);

            foreach (PropertyInfo property in properties)
            {
                if (!fieldsToInclude.Any() || fieldsToInclude.Contains(property.Name.ToUpper()))
                {
                    ((IDictionary<string, object>) returnObject)[property.Name] = property.GetValue(value, null);
                }
                else if (fieldsToInclude.Any(a => a.StartsWith($"{property.Name.ToUpper()}.")))
                {
                    var currentProperty = $"{property.Name.ToUpper()}.";
                    var fieldArrayList = fieldsToInclude.Where(a => a.StartsWith(currentProperty));
                    var newFieldList = String.Join(",", fieldArrayList);
                    var strippedFieldList = newFieldList.Replace(currentProperty, "");
                    var fieldValue = property.GetValue(value, null);
                    //((IDictionary<string, object>) returnObject)[property.Name] = property.GetValue(value, null).ToPartialObject(strippedFieldList);
                    if (fieldValue is IEnumerable<BaseEntity>)
                    {
                        ((IDictionary<string, object>) returnObject)[property.Name] = (fieldValue as IEnumerable<BaseEntity>).ToPartialObject(strippedFieldList);
                    }
                    else if (fieldValue is BaseEntity)
                    {
                        ((IDictionary<string, object>) returnObject)[property.Name] = (fieldValue as BaseEntity).ToPartialObject(strippedFieldList);
                    }
                }
            }
            return returnObject;
        }
    }
}
