using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.Firebase.Shared.Helpers
{
    internal static class DocumentSnapshotHelper
    {
        public static T ConvertToWithId<T>(this DocumentSnapshot documentSnapshot, Action<T, string> idSetterFunc)
        {
            var obj = documentSnapshot.ConvertTo<T>();
            idSetterFunc(obj, documentSnapshot.Id);
            return obj;
        }
    }
}
