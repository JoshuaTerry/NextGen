using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DDI.Shared.Helpers
{
    public static class EnumHelper
    {

        /// <summary>
        /// Convert an object (which is expected to be an enum value) to an Enum value.
        /// </summary>
        public static T ConvertToEnum<T>(object obj, T defaultValue = default(T)) where T : struct
        {
            if (obj is T)
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

        public static T FromDescription<T>(string definition) where T : struct, IConvertible
        {
            Type t = typeof(T);
            foreach (FieldInfo fieldInfo in t.GetFields())
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes.Length > 0)
                {
                    foreach (DescriptionAttribute descriptionAttribute in attributes)
                    {
                        if (descriptionAttribute.Description.Equals(definition))
                        {
                            return (T)fieldInfo.GetValue(null);
                        }
                    }
                }
            }
            return default(T);
        }
    }

}
