using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Routing;
using DDI.Shared;
using DDI.Shared.Attributes;
using DDI.Shared.Extensions;
using DDI.Shared.Models;
using DDI.Shared.Statics;
using Microsoft.Ajax.Utilities;

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
            if (response.Data is IEnumerable<ICanTransmogrify>)
            {
                dynamicResponse.Data = ToDynamicList((response.Data as IEnumerable<ICanTransmogrify>), urlHelper, fields, shouldAddHateoasLinks);
            }
            else if (response.Data is ICanTransmogrify)
            {
                dynamicResponse.Data = ToDynamicObject((response.Data as ICanTransmogrify), urlHelper, fields, shouldAddHateoasLinks);
            }
            else if (IsSimple(response.Data.GetType()))
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

        public dynamic ToDynamicList<T>(IEnumerable<T> data, UrlHelper urlHelper, string fields = null, bool shouldAddHateoasLinks = true)
            where T : ICanTransmogrify
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null && !shouldAddHateoasLinks)
            {
                return data;
            }
            var upperCaseFields = fields?.ToUpper();
            if (!string.IsNullOrWhiteSpace(upperCaseFields) && !upperCaseFields.Contains("LINKS"))
            {
                shouldAddHateoasLinks = false;
            }
            var listOfFields = upperCaseFields?.Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields, shouldAddHateoasLinks);
            return result;
        }

        public dynamic ToDynamicObject<T>(T data, UrlHelper urlHelper, string fields = null, bool shouldAddHateoasLinks = true)
            where T : ICanTransmogrify
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = null;
            }
            if (fields == null && !shouldAddHateoasLinks)
            {
                return data;
            }
            var upperCaseFields = fields?.ToUpper();
            if (!string.IsNullOrWhiteSpace(upperCaseFields) && !upperCaseFields.Contains("LINKS"))
            {
                shouldAddHateoasLinks = false;
            }
            var listOfFields = upperCaseFields?.Split(',').ToList() ?? new List<string>();
            var result = RecursivelyTransmogrify(data, urlHelper, listOfFields, shouldAddHateoasLinks);
            return result;
        }

        private dynamic RecursivelyTransmogrify<T>(T data, UrlHelper urlHelper, List<string> fieldsToInclude = null, bool shouldAddHateoasLinks = true, IEnumerable<Guid> visited = null)
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
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as IEnumerable<ICanTransmogrify>), urlHelper, strippedFieldList, shouldAddHateoasLinks, visited); 
                }
                else if (fieldValue is ICanTransmogrify && (includeEverything || fieldsToInclude.Any(a => a.StartsWith($"{propertyNameUppercased}."))))  
                {
                    var strippedFieldList = StripFieldList(fieldsToInclude, propertyNameUppercased);
                    ((IDictionary<string, object>) returnObject)[property.Name] = RecursivelyTransmogrify((fieldValue as ICanTransmogrify), urlHelper, strippedFieldList, shouldAddHateoasLinks, visited); 
                }
                else if (includeEverything || fieldsToInclude.Contains(propertyNameUppercased))
                {
                    ((IDictionary<string, object>) returnObject)[property.Name] = fieldValue;
                }
            }
            if (shouldAddHateoasLinks)
            {
                returnObject = AddHateoasLinks(returnObject, data as ICanTransmogrify, urlHelper, fieldsToInclude);
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

        public dynamic RecursivelyTransmogrify<T>(IEnumerable<T> entities, UrlHelper urlHelper, List<string> fieldsToInclude = null, bool shouldAddLinks = true, IEnumerable<Guid> visited = null)
            where T : ICanTransmogrify
        {
            var list = new List<ExpandoObject>();
            foreach (var item in entities)
            {
                list.Add(RecursivelyTransmogrify(item, urlHelper, fieldsToInclude, shouldAddLinks, visited));
            }
        
            return list;
        }
        

        private dynamic AddHateoasLinks<T>(IDictionary<string, object> entity, T data, UrlHelper urlHelper, List<string> fieldsToInclude )
            where T: ICanTransmogrify
        {
            string routePath = data.GetType().GetCustomAttribute<HateoasAttribute>()?.RouteName;
            if (!string.IsNullOrWhiteSpace(routePath))
            {
                var links = AddSelfHateoasLinks(data, urlHelper, routePath, fieldsToInclude);
                links.AddRangeNullSafe(FindAllAddChildHateoasLinks(data, urlHelper, routePath, fieldsToInclude));
                entity.Add("Links", links);
            }
            return entity;
        }

        private IEnumerable<HateoasLink> FindAllAddChildHateoasLinks<T>(T data, UrlHelper urlHelper, string routePath, List<string> fieldsToInclude) where T : ICanTransmogrify
        {
            List<HateoasLink> hateoasLinks = new List<HateoasLink>();
            var properties = data.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(HateoasCollectionLinkAttribute)));
            bool doesIncludeAllLinks = (fieldsToInclude?.Count == 0) || fieldsToInclude.Contains("LINKS");
            foreach (var propertyInfo in properties)
            {
                string propertyRoute = propertyInfo.GetAttribute<HateoasCollectionLinkAttribute>().RouteName;
                if (doesIncludeAllLinks || fieldsToInclude.Contains($"LINKS.{propertyRoute.ToUpper()}"))
                {
                    try
                    {
                        hateoasLinks.Add(new HateoasLink
                        {
                            Href = urlHelper.Link($"{propertyRoute}{RouteVerbs.Post}", null),
                            Relationship = $"{RouteRelationships.New}{propertyRoute}",
                            Method = RouteVerbs.Post
                        });
                    }
                    catch (ArgumentException)
                    {
                        //Ignore the error if the route doesn't exist
                    }
                    try
                    {
                        hateoasLinks.Add(new HateoasLink
                        {
                            // For speeds sake, try and just use the id. If that returns null, that means there are other values that are needed. In that case use the whole data object.
                            Href = urlHelper.Link($"{routePath}{propertyRoute}", new
                            {
                                id = data.Id
                            })
                                   ?? urlHelper.Link($"{routePath}{propertyRoute}", data).SubstringUpToFirst('?'),
                            Relationship = $"{RouteRelationships.Get}{propertyRoute}",
                            Method = RouteVerbs.Get
                        });
                    }
                    catch (ArgumentException)
                    {
                        //Ignore the error if the route doesn't exist
                    }
                }
            }

            properties = data.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(HATEOASLookupCollectionAttribute)));
            foreach (var propertyInfo in properties)
            {
                string propertyRoute = propertyInfo.GetAttribute<HATEOASLookupCollectionAttribute>().RouteName;
                if (doesIncludeAllLinks || fieldsToInclude.Contains($"LINKS.{propertyRoute.ToUpper()}"))
                {
                    try
                    {
                        hateoasLinks.Add(new HateoasLink
                        {
                            Href = urlHelper.Link($"{routePath}{propertyRoute}{RouteVerbs.Post}", new {id = data.Id}),
                            Relationship = $"{RouteRelationships.New}{propertyRoute}",
                            Method = RouteVerbs.Post
                        });
                    }
                    catch (ArgumentException)
                    {
                        //Ignore the error if the route doesn't exist
                    }
                    try
                    {
                        hateoasLinks.Add(new HateoasLink
                        {
                            // For speeds sake, try and just use the id. If that returns null, that means there are other values that are needed. In that case use the whole data object.
                            Href = urlHelper.Link($"{routePath}{propertyRoute}", new
                            {
                                id = data.Id
                            })
                                   ?? urlHelper.Link($"{routePath}{propertyRoute}", data).SubstringUpToFirst('?'),
                            Relationship = $"{RouteRelationships.Get}{propertyRoute}",
                            Method = RouteVerbs.Get
                        });
                    }
                    catch (ArgumentException)
                    {
                        //Ignore the error if the route doesn't exist
                    }
                }
            }

            return hateoasLinks;
        }

        private List<HateoasLink> AddSelfHateoasLinks<T>(T data, UrlHelper urlHelper, string routePath, List<string> fieldsToInclude) 
            where T: ICanTransmogrify
        {
            var returnList = new List<HateoasLink>();
            bool doesIncludeAllLinks = (fieldsToInclude?.Count == 0) || fieldsToInclude.Contains("LINKS");
            if (doesIncludeAllLinks || fieldsToInclude.Contains("LINKS.SELF"))
            {
                returnList.Add(new HateoasLink()
                {
                    Href = urlHelper.Link($"{routePath}{RouteVerbs.Get}", new
                    {
                        id = data.Id
                    }),
                    Relationship = RouteRelationships.Self,
                    Method = RouteVerbs.Get
                });
            }
            if (doesIncludeAllLinks || fieldsToInclude.Contains("LINKS.PATCH"))
            {
                returnList.Add(new HateoasLink()
                {
                    Href = urlHelper.Link($"{routePath}{RouteVerbs.Patch}", new
                    {
                        id = data.Id
                    }),
                    Relationship = $"{RouteRelationships.Update}{data.GetType().Name}",
                    Method = RouteVerbs.Patch
                });
            }
            if (doesIncludeAllLinks || fieldsToInclude.Contains("LINKS.DELETE"))
            {
                returnList.Add(new HateoasLink()
                {
                    Href = urlHelper.Link($"{routePath}{RouteVerbs.Delete}", new
                    {
                        id = data.Id
                    }),
                    Relationship = $"{RouteRelationships.Delete}{data.GetType().Name}",
                    Method = RouteVerbs.Delete
                });
            }
            return returnList;
        }
    }
}