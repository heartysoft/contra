using System;
using System.Collections.Generic;
using System.Linq;

namespace Contra
{
    public class TypeRegistry<T>
    {
        readonly List<TypeMapping<T>> _mappings = new List<TypeMapping<T>>();
        readonly Dictionary<T, List<TypeMapping<T>>> _inverse = new Dictionary<T, List<TypeMapping<T>>>();


        public TypeRegistry<T> Register<T1>(T value)
        {
            var typeMapping = new TypeMapping<T>(typeof(T1), value);

            _mappings.Add(typeMapping);

            var inverseList = getListFor(value);
            inverseList.Add(typeMapping);

            return this;
        }

        public IEnumerable<TypeMapping<T>> GetValuesFor(object key)
        {
            var type = key.GetType();
            var values = _mappings.Where(x => x.Matches(type));

            return values;
        }

        public IEnumerable<TypeMapping<T>> GetKeysFor(T value)
        {
            return _inverse[value];
        }

        private List<TypeMapping<T>> getListFor(T key)
        {
            if (!_inverse.ContainsKey(key))
            {
                var list = new List<TypeMapping<T>>();
                _inverse[key] = list;
                return list;
            }

            return _inverse[key];
        }  
    }
}

