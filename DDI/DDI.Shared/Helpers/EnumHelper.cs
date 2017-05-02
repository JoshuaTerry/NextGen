using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DDI.Shared.Helpers
{
    public static class EnumHelper
    {

        /// <summary>
        /// Convert an object to an Enum value.
        /// </summary>
        public static T ConvertToEnum<T>(object obj, T defaultValue = default(T)) where T : struct
        {
            if (obj is T)
            {
                return (T)obj;
            }
            else if (obj is string)
            {
                return GetBestMatch<T>((string)obj, defaultValue);
            }
            else if (obj is int)
            {
                return (T)obj;
            }

            return defaultValue;
        }

        /// <summary>
        /// Get the value of the Description attribute for an Enum member
        /// </summary>
        /// <returns>The string value of the Description attribute</returns>
        public static string GetDescription(object enumValue)
        {
            string definition = string.Empty;

            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            object[] attributes = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attributes?.Length > 0)
            {
                definition = ((DescriptionAttribute)attributes[0]).Description;
            }

            return definition;
        }

        public static Dictionary<string, int> GetDescriptions<T>()
        {
            var values = new Dictionary<string, int>();
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type of argument must be Enum.");
            }

            var type = typeof (T);

            FieldInfo[] fields = type.GetFields();

            foreach (var field in fields)
            {
                object[] attributes = field.GetCustomAttributes(typeof (DescriptionAttribute), false);
                foreach (var attribute in attributes)
                {
                    values.Add(((DescriptionAttribute) attribute).Description, (int)field.GetValue(field));
                }
            }

            return values;
        } 

        /// <summary>
        /// Given a string, return the enum value whose field name, numeric value, or description attribute best matches the string.
        /// </summary>
        private static T GetBestMatch<T>(string matchString, T defaultValue) where T : struct
        {
            Type t = typeof(T);
            FieldInfo[] fields = t.GetFields();
            var candidates = new Dictionary<string, FieldInfo>();

            foreach (var fieldInfo in fields.Where(p => p.IsStatic))
            {
                DescriptionAttribute attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    candidates[attribute.Description] = fieldInfo; // Description attribute
                }
                candidates[fieldInfo.Name] = fieldInfo;  // Enum field name
                candidates[((int)fieldInfo.GetValue(null)).ToString()] = fieldInfo; // Enum field integer value
            }

            string bestMatch = StringHelper.GetBestMatch(matchString, candidates.Select(p => p.Key).ToArray());

            if (bestMatch != null)
            {
                return (T)candidates[bestMatch].GetValue(null);
            }

            return defaultValue;
        }


    }

}
