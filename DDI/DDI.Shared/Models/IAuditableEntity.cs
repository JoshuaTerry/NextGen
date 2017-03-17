using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public interface IAuditableEntity
    {
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
    }
}
