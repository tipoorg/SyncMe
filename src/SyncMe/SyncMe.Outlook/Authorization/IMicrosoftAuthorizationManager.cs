using LanguageExt;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace SyncMe.Outlook.Authorization;

public interface IMicrosoftAuthorizationManager
{
    IPublicClientApplication PCA { get; }

    Task<GraphServiceClient> GetGraphClientAsync(string username);
    Task<Option<string>> TrySignInAsync(object AuthUIParent);
}
