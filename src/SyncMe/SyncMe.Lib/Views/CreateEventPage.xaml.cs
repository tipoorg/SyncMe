using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using dotMorten.Xamarin.Forms;
using SyncMe.Extensions;
using SyncMe.Models;
using SyncMe.ViewModels;

namespace SyncMe.Views;

public sealed partial class CreateEventPage : ContentPage, IDisposable
{
    private readonly ISyncNamespaceService _namespaceService;
    private readonly ISyncEventsService _syncEventsService;
    private readonly IReadOnlyCollection<Namespace> _namespaces;
    private readonly IDisposable _addEventSubscription;
    private readonly SyncEventViewModel _eventModel;

    public CreateEventPage(ISyncEventsService syncEventsService, ISyncNamespaceService namespaceService)
    {
        _eventModel = new SyncEventViewModel();
        InitializeComponent();
        _namespaceService = namespaceService;
        _syncEventsService = syncEventsService;
        _namespaces = _namespaceService.GetAllSyncNamespaces();
        BindingContext = _eventModel;

        _addEventSubscription = Observable
            .FromEventPattern(AddEvent, nameof(Button.Clicked))
            .SelectMany(x => AddNewSyncEvent())
            .Do(x => _namespaceService.Add(_eventModel.Namespace))
            .Subscribe(async x => await NavigateToCalendar());
    }

    private void OnSuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        if (sender is AutoSuggestBox autoSuggest)
        {
            autoSuggest.Text = $"{e.SelectedItem}";
        }
    }

    private void OnTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender is AutoSuggestBox autoSuggest)
        {
           _eventModel.Namespace = autoSuggest.Text;

            autoSuggest.ItemsSource = _namespaces
                .Select(x => x.Key)
                .Where(x => x.StartsWith(autoSuggest.Text, StringComparison.OrdinalIgnoreCase))
                .Distinct()
                .ToArray();
        }
    }

    private async void OnConfigureScheduleClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventSchedulePage(_eventModel));

    private async void OnAlertButtonClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventAlertPage(_eventModel));

    private async Task<Guid> AddNewSyncEvent()
    {
        var guid = _syncEventsService.AddSyncEvent(_eventModel.ToSyncEvent().TrimNamespaceEnd());
        await NavigateToCalendar();
        return guid;
    }

    private static async Task<Unit> NavigateToCalendar()
    {
        await Shell.Current.GoToAsync("//calendar");
        return Unit.Default;
    }

    public void Dispose() => _addEventSubscription.Dispose();
}
