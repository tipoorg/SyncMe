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
    private readonly IDisposable _addGoogleIdentity;

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage(ISyncEventsRepository syncEventsRepository)
    {
        InitializeComponent();
        BindingContext = this;

        var manager = new MicrosoftAuthorizationManager();

        foreach(var account in MicrosoftAuthorizationManager.CurrentAccounts)
        {
            Identities.Add(new Identity(account.Username));
        }

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
                var username = await manager.SignInAsync(App.AuthUIParent);
                var client = await manager.GetGraphClientAsync(username);
                var events = await new OutlookProvider(client, username).GetEventsAsync();

                if (Identities.Any(i => i.Name == username))
                    syncEventsRepository.RemoveEvents(e => username == e.ExternalEmail);

                var syncEvents = events.Select(e => e.ToSyncEvent(username));
                foreach(var @event in syncEvents)
                {
                    syncEventsRepository.AddSyncEvent(@event);
                }

                return username;
            }) // open browser and await task
            .Subscribe(x =>
            {
                Device.BeginInvokeOnMainThread(() => SwitchLayouts());

                if(!Identities.Any(i => i.Name == x))
                    Identities.Add(new Identity(x));
            });

        //Try to use google button as resync
        _addGoogleIdentity = Observable
            .FromEventPattern(AddGoogle, nameof(Button.Clicked))
            .SelectMany(async x =>
            {
                var manager = new MicrosoftAuthorizationManager();
                var accountsToResync = MicrosoftAuthorizationManager.CurrentAccounts.Select(a => a.Username).ToList();
                syncEventsRepository.RemoveEvents(e => accountsToResync.Contains(e.ExternalEmail));
                foreach (var account in MicrosoftAuthorizationManager.CurrentAccounts)
                {
                    var username = account.Username;
                    var client = await manager.GetGraphClientAsync(account.Username);
                    var events = await new OutlookProvider(client, username).GetEventsAsync();

                    var syncEvents = events.Select(e => e.ToSyncEvent(username));

                    foreach (var @event in syncEvents)
                    {
                        syncEventsRepository.AddSyncEvent(@event);
                    }
                }

                return manager;
            }) // open browser and await task
            .Subscribe(x =>
            {
                Device.BeginInvokeOnMainThread(() => SwitchLayouts());
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
