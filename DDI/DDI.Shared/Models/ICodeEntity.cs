namespace DDI.Shared.Models
{
    public interface ICodeEntity
    {
        string Code { get; set; }
        string Name { get; set; }
        bool IsActive { get; set; }
    }
}
