using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Routing;
using DDI.Shared; 
using DDI.Shared.Models;
using DDI.Shared.Statics;
using Microsoft.Ajax.Utilities;

namespace DDI.WebApi.Helpers
{
    public class DynamicTransmogrifier
    {
        public IDataResponse ToDynamicResponse<T>(IDataResponse<T> response, UrlHelper urlHelper, string fields = null)
        {
            var dynamicResponse = new DataResponse<dynamic>
            {
                ErrorMessages = response.ErrorMessages,
                IsSuccessful = response.IsSuccessful,
                TotalResults = response.TotalResults,
                VerboseErrorMessages = response.VerboseErrorMessages
            };
            if (response.Data is IEnumerable<ICanTransmogrify>)
            {
                dynamicResponse.Data = ToDynamicList((response.Data as IEnumerable<ICanTransmogrify>), urlHelper, fields) ;
            }
            else if (response.Data is ICanTransmogrify)
            {
                dynamicResponse.Data = ToDynamicObject((response.Data as ICanTransmogrify), urlHelper, fields);
            }
            else if (response.Data == null || IsSimple(response.Data.GetType()))
            {
                dynamicResponse.Data = response.Data;
            }
            return dynamicResponse;
        }
        internal bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal);
        }

        public dynamic ToDynamicList<T>(IEnumerable<T> data, UrlHelper urlHelper, string fields = null)
            where T : ICanTransmogrify
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null)
            {
                return data;
            }
            var upperCaseFields = fields?.ToUpper();
            
            var listOfFields = upperCaseFields?.Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields);
            return result;
        }

        public dynamic ToDynamicObject<T>(T data, UrlHelper urlHelper, string fields = null)
            where T : ICanTransmogrify
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null)
            {
                return data;
            }
            var upperCaseFields = fields?.ToUpper();
            
            var listOfFields = upperCaseFields?.Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields);
            return result;
        }

        private dynamic RecursivelyTransmogrify<T>(T data, UrlHelper urlHelper, List<string> fieldsToInclude = null, IEnumerable<Guid> visited = null)
            where T : ICanTransmogrify
        {
            dynamic returnObject = new ExpandoObject();
            if (visited == null)
            {
                visited = new List<Guid> {data.Id};
            }
            else if (visited.Contains(data.Id))
            {
                // To avoid infinite loops, if we have processed this object before, just return the Id
                ((IDictionary<string, object>) returnObject)["Id"] = data.Id;
                return returnObject;
            }
            else
            {
                visited = visited.Union(new Guid[] {data.Id});
            }
            Type type = data.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = type.GetProperties(flags);
            bool includeEverything = (fieldsToInclude == null || !fieldsToInclude.Any());
            foreach (PropertyInfo property in properties)
            {
                var fieldValue = property.GetValue(data, null);
                var propertyNameUppercased = property.Name.ToUpper();
                if (fieldValue is IEnumerable<ICanTransmogrify> && (includeEverything || fieldsToInclude.Any(a => a.StartsWith($"{propertyNameUppercased}."))))
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as IEnumerable<ICanTransmogrify>), urlHelper, strippedFieldList, visited); 
                }
                else if (fieldValue is ICanTransmogrify && (includeEverything || fieldsToInclude.Any(a => a.StartsWith($"{propertyNameUppercased}."))))  
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as ICanTransmogrify), urlHelper, strippedFieldList, visited); 
                }
                else if (includeEverything || fieldsToInclude.Contains(propertyNameUppercased))
                {
                    ((IDictionary<string, object>) returnObject)[property.Name] = fieldValue;
                }
            }
            
            return returnObject;
        }

        private List<string> StripFieldList(List<string> fieldsToInclude, string propertyNameUppercased)
        {
            if (fieldsToInclude.Count == 0)
            {
                return fieldsToInclude;
            }
            var currentProperty = $"{propertyNameUppercased}.";
            var strippedFieldList = fieldsToInclude.Where(a => a.StartsWith(currentProperty)).Select(a => a.Substring(currentProperty.Length)).ToList();
            return strippedFieldList;
        }

        public dynamic RecursivelyTransmogrify<T>(IEnumerable<T> entities, UrlHelper urlHelper, List<string> fieldsToInclude = null, IEnumerable<Guid> visited = null)
            where T : ICanTransmogrify
        {
            var list = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                list.Add(RecursivelyTransmogrify(item, urlHelper, fieldsToInclude, visited));
            }
        
            return list;
        }     
    }
}