using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Shared;

public interface IIdentifiable<T>
{
    bool IsMatch(T id);

    T GetIdentity();
}
