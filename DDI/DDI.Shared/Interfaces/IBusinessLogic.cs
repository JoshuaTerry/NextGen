﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IBusinessLogic
    {
        IUnitOfWork UnitOfWork { get; }
    }
}