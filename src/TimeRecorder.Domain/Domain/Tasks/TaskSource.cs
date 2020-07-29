using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Domain.Domain.Tasks
{
    /// <summary>
    /// タスクの生成元種別を表します
    /// </summary>
    public enum TaskSource
    {
        Normal,

        Favorite,
        
        Schedule,
    }

    public static class TaskSourceMethods
    {
        public static bool IsTemporary(this TaskSource taskSource)
        {
            return taskSource != TaskSource.Normal;
        }
    }
}
