﻿using System;
using System.Data.Entity;

namespace DDI.EFAudit.Logging.ValuePairs
{
    public interface IValuePair
    {
        bool HasChanged { get; }
        string PropertyName { get; }
        Func<object> OriginalValue { get; }
        Func<object> NewValue { get; }
        EntityState State { get; }
    }
}
