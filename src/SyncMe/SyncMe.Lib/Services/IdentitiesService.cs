using LanguageExt;
using SyncMe.CalendarProviders.Authorization;
using SyncMe.CalendarProviders.Extensions;
using SyncMe.CalendarProviders.Outlook;
using SyncMe.Models;

namespace SyncMe.Lib.Services;

internal class IdentitiesService : IIdentitiesService
{
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISyncNamespaceRepository _syncNamespaceRepository;
    private readonly MicrosoftAuthorizationManager _authorizationManager;

    public IdentitiesService(
        ISyncEventsService syncEventsService,
        ISyncNamespaceRepository syncNamespaceRepository,
        MicrosoftAuthorizationManager authorizationManager)
    {
        _syncEventsService = syncEventsService;
        _syncNamespaceRepository = syncNamespaceRepository;
        _authorizationManager = authorizationManager;
    }

    public async Task<Option<string>> AddNewIdentity()
    {
        var username = await _authorizationManager.TrySignInAsync(App.AuthUIParent);
        await username.MapAsync(LoadEventsAsync).IsSome;

        return username;
    }

    public async Task LoadEventsAsync(string username)
    {
        var fetchedEvents = await FetchEventsAsync(username);
        _syncEventsService.RemoveEvents(e => username == e.ExternalEmail);
        _ = fetchedEvents.Select(_syncEventsService.AddSyncEvent).ToList();
    }

    private async Task<IEnumerable<SyncEvent>> FetchEventsAsync(string username)
    {
        var outlookNamespace = new Namespace { Key = "Outlook", IsActive = true };

        var client = await _authorizationManager.GetGraphClientAsync(username);
        var events = await new OutlookProvider(client, username).GetEventsAsync();

        _syncNamespaceRepository.TryAddSyncNamespace(outlookNamespace.Key);

        return events.Select(e => e.ToSyncEvent(username) with
        {
            NamespaceKey = outlookNamespace.Key
        });
    }
}
