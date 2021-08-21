using System;
using UnityEngine;

namespace Drepanoid
{
    [Serializable]
    public class SerializableNullable<T> where T : struct
    {
        [SerializeField] private T _value;
        public bool HasValue;

        public T? ToNullable
        {
            get => HasValue ? _value : null as T?;
            set
            {
                HasValue = value.HasValue;
                if (HasValue) _value = value.Value;
            }
        }

        public T Value => ToNullable.Value;

        public SerializableNullable () { }

        public SerializableNullable (T? value)
        {
            ToNullable = value;
        }
    }
}