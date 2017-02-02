using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
        string DisplayName { get; }
        Guid? CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        Guid? LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
    }
}
