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
        public IDataResponse ToDynamicResponse<T>(IDataResponse<T> response, UrlHelper urlHelper, string fields = null, bool shouldAddHateoasLinks = true)
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
                dynamicResponse.Data = ToDynamicList((response.Data as IEnumerable<EntityBase>), urlHelper, fields, shouldAddHateoasLinks);
            }
            else if (response.Data is EntityBase)
            {
                dynamicResponse.Data = ToDynamicObject((response.Data as EntityBase), urlHelper, fields, shouldAddHateoasLinks);
            }
            return dynamicResponse;
        }

        public dynamic ToDynamicList<T>(IEnumerable<T> data, UrlHelper urlHelper, string fields = null, bool shouldAddHateoasLinks = true)
            where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null && !shouldAddHateoasLinks)
            {
                return data;
            }
            var listOfFields = fields?.ToUpper().Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields, shouldAddHateoasLinks);
            return result;
        }

        public dynamic ToDynamicObject<T>(T data, UrlHelper urlHelper, string fields = null, bool shouldAddHateoasLinks = true)
            where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null && !shouldAddHateoasLinks)
            {
                return data;
            }
            var listOfFields = fields?.ToUpper().Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields, shouldAddHateoasLinks);
            return result;
        }

        private dynamic RecursivelyTransmogrify<T>(T data, UrlHelper urlHelper, List<string> fieldsToInclude = null, bool shouldAddHateoasLinks = true)
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
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as IEnumerable<EntityBase>), urlHelper, strippedFieldList, shouldAddHateoasLinks); 
                }
                else if (fieldValue is EntityBase && (includeEverything || fieldsToInclude.Any(a => a.StartsWith($"{propertyNameUppercased}."))))  
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as EntityBase), urlHelper, strippedFieldList, shouldAddHateoasLinks); 
                }
                else if (includeEverything || fieldsToInclude.Contains(propertyNameUppercased))
                {
                    ((IDictionary<string, object>) returnObject)[property.Name] = fieldValue;
                }
            }
            if (shouldAddHateoasLinks)
            {
                returnObject = AddHateoasLinks(returnObject, data as EntityBase, urlHelper);
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
        

        private dynamic AddHateoasLinks<T>(IDictionary<string, object> entity, T data, UrlHelper urlHelper)
            where T: EntityBase
        {
            string routePath = data.GetType().GetCustomAttribute<HateoasAttribute>()?.RouteName;
            if (!string.IsNullOrWhiteSpace(routePath))
            {
                var links = AddSelfHateoasLinks(data, urlHelper, routePath);
                links.AddRangeNullSafe(FindAllAddChildHateoasLinks(data, urlHelper, routePath));
                entity.Add("Links", links);
            }
            return entity;
        }

        private IEnumerable<HateoasLink> FindAllAddChildHateoasLinks<T>(T data, UrlHelper urlHelper, string routePath) where T : EntityBase
        {
            List<HateoasLink> hateoasLinks = null;
            var properties = data.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(HateoasCollectionLinkAttribute)));
            foreach (var propertyInfo in properties)
            {
                if (hateoasLinks == null)
                {
                    hateoasLinks = new List<HateoasLink>();
                }
                string propertyRoute = propertyInfo.GetAttribute<HateoasCollectionLinkAttribute>().RouteName;
                hateoasLinks.AddRange(new List<HateoasLink>
                { 
                    new HateoasLink
                    {
                        Href = urlHelper.Link($"{propertyRoute}{RouteVerbs.Post}", null),
                        Relationship = $"{RouteRelationships.New}{propertyRoute}",
                        Method = RouteVerbs.Post
                    },
                    new HateoasLink
                    {
                        Href = urlHelper.Link($"{routePath}{propertyRoute}", new {id = data.Id}),
                        Relationship = $"{RouteRelationships.Get}{propertyRoute}",
                        Method = RouteVerbs.Get
                    }
                });
            }
            return hateoasLinks;
        }

        private List<HateoasLink> AddSelfHateoasLinks<T>(T data, UrlHelper urlHelper, string routePath) 
            where T: EntityBase
        {
            return new List<HateoasLink>
            {
                new HateoasLink()
                {
                    Href = urlHelper.Link($"{routePath}{RouteVerbs.Get}", new {id = data.Id}), 
                    Relationship = RouteRelationships.Self,
                    Method = RouteVerbs.Get
                },
                new HateoasLink()
                {
                    Href = urlHelper.Link($"{routePath}{RouteVerbs.Patch}", new {id = data.Id}), 
                    Relationship = $"{RouteRelationships.Update}{data.GetType().Name}",
                    Method = RouteVerbs.Patch
                },
                new HateoasLink()
                {
                    Href = urlHelper.Link($"{routePath}{RouteVerbs.Delete}", new {id = data.Id}), 
                    Relationship = $"{RouteRelationships.Delete}{data.GetType().Name}",
                    Method = RouteVerbs.Delete
                },
            };
        }
    }
}