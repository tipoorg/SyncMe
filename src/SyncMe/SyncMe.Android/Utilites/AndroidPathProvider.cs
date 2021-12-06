using System.IO;

namespace SyncMe.Droid.Utilites;

internal sealed class AndroidPathProvider : IPathProvider
{
    public string SyncMeFolder { get; }
    public string SyncMeLogsFolder { get; }
    public string SyncMeDbPath { get; }

    public AndroidPathProvider()
    {
        SyncMeFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        SyncMeLogsFolder = Path.Combine(SyncMeFolder, "logs");
        SyncMeDbPath = Path.Combine(SyncMeFolder, "syncme.db");

        if (!File.Exists(SyncMeDbPath))
        {
            File.Create(SyncMeDbPath).Dispose();
        }
    }
}
