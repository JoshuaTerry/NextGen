using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IDataResponse
    {
        bool IsSuccessful { get; set; }
        List<string> ErrorMessages { get; set; }
        List<string> VerboseErrorMessages { get; set; }
    }

    public interface IDataResponse<T> : IDataResponse
    {
        T Data { get; set; }
    }
}
