using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Clients;

namespace TimeRecorder.Domain.Domain.Segments;
public class Segment : Entity<Segment>
{
    public Identity<Segment> Id { get; set; }

    public string Name { get; set; }

    public static Segment Empty => new(Identity<Segment>.Empty, "未選択");

    public Segment(Identity<Segment> id, string name)
    {
        Id = id;
        Name = name;
    }

    protected override IEnumerable<object> GetIdentityValues()
    {
        yield return Id;
    }
}
