using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DDI.Shared.Extensions
{
    public static class JsonExtensions
    {
        public static KeyValuePair<string, object> ConvertToType<T1>(KeyValuePair<string, JToken> pair)
        {
            var returnValue = new KeyValuePair<string, object>();

            var type = typeof(T1);
            var property = type.GetProperty(pair.Key);

            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                returnValue = !pair.Value.IsNullOrEmpty() ? new KeyValuePair<string, object>(pair.Key, pair.Value.ToObject(Nullable.GetUnderlyingType(property.PropertyType))) : new KeyValuePair<string, object>(pair.Key, null);
            }
            else
            {
                if (pair.Value.IsNullOrEmpty())
                {
                    throw new NullReferenceException("Updated value of property cannot be null or empty.");
                }

                returnValue = new KeyValuePair<string, object>(pair.Key, pair.Value.ToObject(property.PropertyType));
            }

            return returnValue;
        }

        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == string.Empty) ||
                   (token.Type == JTokenType.String && token.ToString().ToLower() == "null") ||
                   (token.Type == JTokenType.Null);
        }
    }
}
