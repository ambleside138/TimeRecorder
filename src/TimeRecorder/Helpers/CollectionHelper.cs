using System;
using System.Collections.Generic;
using System.Text;

namespace TimeRecorder.Helpers;

public static class CollectionHelper
{
    public static void AddRange<T>(this ICollection<T> addTo, IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            addTo.Add(item);
        }
    }
}
