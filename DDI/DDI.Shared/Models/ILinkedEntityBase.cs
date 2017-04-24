using System;

namespace DDI.Shared.Models
{
    public interface ILinkedEntityBase
    {
        string EntityType { get; set; }
        Guid? ParentEntityId { get; set; }
    }
}