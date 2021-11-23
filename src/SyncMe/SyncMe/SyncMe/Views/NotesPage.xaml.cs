using System.IO;
using System.Reactive.Linq;
using CalendarProviders.Authorization;
using SyncMe.Providers.OutlookProvider;
using SyncMe.Extensions;
using SyncMe.Models;
using SyncMe.Repos;

namespace SyncMe.Views;

public partial class NotesPage : ContentPage
{
    private readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");
    private readonly ISyncEventsRepository _syncEventsRepository;

    public NotesPage(ISyncEventsRepository syncEventsRepository)
    {
        _syncEventsRepository = syncEventsRepository;

        InitializeComponent();

        // Read the file.
        if (File.Exists(_fileName))
        {
            editor.Text = File.ReadAllText(_fileName);
        }

        ScheduledEvents = Observable
            .FromEventPattern(SetAlarmButton, nameof(Button.Clicked))
            .SelectMany(x => _syncEventsRepository.GetAllSyncEvents())
            .Select(x => x.Activate());
    }

    public IObservable<SyncEvent> ScheduledEvents { get; }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Save the file.

        var manager = new MicrosoftAuthorizationManager();
        await manager.SignInAsync(App.AuthUIParent);
        var client = await manager.GetGraphClientAsync();
        var events = await new OutlookProvider(client, manager.CurrentAccounts.First().Username).GetEventsAsync();

        File.WriteAllText(_fileName, editor.Text);
    }

    void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Delete the file.
        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }
        editor.Text = string.Empty;
    }
}
