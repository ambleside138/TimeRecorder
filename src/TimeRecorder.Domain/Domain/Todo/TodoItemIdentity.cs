using TimeRecorder.Domain.Domain.Shared;

namespace TimeRecorder.Domain.Domain.Todo;

public record TodoItemIdentity : IdentityBase
{
    public static readonly TodoItemIdentity DoneFilter = new("filter_done");

    public TodoItemIdentity(string id) : base(id) { }

    public TodoItemIdentity() : base("")
    {

    }
}
