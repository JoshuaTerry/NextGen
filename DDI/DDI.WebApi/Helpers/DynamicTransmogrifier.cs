using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Routing;
using DDI.Shared;
using DDI.Shared.Attributes;
using DDI.Shared.Extensions;
using DDI.Shared.Models;
using DDI.Shared.Statics;

namespace DDI.WebApi.Helpers
{
    public class DynamicTransmogrifier
    {
        public IDataResponse ToDynamicResponse<T>(IDataResponse<T> response, UrlHelper urlHelper, string fields = null, bool shouldAddHATEAOSLinks = true)
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
                dynamicResponse.Data = ToDynamicList((response.Data as IEnumerable<EntityBase>), urlHelper, fields, shouldAddHATEAOSLinks);
            }
            else if (response.Data is EntityBase)
            {
                dynamicResponse.Data = ToDynamicObject((response.Data as EntityBase), urlHelper, fields, shouldAddHATEAOSLinks);
            }
            return dynamicResponse;
        }

        public dynamic ToDynamicList<T>(IEnumerable<T> data, UrlHelper urlHelper, string fields = null, bool shouldAddHATEAOSLinks = true)
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
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields, shouldAddHATEAOSLinks);
            return result;
        }

        public dynamic ToDynamicObject<T>(T data, UrlHelper urlHelper, string fields = null, bool shouldAddHATEAOSLinks = true)
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
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields, shouldAddHATEAOSLinks);
            return result;
        }

        private dynamic RecursivelyTransmogrify<T>(T data, UrlHelper urlHelper, List<string> fieldsToInclude = null, bool shouldAddHATEAOSLinks = true)
            where T : EntityBase
        {
            dynamic returnObject = new ExpandoObject();
            Type type = data.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = type.GetProperties(flags);
            bool includeEverything = (fieldsToInclude == null || !fieldsToInclude.Any());
            foreach (PropertyInfo property in properties)
            {
                var fieldValue = property.GetValue(data, null);
                var propertyNameUppercased = property.Name.ToUpper();
                if (fieldValue is IEnumerable<EntityBase> && (includeEverything || fieldsToInclude.Any(a => a.StartsWith($"{propertyNameUppercased}."))))
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as IEnumerable<EntityBase>), urlHelper, strippedFieldList, shouldAddHATEAOSLinks); 
                }
                else if (fieldValue is EntityBase && (includeEverything || fieldsToInclude.Any(a => a.StartsWith($"{propertyNameUppercased}."))))  
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as EntityBase), urlHelper, strippedFieldList, shouldAddHATEAOSLinks); 
                }
                else if (includeEverything || fieldsToInclude.Contains(propertyNameUppercased))
                {
                    ((IDictionary<string, object>) returnObject)[property.Name] = fieldValue;
                }
            }
            if (data is EntityBase && shouldAddHATEAOSLinks)
            {
                returnObject = AddHATEAOSLinks(returnObject, data as EntityBase, urlHelper);
            }
            return returnObject;
        }

        private List<string> StripFieldList(List<string> fieldsToInclude, string propertyNameUppercased)
        {
            var currentProperty = $"{propertyNameUppercased}.";
            var fieldArrayList = fieldsToInclude.Where(a => a.StartsWith(currentProperty));
            var newFieldList = String.Join(",", fieldArrayList);
            var strippedFields = newFieldList.Replace(currentProperty, ""); //TODO this can cause a problem if you had Address.CurrentAddress.State it would be CurrentState
            var strippedFieldList = strippedFields.Split(',').ToList();
            return strippedFieldList;
        }

        public dynamic RecursivelyTransmogrify<T>(IEnumerable<T> entities, UrlHelper urlHelper, List<string> fieldsToInclude = null, bool shouldAddLinks = true)
            where T : EntityBase
        {
            var list = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                list.Add(RecursivelyTransmogrify(item, urlHelper, fieldsToInclude, shouldAddLinks));
            }
        
            return list;
        }
        

        private dynamic AddHATEAOSLinks<T>(IDictionary<string, object> entity, T data, UrlHelper urlHelper)
            where T: EntityBase
        {
            string routePath = data.GetType().GetCustomAttribute<HATEAOSAttribute>()?.RouteName;
            if (!string.IsNullOrWhiteSpace(routePath))
            {
                var links = AddSelfHATEAOSLinks(data, urlHelper, routePath);
                links.AddRangeNullSafe(FindAllAddChildHATEAOSLinks(data, urlHelper, routePath));
                entity.Add("Links", links);
            }
            return entity;
        }

        private IEnumerable<HATEOASLink> FindAllAddChildHATEAOSLinks<T>(T data, UrlHelper urlHelper, string routePath) where T : EntityBase
        {
            List<HATEOASLink> hateaosLinks = null;
            var properties = data.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(HATEAOSPostLinkAttribute)));
            foreach (var propertyInfo in properties)
            {
                if (hateaosLinks == null)
                {
                    hateaosLinks = new List<HATEOASLink>();
                }
                hateaosLinks.Add(new HATEOASLink
                {
                    Href = urlHelper.Link(routePath, new { id = data.Id }),  //TODO Add ConstituentAddresses
                    Relationship = $"{RouteRelationships.New}{propertyInfo.GetAttribute<HATEAOSPostLinkAttribute>().RouteName}",
                    Method = RouteVerbs.Post
                });
            }
            return hateaosLinks;
        }

        private List<HATEOASLink> AddSelfHATEAOSLinks<T>(T data, UrlHelper urlHelper, string routePath) 
            where T: EntityBase
        {
            return new List<HATEOASLink>
            {
                new HATEOASLink()
                {
                    Href = urlHelper.Link(routePath, new {id = data.Id}), 
                    Relationship = RouteRelationships.Self,
                    Method = RouteVerbs.Get
                },
                new HATEOASLink()
                {
                    Href = urlHelper.Link(routePath, new {id = data.Id}), 
                    Relationship = $"{RouteRelationships.Update}{data.GetType().Name}",
                    Method = RouteVerbs.Patch
                },
                new HATEOASLink()
                {
                    Href = urlHelper.Link(routePath, new {id = data.Id}), 
                    Relationship = $"{RouteRelationships.Delete}{data.GetType().Name}",
                    Method = RouteVerbs.Delete
                },
            };
        }
    }
}