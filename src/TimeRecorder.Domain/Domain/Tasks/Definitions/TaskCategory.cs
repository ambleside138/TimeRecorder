using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TimeRecorder.Domain.Utility;
using TimeRecorder.Domain.Utility.Attributes;

namespace TimeRecorder.Domain.Domain.Tasks.Definitions
{
    public enum TaskCategory
    {
        [DisplayText("未指定")]
        [IconKey("HelpCircle")]
        UnKnown,

        [DisplayText("開発")]
        [IconKey("FileCodeOutline")]
        Develop,

        [DisplayText("研究開発")]
        [IconKey("FlaskEmpty")]
        ResearchAndDevelopment,

        [DisplayText("導入支援")]
        [IconKey("AccountGroup")]
        Introduce,

        [DisplayText("保守")]
        [IconKey("Wrench")]
        Maintain,

        [DisplayText("その他")]
        [IconKey("FileTable")]
        Other,
    }
}
