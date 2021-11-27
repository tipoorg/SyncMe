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

    private void OnSuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
    {
        if (sender is AutoSuggestBox autoSuggest)
        {
            autoSuggest.Text = $"{e.SelectedItem}";
            ValidateButtonState();
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

            ValidateButtonState();
        }
    }

    private async void OnConfigureScheduleClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventSchedulePage(_eventModel));

    private async void OnAlertButtonClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventAlertPage(_eventModel));

    private async Task<Guid> AddNewSyncEvent()
    {
        var guid = _eventsRepository.AddSyncEvent(_eventModel.SyncEvent.TrimNamespaceEnd() with { });
        await NavigateToCalendar();
        return guid;
    }

    private async void CancelEvent(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_eventModel.Namespace))
        {
            if (await DisplayAlert(null, "Discard this event?", "Discard", "Keep editing"))
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

    private void ValidateButtonState(object sender, PropertyChangedEventArgs e) => ValidateButtonState();

    private void ValidateButtonState()
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

    public void Dispose() => _addEventSubscription.Dispose();
}
