
namespace DDI.EFAudit.Models
{
    public interface IPropertyChange<TPrincipal>
    {
        IObjectChange<TPrincipal> ObjectChange { get; set; }
        string PropertyName { get; set; }
        string OriginalValue { get; set; }
        string Value { get; set; } 
        string ChangeType { get; set; }        
    }
}
