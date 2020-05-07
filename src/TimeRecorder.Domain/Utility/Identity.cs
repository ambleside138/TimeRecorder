using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Utility
{
    public class Identity<T> : ValueObject<Identity<T>>
    {

        public int Value { get; }

        internal Guid TempValue { get; private set; }

        public Identity(int id)
        {
            Value = id;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            if(Value >= 0)
            {
                yield return Value;
            }
            else
            {
                yield return TempValue;
            }
        }

        public static Identity<T> Temporary => new Identity<T>(-1) { TempValue = Guid.NewGuid() };
        public static Identity<T> Empty => new Identity<T>(0);

       
    }
}
