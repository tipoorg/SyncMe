using CalendarProviders.Authorization;
using SyncMe.Models;
using SyncMe.Providers.OutlookProvider;
using SyncMe.Repos;
using SyncMe.Extensions;
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

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage(ISyncEventsRepository syncEventsRepository)
    {
        InitializeComponent();
        BindingContext = this;

        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x =>
            {
                SwitchLayouts();
            });

        _addOutlookIdentity = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .SelectMany(async x =>
            {
                var manager = new MicrosoftAuthorizationManager();
                await manager.SignInAsync(App.AuthUIParent);
                var client = await manager.GetGraphClientAsync();
                var events = await new OutlookProvider(client, manager.CurrentAccounts.First().Username).GetEventsAsync();

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
                foreach(var @event in syncEvents)
                {
                    syncEventsRepository.AddSyncEvent(@event);
                }

                return manager;
            }) // open browser and await task
            .Subscribe(x =>
            {
                Device.BeginInvokeOnMainThread(() => SwitchLayouts());
                Identities.Add(new Identity(x.CurrentAccounts.First().Username));
            });
    }

    private void SwitchLayouts()
    {
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentity.Dispose();
    }
}
