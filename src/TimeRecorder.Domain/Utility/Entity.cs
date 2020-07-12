using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace TimeRecorder.Domain.Utility
{
    public abstract class Entity<T> : IEquatable<T> where T : Entity<T>
    {

        protected abstract IEnumerable<object> GetIdentityValues();


        public bool Equals(T other)
        {
            if (other == null)
                return false;

            if (GetType() != other.GetType())
                return false;

            return GetIdentityValues().SequenceEqual(other.GetIdentityValues());
        }

        public override bool Equals(object obj)
        {
            if ((obj is T) == false)
                return false;

            return Equals((T)obj);
        }

        public override int GetHashCode()
        {
            return GetIdentityValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
    }
}
