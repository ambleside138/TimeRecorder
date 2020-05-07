using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TimeRecorder.Domain.Domain.Hospitals;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tasks.Definitions;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tasks
{
    public class TaskModel
    {

        public string Title { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public Product Product { get; set; }

        public Identity<Hospital> HospitalId { get; set; }

        public Identity<Process> ProcessId { get; set; }

        public string Remarks { get; set; }


        private TaskModel()
        {

        }
    }
}
