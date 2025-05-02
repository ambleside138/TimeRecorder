using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain;
using TimeRecorder.Domain.Domain.Segments;

namespace TimeRecorder.Repository.SQLite.Segments.Dao;
internal class SegmentTableRow
{
    public int Id { get; set; }

    public string Name { get; set; }


    public Segment ToDomainObject()
    {
        return new Segment(new Identity<Segment>(Id), Name);
    }
}
