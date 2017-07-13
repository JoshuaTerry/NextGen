using System;

namespace DDI.Shared.Attributes.Models
{
    /// <summary>
    /// Specifies that a string property should have a db type of nvarchar(max).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TextAttribute : Attribute
    {
    }
}
