using CalendarProviders.Authorization;
using SyncMe.Models;
using SyncMe.Providers.OutlookProvider;
using SyncMe.Repos;
using System.Threading;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;
public record Identity(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentity;
    private readonly IDisposable _addEventConnection;
    private readonly ISyncEventsRepository _syncEventsRepository;

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();
    public IObservable<Guid> ScheduledEvents { get; }

    public IdentityProvidersPage(ISyncEventsRepository syncEventsRepository)
    {
        InitializeComponent();
        BindingContext = this;
        _syncEventsRepository=syncEventsRepository;

        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x =>
            {
                SwitchLayouts();
            });

        var scheduledEvents = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .Do(_ => SwitchLayouts())
            .SelectMany(_ => LoadEventsAsync())
            .Publish();

        _addEventConnection = scheduledEvents.Connect();
        ScheduledEvents = scheduledEvents.SelectMany(x => x.newEvents);
        _addOutlookIdentity = scheduledEvents
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x => Identities.Add(new Identity(x.username)));
    }

    private async Task<(string username, List<Guid> newEvents)> LoadEventsAsync()
    {
        var manager = new MicrosoftAuthorizationManager();
        await manager.SignInAsync(App.AuthUIParent);
        var client = await manager.GetGraphClientAsync();
        string username = manager.CurrentAccounts.Last().Username;
        var events = await new OutlookProvider(client, username).GetEventsAsync();
        var syncEvents = events.Select(e => new SyncEvent
        {
            ExternalId = e.Id,
            Title = e.Subject,
            Description = e.Body.Content,
            Namespace = new Namespace(),
            Schedule = new SyncSchedule(),
            Alert = new SyncAlert(),
            Status = SyncStatus.Active,
            Start = DateTime.Parse(e.Start.DateTime),
            End = DateTime.Parse(e.End.DateTime)
        });

        var newEvents = syncEvents.Select(_syncEventsRepository.AddSyncEvent).ToList();

        return (username, newEvents);
    }

    private void SwitchLayouts()
    {
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
    }

    private void OnSyncClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: Identity selectedItem })
        {
            // sync identity
        }

        // sync all
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentity.Dispose();
        _addEventConnection.Dispose();
    }
}
