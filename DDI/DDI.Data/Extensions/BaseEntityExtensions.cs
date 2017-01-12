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

        public static dynamic ToDynamic<T>(this T value, List<string> fieldsToInclude = null ) where T: BaseEntity
        {
            //This works, but it would be quicker if we just loop through the list of fields sent in. The only complucated piece is the sub-fields group together in one object
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
