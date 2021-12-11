using LanguageExt;

namespace SyncMe;

public interface IIdentitiesService
{
    IObservable<string> GetIdentities();
    Task<Option<string>> AddNewIdentity();
    Task LoadEventsAsync(string username);
}
