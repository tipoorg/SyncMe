using LanguageExt;

namespace SyncMe;

public interface IIdentitiesService
{
    Task<Option<string>> AddNewIdentity();
    Task LoadEventsAsync(string username);
}
