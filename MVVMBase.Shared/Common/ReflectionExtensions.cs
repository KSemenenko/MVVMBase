using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MVVMBase
{
    public static class ReflectionExtensions
    {
        /// <param name="type">For internal use by the Xamarin.Forms platform.</param>
        /// <param name="predicate">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static FieldInfo GetField(this Type type, Func<FieldInfo, bool> predicate)
        {
            return type.GetFields().FirstOrDefault(predicate);
        }

        /// <param name="type">For internal use by the Xamarin.Forms platform.</param>
        /// <param name="name">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static FieldInfo GetField(this Type type, string name)
        {
            return type.GetField(fi => fi.Name == name);
        }

        /// <param name="type">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            return GetParts(type, i => i.DeclaredFields);
        }

        /// <param name="type">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            return GetParts(type, ti => ti.DeclaredProperties);
        }

        /// <param name="type">For internal use by the Xamarin.Forms platform.</param>
        /// <param name="name">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static PropertyInfo GetProperty(this Type type, string name)
        {
            TypeInfo typeInfo;
            for(Type type1 = type; type1 != null; type1 = typeInfo.BaseType)
            {
                typeInfo = type1.GetTypeInfo();
                PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(name);
                if(declaredProperty != null)
                {
                    return declaredProperty;
                }
            }

            return null;
        }

        /// <param name="self">For internal use by the Xamarin.Forms platform.</param>
        /// <param name="c">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static bool IsAssignableFrom(this Type self, Type c)
        {
            return self.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
        }

        /// <param name="self">For internal use by the Xamarin.Forms platform.</param>
        /// <param name="o">For internal use by the Xamarin.Forms platform.</param>
        /// <summary>For internal use by the Xamarin.Forms platform.</summary>
        /// <returns>To be added.</returns>
        /// <remarks>To be added.</remarks>
        public static bool IsInstanceOfType(this Type self, object o)
        {
            return self.GetTypeInfo().IsAssignableFrom(o.GetType().GetTypeInfo());
        }

        private static IEnumerable<T> GetParts<T>(Type type, Func<TypeInfo, IEnumerable<T>> selector)
        {
            Type type1 = type;
            while(type1 != null)
            {
                TypeInfo ti = type1.GetTypeInfo();
                foreach(T obj in selector(ti))
                {
                    yield return obj;
                }

                type1 = ti.BaseType;
                ti = null;
            }
        }
    }
}