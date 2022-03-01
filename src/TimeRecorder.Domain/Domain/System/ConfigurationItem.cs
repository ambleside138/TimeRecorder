using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.System;

/// <summary>
/// 設定内容を表します
/// </summary>
public class ConfigurationItem
{
    public string Key { get; set; }

    public string JsonString { get; private set; }

    public ConfigurationItem(string key, string jsonString)
    {
        Key = key;
        JsonString = jsonString;
    }

    public ConfigurationItem(string key)
    {
        Key = key;
    }

    public T GetConfigValue<T>()
    {
        return JsonSerializer.Deserialize<T>(JsonString, JsonSerializerHelper.DefaultOptions);
    }

    public void SetConfigValue<T>(T source)
    {
        JsonString = JsonSerializer.Serialize(source, JsonSerializerHelper.DefaultOptions);
    }

}
