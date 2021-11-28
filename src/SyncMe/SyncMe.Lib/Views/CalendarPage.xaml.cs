using SyncMe.Lib.Services;
using SyncMe.Models;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CalendarPage : ContentPage
{
    private readonly CalendarPageViewModel _viewModel;
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISyncNamespaceService _namespaceService;
    private readonly IBackgroundColorService _backgroundColorService;
    private readonly IConfigRepository _configRepository;
    private readonly IdentityProvidersPage _identityProvidersPage;
    private readonly NamespaceManagmentPage _namespaceManagmentPage;

    public CalendarPage(
        CalendarPageViewModel viewModel,
        ISyncEventsService syncEventsService,
        ISyncNamespaceService namespaceService,
        IBackgroundColorService backgroundColorService,
        IConfigRepository configRepository,
        IdentityProvidersPage identityProvidersPage,
        NamespaceManagmentPage namespaceManagmentPage)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _syncEventsService = syncEventsService;
        _namespaceService = namespaceService;
        _backgroundColorService = backgroundColorService;
        _configRepository = configRepository;
        _identityProvidersPage=identityProvidersPage;
        _namespaceManagmentPage=namespaceManagmentPage;

        SoundToggle.IsToggled = !_configRepository.Get(ConfigKey.IsMute);
        ThemeToggle.IsToggled = !_configRepository.Get(ConfigKey.IsDarkTheme);
        AddEvent.Clicked += AddEvent_Clicked;
        year.BindingContext = Calendar1;
        if (!ThemeToggle.IsToggled)
            ChangeColor(isWhiteTheme: false);
    }

    protected override void OnAppearing()
    {
        _viewModel.InitEventsCollection();
        base.OnAppearing();
    }

    public async void AddEvent_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateEventPage(_syncEventsService, _namespaceService));
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: SyncEventViewModel { SyncEvent.Id: var eventId } })
        {
            _syncEventsService.TryRemoveInternalEvent(eventId);
            _viewModel.InitEventsCollection();
        }
    }

    private void OnSoundToggled(object sender, ToggledEventArgs e)
    {
        _configRepository.Set(ConfigKey.IsMute, !e.Value);
    }

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        _configRepository.Set(ConfigKey.IsDarkTheme, !e.Value);
        ChangeColor(e.Value);
    }

    private void ChangeColor(bool isWhiteTheme)
    {
        if (isWhiteTheme)
        {
            _backgroundColorService.UseWhiteTheme(this, _identityProvidersPage, _namespaceManagmentPage);
        }
        else
        {
            _backgroundColorService.UseDarkTheme(this, _identityProvidersPage, _namespaceManagmentPage);
        }
    }
}
