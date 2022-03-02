using System;

namespace Google.Cloud.Firestore;

internal static class TimestampHelper
{
    public static Timestamp ToTimestamp(this DateTime value)
    {
        DateTime utc = TimeZoneInfo.ConvertTimeToUtc(value);
        return Timestamp.FromDateTime(utc);
    }

    public static Timestamp? ToTimestamp(this DateTime? value)
    {
        return value.HasValue ? value.Value.ToTimestamp() : null;
    }

    public static DateTime ToLocalDateTime(this Timestamp timestamp)
    {
        var utc = timestamp.ToDateTime();
        return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZoneInfo.Local);
    }
}
