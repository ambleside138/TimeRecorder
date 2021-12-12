using Google.Cloud.Firestore;
using TimeRecorder.Repository.Firebase.Shared;

namespace TimeRecorder.Repository.Firebase.System.Dao
{
    [FirestoreData]

    class UserDocument : DocumentBase
    {
        [FirestoreProperty]
        public string UserId { get; set; }

        [FirestoreProperty]
        public string DisplayName { get; set; }

        [FirestoreProperty]
        public string Email { get; set; }

        [FirestoreProperty]
        public string PhotoUrl { get; set; }

        [FirestoreProperty]
        public Timestamp LatestSignInDateTime { get; set; }


    }
}
