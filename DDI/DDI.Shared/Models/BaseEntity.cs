using System;

namespace DDI.Shared.Models
{
    /// <summary>
    /// Base class for all entity model classes.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        public abstract Guid Id { get; set; }

        public virtual string DisplayName => string.Empty;

        public override string ToString()
        {
            return DisplayName;
        }

    }


}
