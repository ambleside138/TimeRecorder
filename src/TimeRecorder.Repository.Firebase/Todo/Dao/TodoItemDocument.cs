﻿using System;
using Google.Cloud.Firestore;
using TimeRecorder.Domain.Domain.Todo;
using TimeRecorder.Repository.Firebase.Shared;
using TimeRecorder.Repository.Firebase.Shared.Helpers;

namespace TimeRecorder.Repository.Firebase.Todo.Dao
{
    [FirestoreData]
    class TodoItemDocument : DocumentBase
    {
        public string Id { get; set; }

        [FirestoreProperty]
        public string Title { get; set; }

        [FirestoreProperty]
        public bool IsImportant { get; set; }

        [FirestoreProperty]
        public Timestamp? CompletedDateTime { get; set; }

        [FirestoreProperty]
        public string Memo { get; set; }

        [FirestoreProperty]
        public string TodoListId { get; set; }

        public static TodoItemDocument FromDomainObject(TodoItem todoItem)
        {
            return new TodoItemDocument
            {
                Title = todoItem.Title,
                IsImportant = todoItem.IsImportant,
                CompletedDateTime = todoItem.CompletedDateTime.ToTimestamp(),
                Memo = todoItem.Memo,
                TodoListId = todoItem.TodoListId.Value,
            };
        }

        public TodoItem ConvertToDomainObject()
        {
            return TodoItem.FromRepository(
                             Id,
                             Title,
                             IsImportant,
                             CompletedDateTime?.ToDateTime(),
                             Memo,
                             TodoListId);
        }
    }
}