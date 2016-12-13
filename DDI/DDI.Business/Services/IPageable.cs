namespace DDI.Business.Services
{
    public interface IPageable
    {
        int? Offset { get; set; }
        int? Limit { get; set; }
        string OrderBy { get; set; }
    }
}