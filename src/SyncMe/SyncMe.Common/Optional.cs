namespace SyncMe.Common;

public record struct Optional<T>(T Value)
{

    public bool HasValue => !EqualityComparer<T>.Default.Equals(Value, default);

    public static implicit operator Optional<T> (T value) => new Optional<T>(value);
}
