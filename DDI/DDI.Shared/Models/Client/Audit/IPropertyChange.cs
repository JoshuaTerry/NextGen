using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Audit
{
    public interface IPropertyChange<TPrincipal>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        Guid Id { get; set; }
        Guid ObjectChangeId { get; set; }
        IObjectChange<TPrincipal> ObjectChange { get; set; }
        string PropertyName { get; set; }
        string OriginalValue { get; set; }
        string Value { get; set; } 
        string ChangeType { get; set; }        
    }
}
