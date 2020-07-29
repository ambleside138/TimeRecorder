using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.WorkProcesses;

namespace TimeRecorder.Repository.SQLite.WorkProcesses.Dao
{
    class WorkProcessTableRow
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public WorkProcess ToDomainObject()
        {
            return new WorkProcess(new Identity<WorkProcess>(Id), Title);
        }

    }
}
