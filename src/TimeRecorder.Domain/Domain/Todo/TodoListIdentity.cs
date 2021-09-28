using TimeRecorder.Domain.Domain.Shared;

namespace TimeRecorder.Domain.Domain.Todo
{
    public record TodoListIdentity : IdentityBase
    {
        public static TodoListIdentity Today => new("define_today");

        public static TodoListIdentity Important => new("define_important");

        public static TodoListIdentity Future => new("define_future");

        public static TodoListIdentity None => new("define_none");

        public static TodoListIdentity Divider => new("define_divider");

        public bool IsFixed => Value.StartsWith("define_");

        public TodoListIdentity(string id) : base(id)
        {

        }

        public TodoListIdentity() : base("")
        {

        }
    }
}
