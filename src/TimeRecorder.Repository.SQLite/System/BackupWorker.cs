using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TimeRecorder.Repository.SQLite.System
{
    public class BackupWorker
    {
        public void Backup(string directory)
        {
            var ownFilePath = ConnectionFactory.DbFileName;
            var copyTo = Path.Combine(directory, ConnectionFactory.DbFileName + ".backup");

            File.Copy(ownFilePath, copyTo, true);
        }
    }
}
