using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IDataResponse
    {
        int? TotalResults { get; set; }
        int? PageSize { get; set; }
        int? PageNumber { get; set; }
        bool IsSuccessful { get; set; }
        List<string> ErrorMessages { get; set; }
        List<string> VerboseErrorMessages { get; set; }
        List<HATEOASLink> Links { get; set; } 
    }

    public interface IDataResponse<T> : IDataResponse
    {
        T Data { get; set; }
    }
}
