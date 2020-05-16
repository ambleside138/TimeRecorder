using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Processes
{
    public interface IProcessRepository : IRepository
    {
        Process Regist(Process process);

        Process[] SelectAll();
    }
}
