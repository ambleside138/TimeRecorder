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
using TimeRecorder.Repository.Firebase.Shared;

namespace TimeRecorder.Repository.Firebase
{
    public class FirestoreAccessor
    {
        private readonly string _FirebaseProjectId;

        private FirebaseAuthLink _FirebaseAuthLink;


        public static async Task<FirestoreDb> CreateDbClientAsync()
        {
            FirebaseAuthLink firebaseAuthLink = FirebaseAuthenticator.Current
                                                                     .SignInWithGoogleOAuthAsyncCached();

            string projectId = FirebaseCredentialConfigLoader.Value.ProjectId;

            return await new FirestoreAccessor(firebaseAuthLink, projectId).CreateFirestoreDbAsync();
        }


        public FirestoreAccessor(FirebaseAuthLink firebaseAuthLink, string firebaseProjectId)
        {
            _FirebaseAuthLink = firebaseAuthLink;
            _FirebaseProjectId = firebaseProjectId;
        }


        public async Task<FirestoreDb> CreateFirestoreDbAsync()
        {
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
            ChannelBase grpcChannel = new Channel("firestore.googleapis.com", channelCredentials);
            Firestore.FirestoreClient grcpClient = new(grpcChannel);
            FirestoreClientImpl firestoreClient = new(grcpClient, FirestoreSettings.GetDefault());

            return await FirestoreDb.CreateAsync(_FirebaseProjectId, firestoreClient);
        }

    }
}
