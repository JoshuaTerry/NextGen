using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace DDI.EFAudit.Helpers
{
    public static class ObjectExtensions
    {
        private const BindingFlags DefaultBindingFlags =
            (BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);

        private static readonly MethodInfo ShallowCloneMethod = 
            typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static T DeepCopy<T>(this T original)
        {
            return (T)DeepCopy((Object)original);
        }

        public static object DeepCopy(this object original)
        {
            return DeepCopy(original, new Dictionary<Object, Object>(new ReferenceEqualityComparer()));
        }

        private static object DeepCopy(object original, IDictionary<object, object> visited)
        {
            if (original == null) 
                return null;

            var typeToReflect = original.GetType();

            if (IsPrimitive(typeToReflect)) 
                return original;

            if (visited.ContainsKey(original)) 
                return visited[original];

            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) 
                return null;

            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                var arrayClone = (Array)ShallowCloneMethod.Invoke(original, null);

                if (IsPrimitive(arrayType) == false)
                {
                    for (var i = 0; i < arrayClone.Length; i++)
                    {
                        arrayClone.SetValue(DeepCopy(arrayClone.GetValue(i), visited), i);
                    }
                }

                visited.Add(original, arrayClone);

                return arrayClone;
            }

            if (typeToReflect.IsCollection())
            {
                var collectionType = typeToReflect.GetCollectionElementType();
                var collectionClone = typeof(ObjectExtensions).GetMethod("CopyCollection", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .MakeGenericMethod(new Type[] { collectionType })
                    .Invoke(null, new object[] { original, visited, typeToReflect });

                visited.Add(original, collectionClone);

                return collectionClone;
            }

            var clone = ShallowCloneMethod.Invoke(original, null);
            visited.Add(original, clone);
            CopyFields(original, visited, clone, typeToReflect);
            CopyInheritedFields(original, visited, clone, typeToReflect);

            return clone;
        }

        private static void CopyFields(object original, IDictionary<object, object> visited, object clone, Type typeToReflect,
            BindingFlags bindingFlags = DefaultBindingFlags, Func<FieldInfo, bool> fieldFilter = null)
        {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (fieldFilter != null && !fieldFilter(fieldInfo)) 
                    continue;
                if (IsIgnoreable(fieldInfo.GetCustomAttributes(true)))
                    continue;
                if (IsPrimitive(fieldInfo.FieldType)) 
                    continue;

                var originalFieldValue = fieldInfo.GetValue(original);
                var clonedFieldValue = DeepCopy(originalFieldValue, visited);
                fieldInfo.SetValue(clone, clonedFieldValue);
            }
        }

        private static void CopyInheritedFields(object original, IDictionary<object, object> visited, object clone, Type typeToReflect)
        {
            if (typeToReflect.BaseType == null) 
                return;

            CopyFields(original, visited, clone, typeToReflect.BaseType, (BindingFlags.Instance | BindingFlags.NonPublic), fi => fi.IsPrivate);
            CopyInheritedFields(original, visited, clone, typeToReflect.BaseType);
        }

        private static object CopyCollection<T>(object original, IDictionary<object, object> visited, Type typeToReflect)
        {
            var collectionClone = (ICollection<T>)Activator.CreateInstance(typeToReflect);

            foreach (var item in (IEnumerable)original)
            {
                collectionClone.Add((T)item);
            }

            return collectionClone;
        }

        private static bool IsIgnoreable(this object[] attributes)
        {
            return attributes.Select(a => a.GetType()).Any(t => 
                t == typeof (NonSerializedAttribute) || 
                t == typeof (SoapIgnoreAttribute) || 
                t == typeof (XmlIgnoreAttribute)
            );
        }

        private static bool IsPrimitive(this Type type)
        {
            return ((type.IsValueType & type.IsPrimitive) || (type == typeof(String)));
        }

        private static bool IsCollection(this Type type)
        {
            return type.GetInterfaces().Any(t => 
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(ICollection<>)
            );
        }

        private static Type GetCollectionElementType(this Type type)
        {
            var collectionType = type.GetInterfaces().FirstOrDefault(t =>
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == typeof(ICollection<>)
            );

            if (collectionType == null)
                return null;

            return collectionType.GetGenericArguments().FirstOrDefault();
        }
    }
}
