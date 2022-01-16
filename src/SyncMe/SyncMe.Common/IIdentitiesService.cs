using LanguageExt;

namespace SyncMe;

public interface IIdentitiesService
{
    IObservable<string> GetIdentities();
    Task<Option<string>> AddNewIdentityAsync();
    Task LoadEventsAsync(string username);
}
