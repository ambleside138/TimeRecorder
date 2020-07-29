using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TimeRecorder.Domain
{
    /// <summary>
    /// 値オブジェクトの基底クラスを表します
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject<T>: IEquatable<T> where T : ValueObject<T>
    {
        public bool Equals(T other)
        {
            if (other == null)
                return false;

            if (GetType() != other.GetType())
                return false;

            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }

        public override bool Equals(object obj)
        {
            if ((obj is T) == false)
                return false;

            return Equals((T)obj);
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public static bool operator ==(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            if (vo1 is null && vo2 is null)
                return true;

            if (vo1 is null || vo2 is null)
                return false;

            return Equals(vo1, vo2);
        }

        public static bool operator !=(ValueObject<T> vo1, ValueObject<T> vo2)
        {
            return Equals(vo1, vo2) == false;
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
