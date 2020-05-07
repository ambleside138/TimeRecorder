using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Tasks.Definitions
{
    public enum Product
    {
        [DisplayText("[ なし ]")]
        None,

        [DisplayText("Product1")]
        Product1,

        [DisplayText("Product2")]
        Product2,

        [DisplayText("Product3")]
        Product3,

    }
}
