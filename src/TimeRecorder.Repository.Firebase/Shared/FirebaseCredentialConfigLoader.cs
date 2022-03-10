using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.Firebase.Shared;

class FirebaseCredentialConfigLoader
{
    private static readonly Lazy<FirebaseCredentialConfig> _FirebaseTokenInitialized = new(() => GetFirebaseToken());

    public static FirebaseCredentialConfig Value => _FirebaseTokenInitialized.Value;


    private static FirebaseCredentialConfig GetFirebaseToken()
    {
        const string filename = "firebase-credentialconfig.json";
        return JsonFileIO.Deserialize<FirebaseCredentialConfig>(filename);
    }
}
