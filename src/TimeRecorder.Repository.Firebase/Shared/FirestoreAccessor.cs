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
    public class FirestoreAccessor
    {
        private readonly string _FirebaseProjectId;

        private FirebaseAuthLink _FirebaseAuthLink;

        public FirestoreAccessor(FirebaseAuthLink firebaseAuthLink, string firebaseProjectId)
        {
            _FirebaseAuthLink = firebaseAuthLink;
            _FirebaseProjectId = firebaseProjectId;
        }


        public async Task<FirestoreDb> CreateFirestoreDbAsync()
        {
            //var userProps = auth.User;
            CallCredentials callCredentials = CallCredentials.FromInterceptor(async (context, metadata) =>
            {
                if (_FirebaseAuthLink.IsExpired())
                {
                    _FirebaseAuthLink = await _FirebaseAuthLink.GetFreshAuthAsync();
                }

                if (string.IsNullOrEmpty(_FirebaseAuthLink.FirebaseToken))
                {
                    return;
                }

                metadata.Clear();
                metadata.Add("authorization", $"Bearer {_FirebaseAuthLink.FirebaseToken}");
            });

            ChannelCredentials channelCredentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);


            // Create a custom Firestore Client using custom credentials
            var grpcChannel = new Channel("firestore.googleapis.com", channelCredentials);
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
            Firestore.FirestoreClient grcpClient = new(grpcChannel);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
            FirestoreClientImpl firestoreClient = new(grcpClient, FirestoreSettings.GetDefault());

            return await FirestoreDb.CreateAsync(_FirebaseProjectId, firestoreClient);
        }


        // Sample Source

        //public static void Access()
        //{
        //    FirestoreDb db = CreateFirestoreDbWithGoogleAuthentication().Result;


        //    CollectionReference usersRef = db.Collection("users");
        //    QuerySnapshot snapshot = usersRef.GetSnapshotAsync().Result;
        //    foreach (DocumentSnapshot document in snapshot.Documents)
        //    {
        //        Console.WriteLine("User: {0}", document.Id);
        //        Dictionary<string, object> documentDictionary = document.ToDictionary();
        //        Console.WriteLine("First: {0}", documentDictionary["First"]);
        //        if (documentDictionary.ContainsKey("Middle"))
        //        {
        //            Console.WriteLine("Middle: {0}", documentDictionary["Middle"]);
        //        }
        //        Console.WriteLine("Last: {0}", documentDictionary["Last"]);
        //        Console.WriteLine("Born: {0}", documentDictionary["Born"]);
        //        Console.WriteLine();
        //    }
        //}
    }
}
