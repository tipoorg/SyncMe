using SyncMe.Elements;

namespace SyncMe.Views;

public partial class EventAlert : ContentPage
{
    private readonly CreateEvent createEvent;
    public ButtonWithLoad<int> None { get; init; }
    public ButtonWithLoad<int> AtTimeOfEvent { get; init; }
    public ButtonWithLoad<int> FiveMinBefore { get; init; }
    public ButtonWithLoad<int> TenMinBefore { get; init; }
    public ButtonWithLoad<int> FiveteenMinBefore { get; init; }
    public ButtonWithLoad<int> ThirtyMinBefore { get; init; }
    public ButtonWithLoad<int> HourBefore { get; init; }
    public ButtonWithLoad<int> TwoHoursBefore { get; init; }
    public ButtonWithLoad<int> DayBefore { get; init; }
    public ButtonWithLoad<int> TwoDaysBefore { get; init; }
    public ButtonWithLoad<int> WeekBefore { get; init; }

    public EventAlert(CreateEvent createEvent)
    {
        InitializeComponent();

        None = CreateAndSubscribe("None", -1);
        AtTimeOfEvent = CreateAndSubscribe("At time of event", 0);
        FiveMinBefore = CreateAndSubscribe("5 minutes before", 5);
        TenMinBefore = CreateAndSubscribe("10 minutes before", 10);
        FiveteenMinBefore = CreateAndSubscribe("15 minutes before", 15);
        ThirtyMinBefore = CreateAndSubscribe("30 minutes before", 30);
        HourBefore = CreateAndSubscribe("1 hour before", 60);
        TwoHoursBefore = CreateAndSubscribe("2 hours before", 120);
        DayBefore = CreateAndSubscribe("1 day before", 1440);
        TwoDaysBefore = CreateAndSubscribe("2 days before", 2880);
        WeekBefore = CreateAndSubscribe("1 week before", 10080);

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

    private ButtonWithLoad<int> CreateAndSubscribe(string text, int load)
    {
        ButtonWithLoad<int> button = new() { Text = text, Load = load };
        button.Clicked += OnClicked;
        return button;
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        if (sender is ButtonWithLoad<int> button)
            createEvent.ConfigureAlert.Text += $" {button.Load}";

        await Navigation.PopAsync();
    }
}
