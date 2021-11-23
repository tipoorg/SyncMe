using System.ComponentModel;
using dotMorten.Xamarin.Forms;
using SyncMe.Elements;
using SyncMe.Models;
using SyncMe.Repos;

namespace SyncMe.Views;

public partial class CreateEvent : ContentPage
{
    private static readonly DateTime _minimumDate = new(2000, 1, 1);
    private static readonly DateTime _maximumDate = new(2100, 12, 31);
    private readonly ISyncNamespaceRepository _namespaceRepository;
    private readonly ISyncEventsRepository _eventsRepository;

    public AutoSuggestBox Namespace { get; init; }
    public Entry EventTitle { get; init; }
    public Switch IsAllDay { get; init; }
    public DatePicker StartsDate { get; init; }
    public TimePicker StartsTime { get; init; }
    public DatePicker EndsDate { get; init; }
    public TimePicker EndsTime { get; init; }
    public ButtonWithValue<SyncRepeat> ConfigureSchedule { get; init; }
    public ButtonWithValue<SyncReminder> ConfigureAlert { get; init; }
    public ToolbarItem AddEvent { get; init; }

    public CreateEvent(ISyncEventsRepository eventsRepository, ISyncNamespaceRepository namespaceRepository)
    {
        InitializeComponent();
        _namespaceRepository = namespaceRepository;
        _eventsRepository = eventsRepository;

        AddEvent = new ToolbarItem { Text = "Add event", };
        AddEvent.Clicked += OnAddEventClicked;
        Namespace = new AutoSuggestBox { PlaceholderText = "Namespace" };
        Namespace.PropertyChanged += ValidateButtonState;
        Namespace.TextChanged += OnTextChanged;
        Namespace.SuggestionChosen += OnSuggestionChosen;
        Namespace.QuerySubmitted += OnQuerySubmitted;
        EventTitle = new Entry { Placeholder = "Title" };
        EventTitle.PropertyChanged += ValidateButtonState;

        IsAllDay = new Switch { IsToggled = false, OnColor = Color.FromRgb(74, 215, 100), ThumbColor = Color.White };
        IsAllDay.Toggled += OnSwitchToggled;
        StartsDate = new DatePicker { MinimumDate = _minimumDate, MaximumDate = _maximumDate };
        StartsTime = new TimePicker();
        EndsDate = new DatePicker { MinimumDate = _minimumDate, MaximumDate = _maximumDate };
        EndsTime = new TimePicker();
        ConfigureSchedule = new ButtonWithValue<SyncRepeat> { Text = "Does not repeat", };
        ConfigureSchedule.Clicked += ConfigureSchedule_Clicked;

        ConfigureAlert = new ButtonWithValue<SyncReminder> { Text = "Alert" };
        ConfigureAlert.Clicked += AlertButton_Clicked;

        var stack = CreatePageLayout();
        Content = new ScrollView { Content = stack };
        var cancelEventCreation = new ToolbarItem { Text = "Cancel" };
        cancelEventCreation.Clicked += OnCancelEventCreationClicked;

        ToolbarItems.Add(AddEvent);
        ToolbarItems.Add(cancelEventCreation);
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
            if (particles.Length > 1)

            autoSuggest.Text = $"{e.SelectedItem}.";
        }
    }

    private void OnTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput && sender is AutoSuggestBox autoSuggest)
        {
            autoSuggest.ItemsSource = _namespaceRepository
                .GetAllSyncNamespacrs()
                .Select(x => x.Title)
                .Where(x => x.StartsWith(autoSuggest.Text, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
    }

    private async void ConfigureSchedule_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventSchedule(this));

    private async void OnCancelEventCreationClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Namespace.Text))
        {
            if (!await DisplayAlert(null, "Discard this event?", "Keep editing", "Discard"))
            {
                CleanUpElements();
            }
        }
        await NavigateToNotes();
    }

    private static async Task NavigateToNotes() => await Shell.Current.GoToAsync("//notes");

    private void CleanUpElements()
    {
        Namespace.Text = string.Empty;
        EventTitle.Text = string.Empty;
    }

    private void ValidateButtonState(object sender, PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Namespace.Text))
        {
            AddEvent.IsEnabled = true;
        }
        else
        {
            AddEvent.IsEnabled = false;
        }
    }

    private StackLayout CreatePageLayout() => new() { Spacing = 20, Children = { CreateHeader(), CreateSchedule(), CreateAlert() } };

    private StackLayout CreateHeader() => new() { Children = { Namespace, EventTitle } };

    private StackLayout CreateSchedule()
    {
        var grid = new Grid { Children = { new Label { Text = "All-Day" }, IsAllDay } };

        return new StackLayout()
        {
            Children =
            {
                grid, ConfigureSchedule, 
                BuildDateLayout("Start date", StartsDate,StartsTime),
                BuildDateLayout("End date", EndsDate, EndsTime),
            },
            Padding = new Thickness(0, 10)
        };
    }

    private StackLayout BuildDateLayout(string labelText, DatePicker datepicker, TimePicker timepicker)
    {
        return new StackLayout
        {
            Children = { new Label { Text = labelText, VerticalTextAlignment = TextAlignment.Center }, datepicker, timepicker},
            Orientation = StackOrientation.Horizontal
        };
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            StartsDate.Date = DateTime.Today.Date;
            EndsDate.Date = DateTime.Today.Date.AddDays(1).AddTicks(-1);
        }
    }

    private StackLayout CreateAlert() => new() { Children = { new Label { Text = "Alert" }, ConfigureAlert }, Padding = new Thickness(0, 10) };

    private async void AlertButton_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventAlert(this));

    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        var guid = Guid.NewGuid();
        var newEvent = new SyncEvent(guid, "", EventTitle.Text, "", new Namespace(1, Namespace.Text), new SyncSchedule(ConfigureSchedule.Value, null), new SyncAlert(new SyncReminder[] { ConfigureAlert.Value }), SyncStatus.Active, StartsDate.Date, EndsDate.Date);
        _eventsRepository.AddSyncEvent(newEvent);   
        await NavigateToNotes();
        CleanUpElements();
    }
}
