using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IAddress
    {         
        Guid Id { get; set; }         
        string Line1 { get; set; }         
        string Line2 { get; set; }
        string City { get; set; }
        IState State { get; set; }         
        string Zip { get; set; }
    }
}
