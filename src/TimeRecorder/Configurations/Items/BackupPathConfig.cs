using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TimeRecorder.Configurations.Items
{
    /// <summary>
    /// SQLiteのバックアップ先設定を表します
    /// </summary>
    class BackupPathConfig : ConfigItemBase
    {
        public string DirectoryPath { get; set; }
        internal override ConfigKey Key => ConfigKey.BackupPath;
    }
}
