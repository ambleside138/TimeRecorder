using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Repository.Firebase.Shared;
using TimeRecorder.Repository.Firebase.System.Dao;

namespace TimeRecorder.Repository.Firebase.System
{
    public class FirebaseAccountRepository : IAccountRepository
    {
        public LoginStatus GetLoginStatus()
        {
            FirebaseAuthLink link = FirebaseAuthenticator.Current
                                                         .SignInWithGoogleOAuthAsyncCached()
                                                         .Result;

            LoginStatus status = new()
            {
                DisplayName = link.User.DisplayName,
                Email = link.User.Email,
                PhotoUrl = link.User.PhotoUrl,
                UserId = link.User.LocalId,
            };

            var db = FirestoreAccessor.CreateDbClientAsync().Result;

            DocumentReference docRef = UsersDao.GetUsersReference(db).Document(status.UserId);


            DocumentSnapshot snapshot = docRef.GetSnapshotAsync().Result;

            var timestamp = Timestamp.GetCurrentTimestamp();

            if (snapshot.Exists)
            {
                Dictionary<string, object> updates = new()
                {
                    { nameof(UserDocument.LatestSignInDateTime), timestamp }
                };

                docRef.UpdateAsync(updates).Wait();
            }
            else
            {
                // 新規登録
                UserDocument doc = new()
                {
                    UserId = status.UserId,
                    DisplayName = status.DisplayName,
                    Email = status.Email,
                    PhotoUrl = status.PhotoUrl,
                    LatestSignInDateTime = timestamp,
                    CreatedAt = timestamp,
                    UpdatedAt = timestamp,
                };

                docRef.SetAsync(doc).Wait();
            }

            return status;
        }

    }
}
