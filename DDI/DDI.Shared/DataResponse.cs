using System.Collections.Generic;

namespace DDI.Shared
{
    public class DataResponse : IDataResponse
    {
        public int? TotalResults { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string> ErrorMessages { get; set; }
        public List<string> VerboseErrorMessages { get; set; }

        public DataResponse()
        {
            IsSuccessful = true;
            ErrorMessages = new List<string>();
            VerboseErrorMessages = new List<string>();
        }        
    }

    public class DataResponse<T> : DataResponse, IDataResponse<T>
    {
        public T Data { get; set; }

        public DataResponse() : base() { }

        public DataResponse(T data) : this()
        {
            Data = data;
        }
    }
}
