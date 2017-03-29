using DDI.Shared;

namespace DDI.Services.Extensions
{
    public static class PageableSearchExtensions
    {
        public static T ForPreviousPage<T>(this T search)
            where T : IPageable
        {
            if (search.Offset == null || search.Offset == 0)
            {
                return default(T);
            }
            search.Offset = search.Offset - 1 ?? null;
            return search;
        }

        public static T ForNextPage<T>(this T search, int? totalPages = null)
            where T : IPageable
        {
            if (search.Offset == null || search.Offset >= totalPages)
            {
                return default(T);
            }
            search.Offset = search.Offset + 1 ?? null;
            return search;
        }

        public static T ForFirstPage<T>(this T search)
            where T : IPageable
        {
            search.Offset = 0;
            return search;
        }

        public static T ForLastPage<T>(this T search, int numberOfPages)
            where T : IPageable
        {
            search.Offset = numberOfPages - 1;
            return search;
        }
    }
}
