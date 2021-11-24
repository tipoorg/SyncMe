using SyncMe.Elements;
using SyncMe.Models;

namespace SyncMe.Views;

public partial class EventAlert : ContentPage
{
    private readonly CreateEventPage createEvent;
    public ButtonWithValue<SyncReminder> None { get; init; }
    public ButtonWithValue<SyncReminder> AtTimeOfEvent { get; init; }
    public ButtonWithValue<SyncReminder> FiveMinBefore { get; init; }
    public ButtonWithValue<SyncReminder> TenMinBefore { get; init; }
    public ButtonWithValue<SyncReminder> FiveteenMinBefore { get; init; }
    public ButtonWithValue<SyncReminder> ThirtyMinBefore { get; init; }
    public ButtonWithValue<SyncReminder> HourBefore { get; init; }
    public ButtonWithValue<SyncReminder> TwoHoursBefore { get; init; }
    public ButtonWithValue<SyncReminder> DayBefore { get; init; }
    public ButtonWithValue<SyncReminder> TwoDaysBefore { get; init; }
    public ButtonWithValue<SyncReminder> WeekBefore { get; init; }

    public EventAlert(CreateEventPage createEvent)
    {
        InitializeComponent();

        None = CreateAndSubscribe("None", SyncReminder.None);
        AtTimeOfEvent = CreateAndSubscribe("At time of event", SyncReminder.AtEventTime);
        FiveMinBefore = CreateAndSubscribe("5 minutes before", SyncReminder.Before5Min);
        TenMinBefore = CreateAndSubscribe("10 minutes before", SyncReminder.Before10Min);
        FiveteenMinBefore = CreateAndSubscribe("15 minutes before", SyncReminder.Before15Min);
        ThirtyMinBefore = CreateAndSubscribe("30 minutes before", SyncReminder.Before30Min);
        HourBefore = CreateAndSubscribe("1 hour before", SyncReminder.Before1Hour);
        TwoHoursBefore = CreateAndSubscribe("2 hours before", SyncReminder.TwoDaysBefore);
        DayBefore = CreateAndSubscribe("1 day before", SyncReminder.DayBefore);
        TwoDaysBefore = CreateAndSubscribe("2 days before", SyncReminder.TwoDaysBefore);
        WeekBefore = CreateAndSubscribe("1 week before", SyncReminder.OneWeekBefore);

        Content = new StackLayout 
        { 
            Children = 
            {
                None, AtTimeOfEvent, FiveMinBefore, TenMinBefore, FiveteenMinBefore, 
                ThirtyMinBefore,HourBefore, TwoHoursBefore, DayBefore, TwoDaysBefore, WeekBefore 
            }
        };
        this.createEvent = createEvent;
    }

    private ButtonWithValue<SyncReminder> CreateAndSubscribe(string text, SyncReminder reminder)
    {
        ButtonWithValue<SyncReminder> button = new() { Text = text, Value = reminder };
        button.Clicked += OnClicked;
        return button;
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        if (sender is ButtonWithValue<SyncReminder> button)
        {
            createEvent.ConfigureAlert.Text = $"Alert {button.Text}";
            createEvent.ConfigureAlert.Value = button.Value;
        }

        await Navigation.PopAsync();
    }
}
