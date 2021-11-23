using System.ComponentModel;
using SyncMe.Elements;
using SyncMe.Models;
using SyncMe.Repos;

namespace SyncMe.Views;

public partial class CreateEvent : ContentPage
{
    private static readonly DateTime _minimumDate = new(2000, 1, 1);
    private static readonly DateTime _maximumDate = new(2100, 12, 31);

    public Entry Namespace { get; init; }
    public Entry EventTitle { get; init; }
    public Switch IsAllDay { get; init; }
    public DatePicker StartsPicker { get; init; }
    public DatePicker EndsPicker { get; init; }
    public ButtonWithValue<Repeat> ConfigureSchedule { get; set; }
    public ButtonWithValue<Reminder> ConfigureAlert { get; init; }
    public ToolbarItem AddEvent { get; init; }

    public CreateEvent()
    {
        InitializeComponent();

        AddEvent = new ToolbarItem { Text = "Add event", };
        AddEvent.Clicked += OnAddEventClicked;
        Namespace = new Entry { Placeholder = "Namespace" };
        Namespace.PropertyChanged += ValidateButtonState;
        EventTitle = new Entry { Placeholder = "Title" };
        EventTitle.PropertyChanged += ValidateButtonState;

        IsAllDay = new Switch { IsToggled = false, OnColor = Color.FromRgb(74, 215, 100), ThumbColor = Color.White };
        IsAllDay.Toggled += OnSwitchToggled;
        StartsPicker = new DatePicker { MinimumDate = _minimumDate, MaximumDate = _maximumDate };
        EndsPicker = new DatePicker { MinimumDate = _minimumDate, MaximumDate = _maximumDate };
        ConfigureSchedule = new ButtonWithValue<Repeat> { Text = "Does not repeat", };
        ConfigureSchedule.Clicked += ConfigureSchedule_Clicked;

        ConfigureAlert = new ButtonWithValue<Reminder> { Text = "Alert" };
        ConfigureAlert.Clicked += AlertButton_Clicked;

        var stack = CreatePageLayout();
        Content = new ScrollView { Content = stack };
        var cancelEventCreation = new ToolbarItem { Text = "Cancel" };
        cancelEventCreation.Clicked += OnCancelEventCreationClicked;

        ToolbarItems.Add(AddEvent);
        ToolbarItems.Add(cancelEventCreation);
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
            Children = { grid, ConfigureSchedule, new Label { Text = "Starts" }, StartsPicker, new Label { Text = "Ends" }, EndsPicker },
            Padding = new Thickness(0, 10)
        };
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            StartsPicker.Date = DateTime.Today.Date;
            EndsPicker.Date = DateTime.Today.Date.AddDays(1).AddTicks(-1);
        }
    }

    private StackLayout CreateAlert() => new() { Children = { new Label { Text = "Alert" }, ConfigureAlert }, Padding = new Thickness(0, 10) };

    private async void AlertButton_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new EventAlert(this));

    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        var newEvent = new Event(EventTitle.Text, "", new Namespace(1, Namespace.Text), new Schedule(ConfigureSchedule.Value), new Alert(new Reminder[] { ConfigureAlert.Value }), Status.Active);
        EventRepository.Events.Add(newEvent);
        await NavigateToNotes();
        CleanUpElements();
    }
}
