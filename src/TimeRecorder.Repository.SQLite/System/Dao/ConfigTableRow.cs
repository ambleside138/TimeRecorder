using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.System;

namespace TimeRecorder.Repository.SQLite.System.Dao
{
    class ConfigTableRow
    {
        public string ConfigKey { get; set; }

        public string JsonValue { get; set; }

        public ConfigurationItem ConvertToDomainObject()
        {
            return new ConfigurationItem(ConfigKey, JsonValue);
        }

        public static ConfigTableRow FromDomainObject(ConfigurationItem configurationItem)
        {
            return new ConfigTableRow
            {
                ConfigKey = configurationItem.Key,
                JsonValue = configurationItem.JsonString,
            };
        }
    }
}
