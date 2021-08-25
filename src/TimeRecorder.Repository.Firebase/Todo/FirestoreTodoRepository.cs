using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Repository.Firebase.Todo
{
    class FirestoreTodoRepository : ITodoRepository
    {
        public TodoItemIdentity Add(TodoItem item)
        {
            throw new NotImplementedException();
        }

        public void Delete(TodoItemIdentity id)
        {
            throw new NotImplementedException();
        }

        public void Edit(TodoItem item)
        {
            throw new NotImplementedException();
        }

        public TodoItem[] Select()
        {
            throw new NotImplementedException();
        }

        public TodoItem[] SelectByListId(TodoListIdentity id)
        {
            throw new NotImplementedException();
        }

        public TodoItem[] SelectWithCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
