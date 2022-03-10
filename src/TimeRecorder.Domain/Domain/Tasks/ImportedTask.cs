using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Tasks;

public class ImportedTask
{
    public string ImportKey { get; set; }

    public string Title { get; set; }

    public string Source { get; set; }

    public DateTime CreateDateTime { get; set; }
}
