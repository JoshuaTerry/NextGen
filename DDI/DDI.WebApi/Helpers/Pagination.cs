using DDI.Shared;
using System;
using System.Web;
using System.Web.Http.Routing;

namespace DDI.WebApi.Helpers
{
    public class Pagination : IPagination
    {
        public void AddPaginationHeaderToResponse<T>(UrlHelper urlHelper, T search, int? totalCount, string routeName)
            where T : IPageable
        {
            var paginationHeader = CreatePaginationHeader(urlHelper, search, totalCount, routeName);
            if (HttpContext.Current != null)
                HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));
        }

        public PaginationHeader CreatePaginationHeader<T>(UrlHelper urlHelper, T search, int? totalCount, string routeName)
            where T : IPageable
        {
            int offset = search.Offset ?? 0;
            int limit = search.Limit ?? int.MaxValue;
            int totalPages = 1;
            if (totalCount.HasValue && totalCount > 0)
            {
                totalPages = (int) Math.Ceiling((double) totalCount / limit);
            }

            //var prevLink = offset > 0 ? urlHelper.Link(routeName, search.ForPreviousPage()) : "";
            //var nextLink = offset + 1 < totalPages ? urlHelper.Link(routeName, search.ForNextPage()) : "";
            //var firstLink = urlHelper.Link(routeName, search.ForFirstPage());
            //var lastLink = urlHelper.Link(routeName, search.ForLastPage(totalPages));


            var paginationHeader = new PaginationHeader
            {
                CurrentPage = offset + 1,
                PageSize = limit,
                TotalCount = totalCount,
                TotalPages = totalPages,
                //PreviousPageLink = prevLink,
                //NextPageLink = nextLink,
                //FirstPageLink = firstLink,
                //LastPageLink = lastLink
            };
            return paginationHeader;
        }
    }
}