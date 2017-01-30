using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Extension methods for Guid and nullable Guid values.
/// </summary>
public static class GuidExtensions
{
    /// <summary>
    /// Returns true if a nullable Guid is null or is equal to Guid.Empty.
    /// </summary>
    public static bool IsNullOrEmpty(this Guid? guid)
    {
        return (guid == null || guid.Value == Guid.Empty);
    }

    /// <summary>
    /// Returns true if a Guid value is equal to Guid.Empty.
    /// </summary>
    public static bool IsEmpty(this Guid guid)
    {
        return (guid == Guid.Empty);
    }
}
