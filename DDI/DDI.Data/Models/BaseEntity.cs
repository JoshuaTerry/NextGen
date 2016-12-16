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
    public abstract class BaseEntity
    {
        public virtual string DisplayName => string.Empty;

        public override string ToString()
        {
            return DisplayName;
        }

    }


}
