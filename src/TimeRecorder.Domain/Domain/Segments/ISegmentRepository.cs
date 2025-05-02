using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.Domain.Domain.Segments;
public interface ISegmentRepository : IRepository
{
    Segment[] SelectAll();
}
