using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public interface IEntity : ICanTransmogrify
    {
        string DisplayName { get; }
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedOn { get; set; }
    }
}
