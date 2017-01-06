using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains extension methods for <see cref="MemberInfo" />. These are mostly convenience and
/// shortcut methods to keep code cleaner by handling null checks and other logic.
/// </summary>
/// <remarks>
/// Some of these methods were originally defined in <see cref="TypeExtensions" /> but were moved to
/// cover the parent Type, <see cref="MemberInfo" />.
/// </remarks>
public static class MemberInfoExtensions
{
    #region Public Methods

    /// <summary>
    /// If this <see cref="MemberInfo" /> is annotated with the given <see cref="Attribute" /> type ,
    /// that attribute is returned. Otherwise, <c>null</c>.
    /// </summary>
    /// <remarks>
    /// Note that this will only return a single instance of the attribute. If the attribute can be
    /// defined multiple times, use <see cref="GetAttributes{T}(Type, bool)" /> instead. This will
    /// return the first discovered attribute definition.
    /// </remarks>
    /// <typeparam name="TAttribute">Attribute type</typeparam>
    /// <param name="self">This <see cref="MemberInfo" /></param>
    /// <param name="inherit">If true, all parent types will be checked as well</param>
    /// <returns></returns>
    public static TAttribute GetAttribute<TAttribute>(this MemberInfo self, bool inherit = false)
        where TAttribute : Attribute
    {
        if (self.IsAttributeDefined<TAttribute>(inherit))
        {
            return self.GetCustomAttribute<TAttribute>();
        }

        return null;
    }

    /// <summary>
    /// Returns an instance of the given <see cref="Attribute" /> type, if it is defined. Otherwise,
    /// a default instance of the attribute will be created and returned.
    /// </summary>
    /// <remarks>This method will never return <c>null</c>.</remarks>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="self"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static TAttribute GetAttributeOrDefault<TAttribute>(this MemberInfo self, bool inherit = false)
        where TAttribute : Attribute
    {
        var attribute = self.GetAttribute<TAttribute>(inherit);
        return attribute ?? default(TAttribute);
    }

    /// <summary>
    /// If this <see cref="MemberInfo" /> is annotated with the given <see cref="Attribute" /> type ,
    /// all instances of that attribute type are returned. Otherwise, an empty list.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type</typeparam>
    /// <param name="type">This <see cref="MemberInfo" /></param>
    /// <param name="inherit">If true, all parent types will be checked as well</param>
    /// <returns></returns>
    public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this MemberInfo self, bool inherit = false)
        where TAttribute : Attribute
    {
        var attributes = new List<TAttribute>();

        if (self.IsAttributeDefined<TAttribute>())
        {
            attributes.AddRange(self.GetCustomAttributes<TAttribute>());
        }

        return attributes;
    }

    /// <summary>
    /// Determines whether or not this <see cref="MemberInfo" /> has an <see cref="Attribute" /> of
    /// the given Type defined.
    /// <para>
    /// To access the defined attribute(s), use <see cref="GetAttribute{T}(MemberInfo, bool)" /> or
    /// <see cref="GetAttributes{T}(MemberInfo, bool)" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TAttribute">Attribute Type</typeparam>
    /// <param name="self"></param>
    /// <param name="inherit">If true, all parent types well be checked as well</param>
    /// <returns></returns>
    public static bool IsAttributeDefined<TAttribute>(this MemberInfo self, bool inherit = false)
    {
        return self.IsDefined(typeof(TAttribute), inherit);
    }

    /// <summary>
    /// Determines whether or not this has an <see cref="Attribute" /> of the given Type defined.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="attributeType">Attribute Type</param>
    /// <param name="inherit">If true, all parent types well be checked as well</param>
    /// <returns></returns>
    public static bool IsAttributeDefined(this MemberInfo self, Type attributeType, bool inherit = false)
    {
        return self.IsDefined(attributeType, inherit);
    }

    #endregion Public Methods
}
