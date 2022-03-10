using System;
using TimeRecorder.Domain.Domain;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Products;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.WorkProcesses;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.UseCase.Tracking.Reports;

public class WorkingTimeRecordForReport
{
    public YmdString Ymd { get; set; }

    public TaskCategory TaskCategory { get; set; }

    public WorkProcess WorkProcess { get; set; }

    public Product Product { get; set; }

    public Client Client { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string Title { get; set; }

    public Identity<WorkingTimeRange> WorkingTimeId { get; set; }

    public Identity<WorkTask> WorkTaskId { get; set; }

    public bool IsTemporary { get; set; }

    public bool IsScheduled { get; set; }

    public WorkingTimeRange ConvertToWorkingTimeRange()
    {
        return new WorkingTimeRange(WorkingTimeId, WorkTaskId, StartDateTime, EndDateTime);
    }
}
