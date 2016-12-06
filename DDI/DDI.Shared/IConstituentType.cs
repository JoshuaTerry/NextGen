using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IConstituentType
    {
        Guid Id { get; set; }

        //Question - JLT Is the code still important or was it the Id in the old system?
        string Code { get; set; }

        string Description { get; set; }

        string BaseType { get; set; }

        bool IsActive { get; set; }

        bool IsRequired { get; set; }
    }
}
