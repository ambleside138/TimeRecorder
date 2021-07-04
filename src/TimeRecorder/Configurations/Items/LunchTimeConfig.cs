using System.Text.Json.Serialization;
using TimeRecorder.Domain.Domain.Tracking;

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

        [JsonIgnore]
        public TimePeriod TimePeriod => new TimePeriod(StartHHmm + "00", EndHHmm + "00");
        
    }
}
