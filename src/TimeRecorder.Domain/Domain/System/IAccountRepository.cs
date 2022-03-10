using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.System;

public interface IAccountRepository
{
    bool IsSignined();

    LoginStatus Signin();

    void Signout();
}
