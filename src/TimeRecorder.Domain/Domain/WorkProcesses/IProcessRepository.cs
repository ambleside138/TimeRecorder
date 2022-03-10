using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.WorkProcesses;

public interface IWorkProcessRepository : IRepository
{
    WorkProcess Regist(WorkProcess process);

    WorkProcess[] SelectAll();
}
