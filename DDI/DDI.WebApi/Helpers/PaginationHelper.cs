using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;
using DDI.Services.Extensions;
using DDI.Services.Search;
using DDI.Shared;

namespace DDI.WebApi.Helpers
{
    public class PaginationHelper
    {
        public static void AddPaginationHeaderToResponse<T>(UrlHelper urlHelper, T search, int totalPages, int? totalCount, string routeName)
            where T : IPageable
        {
            var paginationHeader = CreatePaginationHeader(urlHelper, search, totalPages, totalCount, routeName);
            HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
        }

        public static object CreatePaginationHeader<T>(UrlHelper urlHelper, T search, int totalPages, int? totalCount, string routeName)
            where T : IPageable
        {
            int? offset = search.Offset;
            int? limit = search.Limit;
            var prevLink = offset > 0 ? urlHelper.Link(routeName, search.ForPreviousPage()) : "";
            var nextLink = offset + 1 < totalPages ? urlHelper.Link(routeName, search.ForNextPage()) : "";
            var firstLink = urlHelper.Link(routeName, search.ForFirstPage());
            var lastLink = urlHelper.Link(routeName, search.ForLastPage(totalPages));


            var paginationHeader = new
            {
                currentPage = offset + 1,
                pageSize = limit,
                totalCount = totalCount,
                totalPages = totalPages,
                previousPageLink = prevLink,
                nextPageLink = nextLink,
                firstPageLink = firstLink,
                lastPageLink = lastLink
            };
            return paginationHeader;
        }
    }
}