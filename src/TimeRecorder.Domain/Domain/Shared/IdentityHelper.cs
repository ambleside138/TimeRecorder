using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Shared;

public static class IdentityHelper
{
    public static T CreateTempId<T>() where T : IdentityBase, new()
    {
        return new T() { TempValue = Guid.NewGuid() };
    }
}
