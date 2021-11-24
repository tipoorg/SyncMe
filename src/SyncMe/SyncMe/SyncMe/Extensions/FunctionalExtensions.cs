namespace SyncMe.Extensions;

public static class FunctionalExtensions
{
    public static void FeedTo<T1>(this T1 input, Action<T1> action) => action(input);
    public static T2 FeedTo<T1, T2>(this T1 input, Func<T1, T2> func) => func(input);
}
