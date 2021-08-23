using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using System.Diagnostics;
using System.Threading;


namespace TimeRecorder.Repository.Firebase
{
    public class Accessor
    {
        private static Lazy<FirebaseToken> _FirebaseTokenInitialized = new(() => GetFirebaseToken());

        private static FirebaseToken FirebaseToken => _FirebaseTokenInitialized.Value;
        
        private static readonly string _CredentialFileName = "credentials_firebase.json";



        public static async Task<FirestoreDb> CreateFirestoreDbWithGoogleAuthentication()
        {
            var token = "";

            // WebBrowser経由でgoogleログイン
            {
                var currentPath = Directory.GetParent(Process.GetCurrentProcess().MainModule.FileName);
                var credentialFullPath = Path.Combine(currentPath.FullName, _CredentialFileName);


                using (var stream = new FileStream(credentialFullPath, FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.

                    var result = GoogleWebAuthorizationBroker.AuthorizeAsync(
    GoogleClientSecrets.Load(stream).Secrets,
    new[] { "email", "profile" },
    "user",
    CancellationToken.None,
    null,
    null).Result;

                    if (result.Token.IsExpired(SystemClock.Default))
                    {
                        await result.RefreshTokenAsync(CancellationToken.None);
                    }

                    token = result.Token.IdToken;

                }

            }

            // Firebaseにトークンを渡す
            ChannelCredentials channelCredentials = null;
            {
                // Create a custom authentication mechanism for Email/Password authentication
                // If the authentication is successful, we will get back the current authentication token and the refresh token
                // The authentication expires every hour, so we need to use the obtained refresh token to obtain a new authentication token as the previous one expires
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseToken.ApiKey));
                var auth = authProvider.SignInWithGoogleIdTokenAsync(token).Result;
                var callCredentials = CallCredentials.FromInterceptor(async (context, metadata) =>
                {
                    if (auth.IsExpired()) auth = await auth.GetFreshAuthAsync();
                    if (string.IsNullOrEmpty(auth.FirebaseToken)) return;

                    metadata.Clear();
                    metadata.Add("authorization", $"Bearer {auth.FirebaseToken}");
                });
                channelCredentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);
            }



            // Create a custom Firestore Client using custom credentials
            var grpcChannel = new Channel("firestore.googleapis.com", channelCredentials);
            var grcpClient = new Firestore.FirestoreClient(grpcChannel);
            var firestoreClient = new FirestoreClientImpl(grcpClient, FirestoreSettings.GetDefault());

            return FirestoreDb.Create(FirebaseToken.ProjectId, firestoreClient);
        }

        public static void Access()
        {
            FirestoreDb db = CreateFirestoreDbWithGoogleAuthentication().Result;


            CollectionReference usersRef = db.Collection("users");
            QuerySnapshot snapshot = usersRef.GetSnapshotAsync().Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Console.WriteLine("User: {0}", document.Id);
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                Console.WriteLine("First: {0}", documentDictionary["First"]);
                if (documentDictionary.ContainsKey("Middle"))
                {
                    Console.WriteLine("Middle: {0}", documentDictionary["Middle"]);
                }
                Console.WriteLine("Last: {0}", documentDictionary["Last"]);
                Console.WriteLine("Born: {0}", documentDictionary["Born"]);
                Console.WriteLine();
            }
        }


        internal static FirebaseToken GetFirebaseToken()
        {
            const string filename = "firebase-tokenconfig.json";
            return JsonFileIO.Deserialize<FirebaseToken>(filename);
        }
    }
}
