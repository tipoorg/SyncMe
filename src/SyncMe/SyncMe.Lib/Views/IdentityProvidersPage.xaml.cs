using System.Reactive.Linq;
using SyncMe.Functional;
using SyncMe.Models;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;


[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentitySubscription;
    private readonly IdentityProvidersViewModel _viewModel;
    private readonly IIdentitiesService _identitiesService;

    public IdentityProvidersPage(IdentityProvidersViewModel viewModel, IIdentitiesService identitiesService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _identitiesService = identitiesService;
        
        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x => SwitchLayouts());

        _addOutlookIdentitySubscription = _identitiesService.GetIdentities()
            .Concat(Observable
                .FromEventPattern(AddOutlook, nameof(Button.Clicked))
                .Do(_ => SwitchLayouts())
                .SelectMany(_ => _identitiesService.AddNewIdentityAsync())
                .Filter(username => !_viewModel.Identities.Any(i => i.Name == username)))
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(username => _viewModel.Identities.Add(new Identity(username)));
    }

    private void SwitchLayouts()
    {
        _viewModel.ProvidersOpenned = !_viewModel.ProvidersOpenned;
    }

    private void OnSyncAllClicked(object sender, EventArgs e)
    {
        _identitiesService.GetIdentities()
            .Subscribe(async x => await _viewModel.LoadEventsAsync(x));
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: Identity selectedItem })
        {
            await _viewModel.LoadEventsAsync(selectedItem.Name);
        }
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentitySubscription.Dispose();
    }
}
