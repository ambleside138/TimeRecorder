using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Configurations.Items
{
    /// <summary>
    /// SQLiteのバックアップ先設定を表します
    /// </summary>
    class BackupPathConfig : ConfigItemBase
    {
        public string DirectoryPath { get; set; }

        public override ConfigKey Key => ConfigKey.BackupPath;
    }
}
