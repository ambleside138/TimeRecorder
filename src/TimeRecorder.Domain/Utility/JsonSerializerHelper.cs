using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TimeRecorder.Domain.Utility;

public static class JsonSerializerHelper
{
    public static JsonSerializerOptions DefaultOptions { get; } = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // デフォルトでは日本語がエンコードされない
        WriteIndented = true,
        AllowTrailingCommas = true, // 末尾のカンマを許可する
    };
}
