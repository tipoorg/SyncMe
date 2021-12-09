using System;
using System.IO;

namespace SyncMe.IOS.Utilities;

internal sealed class IOSPathProvider : IPathProvider
{
    public string SyncMeFolder { get; }
    public string SyncMeLogsFolder { get; }
    public string SyncMeDbPath { get; }

    public IOSPathProvider()
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

