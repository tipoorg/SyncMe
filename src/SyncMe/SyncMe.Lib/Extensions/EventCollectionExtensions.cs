using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.Lib.Extensions;

public static class EventCollectionExtensions
{
    public static void AddSafe(this EventCollection events, EventCollection second)
    {
        foreach (var e in second)
            if (!events.ContainsKey(e.Key))
                events.Add(e.Key, e.Value);
    }

    public static EventCollection ToEventCollection<T>(this IEnumerable<T> events, Func<T, DateTime> keySelector, Func<T, object> elementSelector)
    {
        return events.GroupBy(keySelector, elementSelector).ToEventCollection();
    }

    public static EventCollection ToEventCollection<T>(this IEnumerable<IGrouping<DateTime, T>> events)
    {
        var result = new EventCollection();
        foreach (var e in events)
            result.Add(e.Key, e.ToList());

        return result;
    }
}
