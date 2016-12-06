using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IPaymentPreference
    {
        Guid Id { get; set; }
        string Name { get; set; }
        int ABANumber { get; set; }
        string AccountNumber { get; set; }
        string AccountType { get; set; }
    }
}
