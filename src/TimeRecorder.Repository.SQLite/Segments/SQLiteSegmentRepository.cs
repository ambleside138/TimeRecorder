using System;
using System.Collections.Generic;
using System.Linq;
using TimeRecorder.Domain.Domain.Segments;
using TimeRecorder.Repository.SQLite.Segments.Dao;

namespace TimeRecorder.Repository.SQLite.Segments;
public class SQLiteSegmentRepository : ISegmentRepository
{
    public Segment[] SelectAll()
    {
        var list = new List<Segment>();

        RepositoryAction.Query(c =>
        {
            var listRow = new SegmentDao(c, null).SelectAll();
            list.AddRange(listRow.Select(r => r.ToDomainObject()));
        });

        return list.ToArray();
    }
}
