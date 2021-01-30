using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Helpers
{
    /// <summary>
    /// JSONファイルを入出力するメソッドを提供します
    /// </summary>
    static class JsonFileIO
    {
        private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();


        private static readonly JsonSerializerOptions _Options = JsonSerializerHelper.DefaultOptions;

        /// <summary>
        /// 指定パスのファイルに格納されているJSONドキュメントを逆シリアル化します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 指定した source をシリアル化し、生成された JSON ドキュメントをファイルに書き込みます。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        public static void Serialize<T>(T source, string filePath)
        {
            const int RetryMaxCount = 3;
            var tryCount = 0;

            while(true)
            {
                tryCount++;

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        var jsonString = JsonSerializer.SerializeToUtf8Bytes(source, _Options);
                        stream.Write(jsonString);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if(tryCount < RetryMaxCount)
                    {
                        _Logger.Warn(ex, "ファイルの出力に失敗しました。1秒後にリトライします path=" + filePath);
                        Task.Delay(1000);
                    }
                    else
                    {
                        _Logger.Error(ex, "リトライ回数の上限に達したため処理を中止します");
                        break;
                    }
                }
            }

        }

    }
}
