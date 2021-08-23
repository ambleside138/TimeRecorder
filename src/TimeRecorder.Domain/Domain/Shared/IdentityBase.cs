using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Shared
{
    public record IdentityBase
    {
        public string Value { get; }

        internal Guid TempValue { get; set; } = Guid.Empty;

        public IdentityBase(string id)
        {
            Value = id;
        }

        //public static IdentityBase CreateForTemporary() 
        //    => new IdentityBase("") { TempValue = Guid.NewGuid() };
    }
}
