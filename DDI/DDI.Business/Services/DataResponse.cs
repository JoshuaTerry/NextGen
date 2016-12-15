﻿using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public class DataResponse : IDataResponse
    {
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
    }
}