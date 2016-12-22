﻿using System.Collections.Generic;
using DDI.Shared;

namespace DDI.WebApi.Services
{
    public class DataResponse : IDataResponse
    {
        public int? TotalResults { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string> ErrorMessages { get; set; }
        public List<string> VerboseErrorMessages { get; set; }
        public List<HATEOASLink> Links { get; set; }

        public DataResponse()
        {
            IsSuccessful = true;
            ErrorMessages = new List<string>();
            VerboseErrorMessages = new List<string>();
            Links = new List<HATEOASLink>();
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