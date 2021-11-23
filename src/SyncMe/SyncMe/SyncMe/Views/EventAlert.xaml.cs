using SyncMe.Elements;
using SyncMe.Models;

namespace SyncMe.Views;

public partial class EventAlert : ContentPage
{
    private readonly CreateEvent createEvent;
    public ButtonWithValue<Reminder> None { get; init; }
    public ButtonWithValue<Reminder> AtTimeOfEvent { get; init; }
    public ButtonWithValue<Reminder> FiveMinBefore { get; init; }
    public ButtonWithValue<Reminder> TenMinBefore { get; init; }
    public ButtonWithValue<Reminder> FiveteenMinBefore { get; init; }
    public ButtonWithValue<Reminder> ThirtyMinBefore { get; init; }
    public ButtonWithValue<Reminder> HourBefore { get; init; }
    public ButtonWithValue<Reminder> TwoHoursBefore { get; init; }
    public ButtonWithValue<Reminder> DayBefore { get; init; }
    public ButtonWithValue<Reminder> TwoDaysBefore { get; init; }
    public ButtonWithValue<Reminder> WeekBefore { get; init; }

    public EventAlert(CreateEvent createEvent)
    {
        InitializeComponent();

        None = CreateAndSubscribe("None", Reminder.None);
        AtTimeOfEvent = CreateAndSubscribe("At time of event", Reminder.AtEventTime);
        FiveMinBefore = CreateAndSubscribe("5 minutes before", Reminder.Before5Min);
        TenMinBefore = CreateAndSubscribe("10 minutes before", Reminder.Before10Min);
        FiveteenMinBefore = CreateAndSubscribe("15 minutes before", Reminder.Before15Min);
        ThirtyMinBefore = CreateAndSubscribe("30 minutes before", Reminder.Before30Min);
        HourBefore = CreateAndSubscribe("1 hour before", Reminder.Before1Hour);
        TwoHoursBefore = CreateAndSubscribe("2 hours before", Reminder.TwoDaysBefore);
        DayBefore = CreateAndSubscribe("1 day before", Reminder.DayBefore);
        TwoDaysBefore = CreateAndSubscribe("2 days before", Reminder.TwoDaysBefore);
        WeekBefore = CreateAndSubscribe("1 week before", Reminder.OneWeekBefore);

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

    private ButtonWithValue<Reminder> CreateAndSubscribe(string text, Reminder reminder)
    {
        ButtonWithValue<Reminder> button = new() { Text = text, Value = reminder };
        button.Clicked += OnClicked;
        return button;
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        if (sender is ButtonWithValue<Reminder> button)
        {
            createEvent.ConfigureAlert.Text = $"Alert {button.Text}";
            createEvent.ConfigureAlert.Value = button.Value;
        }

        await Navigation.PopAsync();
    }
}
