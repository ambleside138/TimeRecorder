using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tasks
{
    public class WorkTaskImportSource : ValueObject<WorkTaskImportSource>
    {
        public string Key { get; private set; }

        public string Kind { get; private set; }

        public WorkTaskImportSource(string key, string kind)
        {
            Key = key;
            Kind = kind;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Kind;
        }
    }
}
