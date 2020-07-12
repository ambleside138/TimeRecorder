using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TimeRecorder.Configurations.Items
{
    enum ConfigKey
    {
        Theme,
        BackupPath,
        FavoriteWorkTask,
        ScheduleTitleMap,
        WorkingHourImportApiUrl,
        ImportParam,
    };

    abstract class ConfigItemBase
    {
        [JsonIgnore]
        internal abstract ConfigKey Key { get; }
    }
}
