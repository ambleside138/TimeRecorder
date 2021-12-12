using Firebase.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TimeRecorder.Repository.Firebase.System;

namespace TimeRecorder.Repository.Firebase.Shared
{
    /// <summary>
    /// Firebase.Authenticationサービスを利用してログインを行う処理を提供します
    /// </summary>
    internal class FirebaseAuthenticator
    {
        private static readonly string _CredentialFileName = "credentials_firebase.json";

        private FirebaseAuthLink _firebaseAuthLink = null;

        public static FirebaseAuthenticator Current { get; } = new();

        public string UserId => _firebaseAuthLink?.User.LocalId;

        private FirebaseAuthenticator()
        {

        }

        public bool ExistsLoginCache() => LoginCacheHelper.Exists();

        public FirebaseAuthLink SignInWithGoogleOAuthAsyncCached()
        {
            if (_firebaseAuthLink != null)
            {
                return _firebaseAuthLink;
            }
            else
            {
                return  SignInWithGoogleOAuthAsync();
            }
        }

        public void SignOut()
        {
            if (_firebaseAuthLink == null)
                return;

            FirebaseAuthProvider authProvider = new(new FirebaseConfig(FirebaseCredentialConfigLoader.Value.ApiKey));
            authProvider.UnlinkAccountsAsync(_firebaseAuthLink.FirebaseToken, FirebaseAuthType.Google);
            _firebaseAuthLink = null;

            UserCredential googleCredential = GetGoogleOAuthUserCredentialAsync();
            if (googleCredential != null)
            {
                _ = googleCredential.RevokeTokenAsync(new CancellationToken());
            }

            LoginCacheHelper.Clear();
        }



        private UserCredential GetGoogleOAuthUserCredentialAsync()
        {
            // WebBrowser経由でgoogleログイン
            var currentPath = Directory.GetParent(Process.GetCurrentProcess().MainModule.FileName);
            var credentialFullPath = Path.Combine(currentPath.FullName, _CredentialFileName);

            using FileStream stream = new(credentialFullPath, FileMode.Open, FileAccess.Read);

            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the f0irst time.

            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                                                        GoogleClientSecrets.Load(stream).Secrets,
                                                        new[] { "email", "profile" },
                                                        "user",
                                                        CancellationToken.None,
                                                        null,
                                                        null).Result;
        }

        /// <summary>
        /// Googleでサインインし、OAuthでFirebaseに認証情報を渡します
        /// </summary>
        /// <returns></returns>
        public FirebaseAuthLink SignInWithGoogleOAuthAsync()
        {
            if (_firebaseAuthLink != null)
                return _firebaseAuthLink;

            UserCredential result = GetGoogleOAuthUserCredentialAsync();


            if (result.Token.IsExpired(SystemClock.Default))
            {
                _ = result.RefreshTokenAsync(CancellationToken.None).Result;
            }

            // Firebase.AuthenticatioサービスにOAuthトークンを渡す
            // Create a custom authentication mechanism for Email/Password authentication
            // If the authentication is successful, we will get back the current authentication token and the refresh token
            // The authentication expires every hour, so we need to use the obtained refresh token to obtain a new authentication token as the previous one expires
            FirebaseAuthProvider authProvider = new(new FirebaseConfig(FirebaseCredentialConfigLoader.Value.ApiKey));
            
            // なぜかawaitするとフリーズする...
            _firebaseAuthLink = authProvider.SignInWithGoogleIdTokenAsync(result.Token.IdToken).Result;


            LoginCacheHelper.Cache(_firebaseAuthLink.User.Email);
            return _firebaseAuthLink;
        }

    }
}
