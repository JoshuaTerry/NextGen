using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{ 
    public enum TransactionStatus { NonPosting = 0, Pending = 1, Unposted = 2, Posted = 3, Reversed = 4, Deleted = 5 }
}
