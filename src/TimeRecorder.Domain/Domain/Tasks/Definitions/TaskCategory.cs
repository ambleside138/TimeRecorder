using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tasks.Definitions
{
    public enum TaskCategory
    {
        [DisplayText("未指定")]
        UnKnown,

        [DisplayText("開発")]
        Develop,

        [DisplayText("研究開発")]
        ResearchAndDevelopment,

        [DisplayText("導入支援")]
        Introduce,

        [DisplayText("保守")]
        Maintain,

        [DisplayText("その他")]
        Other,
    }
}
