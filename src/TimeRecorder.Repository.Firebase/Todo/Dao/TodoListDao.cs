﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Repository.Firebase.Shared.Helpers;

namespace TimeRecorder.Repository.Firebase.Todo.Dao;

internal class TodoListDao
{
    private readonly FirestoreDb _FirestoreDb;
    private readonly string _UserId;
    private readonly string CollectionName = "todos";

    private readonly string SubCollectionName = "todoLists";

    public TodoListDao(FirestoreDb firestoreDb, string userId)
    {
        _FirestoreDb = firestoreDb;
        _UserId = userId;
    }

    public async Task<TodoListIdentity> SetAsync(TodoListIdentity docId, TodoListDocument todoDocument)
    {
        DocumentReference rootDocRef = GetRootDocumentReference();

        // 初回の場合はUIdを設定する
        var doc = await rootDocRef.GetSnapshotAsync();
        if (doc.Exists == false)
        {
            _ = await rootDocRef.SetAsync(new TodoRootDocument { UserId = _UserId, CreatedAt = Timestamp.GetCurrentTimestamp() });
        }

        var subCollectionRef = rootDocRef.Collection(SubCollectionName);


        if (docId.IsEmpty)
        {
            // get auto-generated-id
            DocumentReference documentReference = subCollectionRef.Document();
            docId = new TodoListIdentity(documentReference.Id);
            todoDocument.SetCreateDateTime();
        }

        todoDocument.SetUpdateDateTime();

        _ = await subCollectionRef.Document(docId.Value).SetAsync(todoDocument);
        return docId;
    }


    public async Task DeleteAsync(TodoListIdentity docId) => await GetCollectionReference().Document(docId.Value).DeleteAsync();


    private DocumentReference GetRootDocumentReference() => _FirestoreDb.Collection(CollectionName).Document(_UserId);

    private CollectionReference GetCollectionReference() => _FirestoreDb.Collection($"{CollectionName}/{_UserId}/{SubCollectionName}");

    /// <summary>
    /// 指定したUserIdのTodoを取得します
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TodoListDocument>> SelectAsync()
    {
        var docRef = await GetRootDocumentReference().GetSnapshotAsync();

        if (docRef.Exists == false)
        {
            return null;
        }

        // さらにサブコレクションを取得する
        var todoCollection = await docRef.Reference.Collection(SubCollectionName).GetSnapshotAsync();

        return todoCollection.Documents
                                 .Select(d => d.ConvertToWithId<TodoListDocument>((obj, id) => obj.Id = id))
                                 .ToList();
    }
}
