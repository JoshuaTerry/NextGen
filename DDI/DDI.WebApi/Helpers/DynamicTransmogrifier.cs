using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using DDI.Shared;
using DDI.Shared.Models;

namespace DDI.WebApi.Helpers
{
    public class DynamicTransmogrifier
    {
        public IDataResponse ToDynamicResponse<T>(IDataResponse<T> response, string fields = null, bool shouldAddHATEAOSLinks = true)
        {
            var dynamicResponse = new DataResponse<dynamic>
            {
                ErrorMessages = response.ErrorMessages,
                IsSuccessful = response.IsSuccessful,
                TotalResults = response.TotalResults,
                VerboseErrorMessages = response.VerboseErrorMessages
            };
            if (response.Data is IEnumerable<EntityBase>)
            {
                dynamicResponse.Data = ToDynamicList((response.Data as IEnumerable<EntityBase>), fields, shouldAddHATEAOSLinks);
            }
            else if (response.Data is EntityBase)
            {
                dynamicResponse.Data = ToDynamicObject((response.Data as EntityBase), fields, shouldAddHATEAOSLinks);
            }
            return dynamicResponse;
        }

        public dynamic ToDynamicList<T>(IEnumerable<T> data, string fields = null, bool shouldAddHATEAOSLinks = true)
            where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null && !shouldAddHATEAOSLinks)
            {
                return data;
            }
            var listOfFields = fields?.ToUpper().Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, listOfFields, shouldAddHATEAOSLinks);
            return result;
        }

        public dynamic ToDynamicObject<T>(T data, string fields = null, bool shouldAddHATEAOSLinks = true)
            where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null && !shouldAddHATEAOSLinks)
            {
                return data;
            }
            var listOfFields = fields?.ToUpper().Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, listOfFields, shouldAddHATEAOSLinks);
            return result;
        }

        private dynamic RecursivelyTransmogrify<T>( T data, List<string> fieldsToInclude = null, bool shouldAddHATEAOSLinks = true)
            where T : EntityBase
        {
            //This works, but it would be quicker if we just loop through the list of fields sent in. The only complicated piece is the sub-fields
            dynamic returnObject = new ExpandoObject();
            Type type = data.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = type.GetProperties(flags);

            foreach (PropertyInfo property in properties)
            {
                if (!fieldsToInclude.Any() || fieldsToInclude.Contains(property.Name.ToUpper()))
                {
                    ((IDictionary<string, object>) returnObject)[property.Name] = property.GetValue(data, null);
                }
                else if (fieldsToInclude.Any(a => a.StartsWith($"{property.Name.ToUpper()}.")))
                {
                    var currentProperty = $"{property.Name.ToUpper()}.";
                    var fieldArrayList = fieldsToInclude.Where(a => a.StartsWith(currentProperty));
                    var newFieldList = String.Join(",", fieldArrayList);
                    var strippedFields = newFieldList.Replace(currentProperty, ""); //TODO this can cause a problem if you had Address.CurrentAddress.State it would be CurrentState
                    var strippedFieldList = strippedFields.Split(',').ToList();
                    var fieldValue = property.GetValue(data, null);
                    if (fieldValue is IEnumerable<EntityBase>)
                    {
                        ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as IEnumerable<EntityBase>), strippedFieldList, shouldAddHATEAOSLinks); //(fieldValue as IEnumerable<EntityBase>).ToPartialObject(strippedFieldList);
                    }
                    else if (fieldValue is EntityBase)
                    {
                        ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as EntityBase), strippedFieldList, shouldAddHATEAOSLinks); //(fieldValue as EntityBase).ToPartialObject(strippedFieldList);
                    }
                }
            }
            if (data is EntityBase && shouldAddHATEAOSLinks)
            {
                returnObject = AddHATEAOSLinks(returnObject, data as EntityBase);
            }
            return returnObject;
        }

        public dynamic RecursivelyTransmogrify<T>(IEnumerable<T> entities, List<string> fieldsToInclude = null, bool shouldAddLinks = true)
            where T : EntityBase
        {
            var list = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                list.Add(RecursivelyTransmogrify(item, fieldsToInclude, shouldAddLinks));
            }
        
            return list;
        }
        

        private dynamic AddHATEAOSLinks<T>(IDictionary<string, object> entity, T data)
            where T: EntityBase
        {
            var links = new List<HATEOASLink>
            {
                new HATEOASLink()
                {
                    Href = $"api/v1/TODO_CHANGE_THIS/{data.Id}",
                    Relationship = "self",
                    Method = "GET"
                }
            };
            entity.Add("Links", links);
            return entity;
        }
    }
}