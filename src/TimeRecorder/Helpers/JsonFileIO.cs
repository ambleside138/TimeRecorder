using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TimeRecorder.Helpers
{
    static class JsonFileIO
    {
        private static readonly JsonSerializerOptions _Options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), // デフォルトでは日本語がエンコードされない
            WriteIndented = true
        };

        public static T Deserialize<T>(string filePath)
        {
            if(File.Exists(filePath) == false)
                return default;

            using (var stream = new StreamReader(filePath))
            {
                var text = stream.ReadToEnd();
                return JsonSerializer.Deserialize<T>(text, _Options);
            }
        }

        public static void Serialize<T>(T source, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var jsonString = JsonSerializer.SerializeToUtf8Bytes(source, _Options);
                stream.Write(jsonString);
            }
        }

    }
}
