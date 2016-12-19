using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data.Models
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
