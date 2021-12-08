namespace SyncMe;

public interface IPathProvider
{
    string SyncMeFolder { get; }
    string SyncMeLogsFolder { get; }
    string SyncMeDbPath { get; }
}
