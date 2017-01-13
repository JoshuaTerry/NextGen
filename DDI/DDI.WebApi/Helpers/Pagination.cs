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
    public class Pagination : IPagination
    {
        public void AddPaginationHeaderToResponse<T>(UrlHelper urlHelper, T search, int? totalCount, string routeName)
            where T : IPageable
        {
            var paginationHeader = CreatePaginationHeader(urlHelper, search, totalCount, routeName);
            HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
        }

        public PaginationHeader CreatePaginationHeader<T>(UrlHelper urlHelper, T search, int? totalCount, string routeName)
            where T : IPageable
        {
            int? offset = search.Offset;
            int? limit = search.Limit;
            int totalPages = (int) Math.Ceiling((double) totalCount / limit.Value);

            var prevLink = offset > 0 ? urlHelper.Link(routeName, search.ForPreviousPage()) : "";
            var nextLink = offset + 1 < totalPages ? urlHelper.Link(routeName, search.ForNextPage()) : "";
            var firstLink = urlHelper.Link(routeName, search.ForFirstPage());
            var lastLink = urlHelper.Link(routeName, search.ForLastPage(totalPages));


            var paginationHeader = new PaginationHeader
            {
                CurrentPage = offset + 1,
                PageSize = limit,
                TotalCount = totalCount,
                TotalPages = totalPages,
                PreviousPageLink = prevLink,
                NextPageLink = nextLink,
                FirstPageLink = firstLink,
                LastPageLink = lastLink
            };
            return paginationHeader;
        }
    }

    public class PaginationHeader
    {
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public int? TotalCount { get; set; }
        public int TotalPages { get; set; }
        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }
        public string FirstPageLink { get; set; }
        public string LastPageLink { get; set; }

    }
}