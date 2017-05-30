using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface ITestRepository<T> : IRepository<T> where T : class
    {
        string ModifiedPropertyList { get; set; }
        void Clear();
    }
}
