using System.Collections.ObjectModel;
using SyncMe.Models;

namespace SyncMe.ViewModels;

public class IdentityProvidersViewModel : BaseViewModel
{
    private readonly IIdentitiesService _identitiesService;

    public IdentityProvidersViewModel(IIdentitiesService identitiesService)
    {
        _identitiesService = identitiesService;
    }

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();
    
    public string Image => ProvidersOpenned ? "icon_arrow_major.png" : "icon_plus_minor.xml";

    private bool _isSyncronized = true;
    public bool IsSyncronized
    {
        get => _isSyncronized;
        set
        {
            ChangeProperty(ref _isSyncronized, value, nameof(IsSyncronized));
        }
    }

    private bool _providersOpenned;
    public bool ProvidersOpenned
    {
        get => _providersOpenned;
        set
        {
            if (ChangeProperty(ref _providersOpenned, value, nameof(ProvidersOpenned)))
            {
                OnPropertyChanged(nameof(Image));
            }
        }
    }

    public async Task LoadEventsAsync(string identityName)
    {
        if (IsSyncronized)
        {
            try
            {
                IsSyncronized = false;
                await _identitiesService.LoadEventsAsync(identityName);
            }
            finally
            {
                IsSyncronized = true;
            }
        }
    }
}
