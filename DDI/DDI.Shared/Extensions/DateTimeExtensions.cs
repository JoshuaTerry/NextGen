﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Extension Methods for DateTime and DateTime? that ensure datetime values will not throw exceptions when stored in SQL columns.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns TRUE if DateTime value is &gt;= SqlDateTime.MinValue
    /// </summary>
    public static bool IsValidSQLDate(this DateTime? dt)
    {
        return dt != null && 
               dt.Value >= System.Data.SqlTypes.SqlDateTime.MinValue.Value && 
               dt.Value <= System.Data.SqlTypes.SqlDateTime.MaxValue.Value;
    }

    /// <summary>
    /// Returns TRUE if DateTime value is &gt;= SqlDateTime.MinValue
    /// </summary>
    public static bool IsValidSQLDate(this DateTime dt)
    {
        return dt >= System.Data.SqlTypes.SqlDateTime.MinValue.Value &&
               dt <= System.Data.SqlTypes.SqlDateTime.MaxValue.Value;
    }

    /// <summary>
    /// Returns TRUE if DateTime value has a time of day.
    /// </summary>
    public static bool HasTime(this DateTime? dt)
    {
        return dt.HasValue && dt.Value.TimeOfDay.Ticks > 0;
    }

    /// <summary>
    /// Returns TRUE if DateTime value has a time of day.
    /// </summary>
    public static bool HasTime(this DateTime dt)
    {
        return dt.TimeOfDay.Ticks > 0;
    }

}

