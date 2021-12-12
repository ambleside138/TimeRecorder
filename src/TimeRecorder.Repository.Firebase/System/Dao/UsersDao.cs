using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.Firebase.System.Dao
{
    class UsersDao
    {
        public const string CollectionName = "users";


        public static CollectionReference GetUsersReference(FirestoreDb firestoreDb)
        {
            return firestoreDb.Collection(CollectionName);
        }
    }
}
