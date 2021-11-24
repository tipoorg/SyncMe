using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CalendarProviders.Authorization
{
    public class MicrosoftAuthorizationManager
    {
        public string[] Scopes => new[] { "Calendars.Read" };
        public string iOSKeychainSecurityGroup { get; private set; } = null;
        public IPublicClientApplication PCA { get; private set; }
        public static IEnumerable<IAccount> CurrentAccounts { get; private set; } = new List<IAccount>();

        public MicrosoftAuthorizationManager()
        {
            var builder = PublicClientApplicationBuilder
            .Create(OAuthSettings.ApplicationId)
            .WithRedirectUri(OAuthSettings.RedirectUri);

            if (!string.IsNullOrEmpty(iOSKeychainSecurityGroup))
            {
                builder = builder.WithIosKeychainSecurityGroup(iOSKeychainSecurityGroup);
            }

            PCA = builder.Build();
            CurrentAccounts = PCA.GetAccountsAsync().Result;
        }

        public async Task<string> SignInAsync(object AuthUIParent)
        {
            // This exception is thrown when an interactive sign-in is required.
            // Prompt the user to sign-in
            var interactiveRequest = PCA.AcquireTokenInteractive(Scopes);

            if (AuthUIParent != null)
            {
                interactiveRequest = interactiveRequest
                    .WithParentActivityOrWindow(AuthUIParent);
            }

            var result = await interactiveRequest.ExecuteAsync();
            return result.Account.Username;
        }

        public async Task<GraphServiceClient> GetGraphClientAsync(string username)
        {
            if (CurrentAccounts.Count() > 0)
            {
                // Initialize Graph client
                return new GraphServiceClient(new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        var currentAccount = CurrentAccounts.FirstOrDefault(a => a.Username == username);
                        var result = await PCA.AcquireTokenSilent(Scopes, currentAccount)
                            .ExecuteAsync();

                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));
            }

            return null;
        }
    }
}
