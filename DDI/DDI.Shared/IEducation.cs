using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IEducation
    {
        Guid Id { get; set; }
        string Name { get; set; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        string School { get; set; }
        IEducationLevel Degree { get; set; }
        string Major { get; set; }
    }
}
