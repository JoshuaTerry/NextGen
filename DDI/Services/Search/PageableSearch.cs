using DDI.Shared;

namespace DDI.Services.Search
{
    public class PageableSearch : IPageable
    {
        #region IPageable
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string OrderBy { get; set; }

        public PageableSearch()
        {
        }

        public PageableSearch(int? offset, int? limit, string orderBy)
        {
            Offset = offset;
            Limit = limit;
            OrderBy = orderBy;
        }

        #endregion
    }
}
