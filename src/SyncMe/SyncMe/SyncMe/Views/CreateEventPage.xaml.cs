using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using dotMorten.Xamarin.Forms;
using SyncMe.Models;
using SyncMe.Repos;

namespace SyncMe.Views;

public sealed partial class CreateEventPage : ContentPage, IDisposable
{
    private readonly ISyncNamespaceRepository _namespaceRepository;
    private readonly ISyncEventsRepository _eventsRepository;
    private readonly Dictionary<string, Namespace> _namespaces;
    private readonly IDisposable _addEventSubscription;
    private readonly SyncEventViewModel _eventModel;

    public CreateEventPage(ISyncEventsRepository eventsRepository, ISyncNamespaceRepository namespaceRepository)
    {
        _eventModel = new SyncEventViewModel();
        InitializeComponent();
        _namespaceRepository = namespaceRepository;
        _eventsRepository = eventsRepository;
        _namespaces = _namespaceRepository.GetAllSyncNamespaces();
        BindingContext = _eventModel;

        _addEventSubscription = Observable
            .FromEventPattern(AddEvent, nameof(Button.Clicked))
            .SelectMany(x => AddNewSyncEvent())
            .Do(x => _namespaceRepository.AddSyncNamespace(_eventModel.Namespace))
            .Subscribe(async x => await NavigateToCalendar());
    }

    private void OnQuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
        if (e.ChosenSuggestion != null)
        {
            // User selected an item from the suggestion list, take an action on it here.
        }
        else
        {
            // User hit Enter from the search box. Use args.QueryText to determine what to do.
        }
    }

    private void OnSuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        if (sender is AutoSuggestBox autoSuggest)
        {
            var particles = autoSuggest.Text.Split('.');
            //if (particles.Length > 1)

            autoSuggest.Text = $"{e.SelectedItem}.";
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
        var guid = _eventsRepository.AddSyncEvent(_eventModel.SyncEvent);
        await NavigateToCalendar();
        return guid;
    }

    private async void CancelEvent(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_eventModel.Namespace))
        {
            if (await DisplayAlert(null, "Discard this event?", "Keep editing", "Discard"))
            {
                await NavigateToCalendar();
            }
        }
    }

    private static async Task<Unit> NavigateToCalendar()
    {
        await Shell.Current.GoToAsync("//calendar");
        return Unit.Default;
    }

    private void ValidateButtonState(object sender, PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_eventModel.Namespace) && !string.IsNullOrEmpty(_eventModel.Title))
        {
            AddEvent.IsEnabled = true;
        }
        else
        {
            AddEvent.IsEnabled = false;
        }
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            _eventModel.StartDate = DateTime.Today.Date;
            _eventModel.EndDate = DateTime.Today.Date.AddDays(1).AddTicks(-1);
        }
    }

    public void Dispose()
    {
        _addEventSubscription.Dispose();
    }
}
