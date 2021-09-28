using Google.Cloud.Firestore;

namespace TimeRecorder.Repository.Firebase.Shared
{

    [FirestoreData]
    class DocumentBase
    {
        [FirestoreProperty]
        public Timestamp CreatedAt { get; set; }

        [FirestoreProperty]
        public Timestamp UpdatedAt { get; set; }

        public void SetCreateDateTime()
        {
            CreatedAt = Timestamp.GetCurrentTimestamp();
        }

        public void SetUpdateDateTime()
        {
            UpdatedAt = Timestamp.GetCurrentTimestamp();
        }
    }
}
