using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Repository.Firebase.Todo.Dao
{
    // Idは別で扱う必要がある

    [FirestoreData]
    class TodoDocument
    {
        [FirestoreProperty]
        public string Title { get; set; }

        [FirestoreProperty] 
        public bool IsImportant { get; set; }

        [FirestoreProperty]
        public DateTime? CompletedDateTime { get; set; }

        [FirestoreProperty]
        public string Memo { get; set; }

        public static TodoDocument FromDomainObject(TodoItem todoItem)
        {
            return new TodoDocument
            {
                Title = todoItem.Title,
                IsImportant = todoItem.IsImportant,
                CompletedDateTime = todoItem.CompletedDateTime,
                Memo = todoItem.Memo,
            };
        }

        public TodoItem ConvertToDomainObject(string key)
        {
            return null;
        }
    }
}
