﻿using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Data.Models.Common;
using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public interface IGenericService<T>
    {
        IDataResponse<List<T>> GetAll(string orderBy = "");
    }
}