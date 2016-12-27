namespace DDI.WebApi.Services.Search
{
    public class PageableSearch : IPageable
    {
        #region IPageable
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public string OrderBy { get; set; }
        #endregion
    }
}