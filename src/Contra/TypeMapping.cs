using System;

namespace Contra
{
    /// <summary>
    /// A mapping from type to T.
    /// </summary>
    public class TypeMapping<T>
    {
        public Type Key { get; private set; }
        public T MappedValue { get; private set; }

        public TypeMapping(Type key, T mappedValue)
        {
            Key = key;
            MappedValue = mappedValue;
        }

        public bool Matches(Type type)
        {
            return recMatches(type, Key);
        }

        private static bool recMatches(Type msg, Type target)
        {
            if (target.IsAssignableFrom(msg)) return true;

            if (target.IsGenericType == false) return false;
            if (msg.IsGenericType == false) return false;

            if (target.GetGenericTypeDefinition().IsAssignableFrom(msg.GetGenericTypeDefinition()) == false) return false;

            var targetTypeParams = target.GetGenericParameterConstraints();
            var msgTypeParams = msg.GetGenericParameterConstraints();

            if (targetTypeParams.Length != msgTypeParams.Length) return false;

            for (int i = 0; i < targetTypeParams.Length; i++)
            {
                if (!recMatches(targetTypeParams[i], msgTypeParams[i])) return false;
            }

            return true;
        }

        public bool InverseMatches(T value)
        {
            return value.Equals(MappedValue);
        }
    }
}