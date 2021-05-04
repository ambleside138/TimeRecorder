using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Configurations.Items
{
    /// <summary>
    /// 昼休憩時間設定を表します
    /// </summary>
    class LunchTimeConfig : ConfigItemBase
    {
        public string StartHHmm { get; set; }

        public string EndHHmm { get; set; }

        internal override ConfigKey Key => ConfigKey.LunchTime;

        
    }
}
