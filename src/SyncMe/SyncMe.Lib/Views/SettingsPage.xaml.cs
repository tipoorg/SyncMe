using System.Collections.ObjectModel;
using SyncMe.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

public record LogFile(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsPage : ContentPage
{
    private readonly IPathProvider _pathProvider;
    private readonly IConfigRepository _configRepository;

    public ObservableCollection<LogFile> Logs { get; } = new ObservableCollection<LogFile>();

    public string CurrentBuild => VersionTracking.CurrentBuild;
    public string CurrentVersion => VersionTracking.CurrentVersion;

    public SettingsPage(IPathProvider pathProvider,
                        IConfigRepository configRepository)
    {
        InitializeComponent();
        _pathProvider = pathProvider;
        _configRepository = configRepository;
        SoundToggle.IsToggled = !_configRepository.Get(ConfigKey.IsMute);
        BindingContext = this;

        if (Directory.Exists(_pathProvider.SyncMeLogsFolder))
        {
            foreach (var item in Directory.EnumerateFiles(_pathProvider.SyncMeLogsFolder).Select(Path.GetFileName))
            {
                Logs.Add(new LogFile(item));
            }
        }
    }

    private async void OnLogFileTapped(object sender, EventArgs e)
    {
        if (sender is Label label && Path.Combine(_pathProvider.SyncMeLogsFolder, label.Text) is string logFile && File.Exists(logFile))
            await Launcher.OpenAsync(new OpenFileRequest { File = new(logFile) });
    }

    private void OnLogFileRemoved(object sender, EventArgs e)
    {
        if (sender is SwipeItem { CommandParameter: LogFile logFile } &&
            Path.Combine(_pathProvider.SyncMeLogsFolder, logFile.Name) is string logFilePath &&
            File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
            Logs.Remove(Logs.First(x => x.Name.Equals(Path.GetFileName(logFilePath))));
        }
    }

    private void OnSoundTapped(object sender, EventArgs e)
    {
        if (sender is Switch switchItem)
        {
            _configRepository.Set(ConfigKey.IsMute, !switchItem.IsToggled);
        }
    }
}
