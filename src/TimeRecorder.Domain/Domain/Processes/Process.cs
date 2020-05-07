using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Processes
{
    /// <summary>
    /// 工程
    /// </summary>
    public class Process
    {
        public Identity<Process> Id { get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Domain層内のみで、titleのみでの生成を許可する
        /// </summary>
        /// <param name="title"></param>
        internal Process(string title)
            : this(Identity<Process>.Temporary, title) { }


        public Process(Identity<Process> identity, string title)
        {
            Id = identity;
            Title = title;
        }
    }
}
