using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.Firebase.System;

internal class LoginCache
{

    public string Email { get; set; }
}

internal static class LoginCacheHelper
{
    private const string _FileName = "login.cache";

    public static void Cache(string email)
    {
        JsonFileIO.Serialize(new LoginCache { Email = email }, _FileName);
    }

    public static void Clear()
    {
        if (File.Exists(_FileName))
        {
            File.Delete(_FileName);
        }
    }

    public static bool Exists() => File.Exists(_FileName);
}
