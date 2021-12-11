using LanguageExt;
using System.Reactive.Linq;

namespace SyncMe.Functional;

public static class FunctionalExtensions
{
    public static void FeedTo<T1>(this T1 input, Action<T1> action) => action(input);
    public static T2 FeedTo<T1, T2>(this T1 input, Func<T1, T2> func) => func(input);

    public static IObservable<T> Filter<T>(this IObservable<Option<T>> that, Func<T, bool> func)
    {
        return that
            .Where(x => x.Filter(func).IsSome)
            .Select(x => x.Match(x => x, () => throw new NotImplementedException()));
    }
}
