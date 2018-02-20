using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMBase
{
    internal class TypedStorage : Dictionary<Type, object>
    {
        //private Dictionary<string, object> storage = new Dictionary<string, object>();

        public Dictionary<string, T> GetDictionary<T>(Type type)
        {
            if (TryGetValue(type, out var dictionary))
            {
                return (Dictionary<string, T>)dictionary;
            }

            Add(type, new Dictionary<string, T>());
            return (Dictionary<string, T>)this[type];
        }

        //public Dictionary<string, object> GetDictionary<T>(Type type)
        //{
        //    return storage;
        //}

    }
}
