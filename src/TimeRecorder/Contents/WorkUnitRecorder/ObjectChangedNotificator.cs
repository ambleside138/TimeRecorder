using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Contents.WorkUnitRecorder
{
    // もうちょっといい名前をつけたい
    class ObjectChangedNotificator
    {
        public event Action WorkTaskEdited;

        public static ObjectChangedNotificator Instance { get; } = new ObjectChangedNotificator();

        private ObjectChangedNotificator()
        {

        }

        public void NotifyWorkTaskEdited()
        {
            WorkTaskEdited?.Invoke();
        }
    }
}
