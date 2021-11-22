//using Microsoft.Graph;
namespace SyncMe.Views;

public partial class CreateEvent : ContentPage
{
    private static readonly DateTime _minimumDate = new(2000, 1, 1);
    private static readonly DateTime _maximumDate = new(2100, 12, 31);

    private Entry Namespace { get; init; }
    private Entry EventTitle { get; init; }
    private Switch IsAllDay { get; init; }
    private DatePicker StartsPicker { get; init; }
    private DatePicker EndsPicker { get; init; }
    private Button ConfigureAlert { get; init; }
    private Button AddEvent { get; init; }

    public CreateEvent()
    {
        InitializeComponent();

        Namespace = new Entry { Placeholder = "Namespace" };
        EventTitle = new Entry { Placeholder = "Title" };

        IsAllDay = new Switch { IsToggled = false, OnColor = Color.FromRgb(74, 215, 100), ThumbColor = Color.White };
        IsAllDay.Toggled += OnSwitchToggled;
        StartsPicker = new DatePicker { MinimumDate = _minimumDate, MaximumDate = _maximumDate };
        EndsPicker = new DatePicker { MinimumDate = _minimumDate, MaximumDate = _maximumDate };

        ConfigureAlert = new Button { Text = "Alert" };
        ConfigureAlert.Clicked += AlertButton_Clicked;

        AddEvent = new() { Text = "Add" };
        AddEvent.Clicked += OnButtonClicked;

        var stack = CreatePageLayout();
        Content = new ScrollView { Content = stack };
    }

    private StackLayout CreatePageLayout() => new() { Spacing = 20, Children = { CreateHeader(), CreateSchedule(), CreateAlert(), AddEvent } };

    private StackLayout CreateHeader() => new() { Children = { Namespace, EventTitle } };

    private StackLayout CreateSchedule()
    {
        var grid = new Grid { Children = { new Label { Text = "All-Day" }, IsAllDay } };

        return new StackLayout()
        {
            Children = { grid, new Label { Text = "Starts" }, StartsPicker, new Label { Text = "Ends" }, EndsPicker },
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

    private async void AlertButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EventAlert());
    }

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        //var newEvent = new Event
        //{
        //    Start = new DateTimeTimeZone { DateTime = StartsPicker.Date.ToString() },
        //    End = new DateTimeTimeZone { DateTime = EndsPicker.Date.ToString() },
        //    IsAllDay = IsAllDay.IsToggled,
        //    Subject = EventTitle.Text
        //};

        //EventRepository.Events.Add(newEvent);
    }
}
