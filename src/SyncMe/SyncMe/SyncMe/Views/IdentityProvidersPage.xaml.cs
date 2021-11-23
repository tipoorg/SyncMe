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

                var syncEvents = events.Select(e => new SyncEvent(Guid.Empty, e.Id, e.Subject, e.Body.Content, new Namespace(0, ""), new SyncSchedule(SyncRepeat.None, null),
                                                 new SyncAlert(new SyncReminder[] { }), SyncStatus.Active,
                                                 DateTime.Parse(e.Start.DateTime), DateTime.Parse(e.End.DateTime)));
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
        AddIdentity.IsVisible = !AddIdentity.IsVisible;
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentity.Dispose();
    }
}
