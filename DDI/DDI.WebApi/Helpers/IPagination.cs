using DDI.Shared;
using System.Web.Http.Routing;

namespace DDI.WebApi.Helpers
{
    public interface IPagination
    {
        void AddPaginationHeaderToResponse<T>(UrlHelper urlHelper, T search, int? totalCount, string routeName)
            where T : IPageable;
    }
}