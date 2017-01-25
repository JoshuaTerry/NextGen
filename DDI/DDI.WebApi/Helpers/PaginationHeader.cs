namespace DDI.WebApi.Helpers
{
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