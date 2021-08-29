using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;

namespace TimeRecorder.Repository.Firebase.Todo.Dao
{
    class TodoDao
    {
        private readonly FirestoreDb _FirestoreDb;
        private readonly string _UserId;
        private readonly string CollectionName = "todos";

        private readonly string SubCollectionName = "todoItems";


        public TodoDao(FirestoreDb firestoreDb, string userId)
        {
            _FirestoreDb = firestoreDb;
            _UserId = userId;
        }

        public async Task<TodoItemIdentity> SetAsync(TodoItemIdentity docId, TodoItemDocument todoDocument)
        {
            var subCollectionRef = GetRootDocumentReference().Collection(SubCollectionName);

            if(docId.IsEmpty)
            {
                // get auto-generated-id
                DocumentReference documentReference = subCollectionRef.Document();
                docId = new TodoItemIdentity(documentReference.Id);
            }

            await subCollectionRef.Document(docId.Value).SetAsync(todoDocument);

            return docId;
        }



        private DocumentReference GetRootDocumentReference()
        {
            return _FirestoreDb.Document($"{CollectionName}/{_UserId}");
        }


        /// <summary>
        /// 指定したUserIdのTodoを取得します
        /// </summary>
        /// <returns></returns>
        public async Task<TodoRootDocument> SelectAsync()
        {
            var docRef = await GetRootDocumentReference().GetSnapshotAsync();

            if(docRef.Exists == false)
            {
                return null;
            }

            // さらにサブコレクションを取得する
            var todoCollection = await docRef.Reference.Collection(SubCollectionName).GetSnapshotAsync();

            var list = todoCollection.Documents
                                     .Select(d => d.ConvertTo<TodoItemDocument>())
                                     .ToList();


            var rootDoc = docRef.ConvertTo<TodoRootDocument>();
            rootDoc.TodoItems = list.ToArray();

            return rootDoc;
        }
    }
}
