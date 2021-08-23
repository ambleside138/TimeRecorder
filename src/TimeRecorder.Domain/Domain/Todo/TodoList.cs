﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Todo
{
    public class TodoList : Entity<TodoList>
    {
        public TodoListIdentity Id { get; private set; }



        #region IconCharacter変更通知プロパティ
        private string _IconCharacter;

        public string IconCharacter
        {
            get => _IconCharacter;
            set => RaisePropertyChangedIfSet(ref _IconCharacter, value);
        }
        #endregion


        #region IconKey変更通知プロパティ
        private string _IconKey;

        public string IconKey
        {
            get => _IconKey;
            set => RaisePropertyChangedIfSet(ref _IconKey, value);
        }
        #endregion


        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get => _Title;
            set => RaisePropertyChangedIfSet(ref _Title, value);
        }
        #endregion


        #region Background変更通知プロパティ
        private string _Background;

        public string Background
        {
            get => _Background;
            set => RaisePropertyChangedIfSet(ref _Background, value);
        }
        #endregion


        public TodoList(TodoListIdentity id)
        {
            Id = id;
        }


        protected override IEnumerable<object> GetIdentityValues()
        {
            yield return Id;
        }
    }
}
