using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility
{
    public class Identity<T> : ValueObject<Identity<T>>
    {
        private const int TemporaryIdValue = -1;

        public int Value { get; }

        internal Guid TempValue { get; private set; }

        public Identity(int id)
        {
            Value = id;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            if(IsTemporary)
            {
                yield return TempValue;
            }
            else
            {
                yield return Value;
            }
        }

        public bool IsTemporary => Value == TemporaryIdValue;

        public static Identity<T> Temporary => new Identity<T>(TemporaryIdValue) { TempValue = Guid.NewGuid() };
        public static Identity<T> Empty => new Identity<T>(0);

       
    }
}
