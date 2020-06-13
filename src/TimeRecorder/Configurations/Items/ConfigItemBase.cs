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
        FavoriteWorkTask
    };

    abstract class ConfigItemBase
    {
        [JsonIgnore]
        public abstract ConfigKey Key { get; }
    }
}
