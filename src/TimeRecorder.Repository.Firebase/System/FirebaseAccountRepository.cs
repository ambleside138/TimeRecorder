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

namespace TimeRecorder.Repository.Firebase.System
{
    public class FirebaseAccountRepository : IAccountRepository
    {
        public LoginStatus GetLoginStatus()
        {
            FirebaseAuthLink link = FirebaseAuthenticator.Current.SignInWithGoogleOAuthAsyncCached().Result;


            return new LoginStatus
            {
                DisplayName = link.User.DisplayName,
                Email = link.User.Email,
                PhotoUrl = link.User.PhotoUrl,
                UserId = link.User.LocalId,
            };
        }

    }
}
