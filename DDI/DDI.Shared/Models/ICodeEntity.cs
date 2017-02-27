﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models
{
    public interface ICodeEntity : IEntity
    {
        string Code { get; set; }
        string Name { get; set; }
    }
}
