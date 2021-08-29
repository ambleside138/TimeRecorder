using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Repository.Firebase.Shared.Helpers
{
    internal static class TimestampHelper
    {
        public static Timestamp ToTimestamp(this DateTime value)
        {
            return Timestamp.FromDateTime(value);
        }

        public static Timestamp? ToTimestamp(this DateTime? value)
        {
            return value.HasValue ? Timestamp.FromDateTime(value.Value) : null;
        }
    }
}
