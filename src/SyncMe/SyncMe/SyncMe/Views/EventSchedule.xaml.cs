using SyncMe.Elements;
using SyncMe.Models;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EventSchedule : ContentPage
{
    private readonly CreateEventPage _createEvent;

    public Label DoesNotRepeatLabel { get; set; }
    public RadioWithValue<SyncRepeat> DoesNotRepeat { get; init; }
    public Label EveryDayLabel { get; set; }
    public RadioWithValue<SyncRepeat> EveryDay { get; init; }
    public Label EveryWeekLabel { get; set; }
    public RadioWithValue<SyncRepeat> EveryWeek { get; init; }
    public Label EveryMonthLabel { get; set; }
    public RadioWithValue<SyncRepeat> EveryMonth { get; init; }
    public Label EveryYearLabel { get; set; }
    public RadioWithValue<SyncRepeat> EveryYear { get; init; }

    public EventSchedule(CreateEventPage createEvent)
    {
        InitializeComponent();
        DoesNotRepeat = CreateButton(SyncRepeat.None);
        EveryDay = CreateButton(SyncRepeat.Dayly);
        EveryWeek = CreateButton(SyncRepeat.WeekDays);
        EveryMonth = CreateButton(SyncRepeat.EveryMonth);
        EveryYear = CreateButton(SyncRepeat.EveryYear);
        DoesNotRepeatLabel = CreateLabel("Does not repeat");
        EveryDayLabel = CreateLabel("Every day");
        EveryWeekLabel = CreateLabel("Every week");
        EveryMonthLabel = CreateLabel("Every month");
        EveryYearLabel = CreateLabel("Every year");

        Disappearing += ResetState;
        Appearing += ResetState;

        var layout = new StackLayout
        {
            Children = 
            {
                CreateLayout(DoesNotRepeat, DoesNotRepeatLabel),
                CreateLayout(EveryDay, EveryDayLabel),
                CreateLayout(EveryWeek, EveryWeekLabel),
                CreateLayout(EveryMonth, EveryMonthLabel),
                CreateLayout(EveryYear, EveryYearLabel),
            }
        };

        Content = layout;
        _createEvent = createEvent;
    }

    private void ResetState(object sender, EventArgs e)
    {
        DoesNotRepeat.IsChecked = false;
        EveryDay.IsChecked = false;
        EveryWeek.IsChecked = false;
        EveryMonth.IsChecked = false;
        EveryYear.IsChecked = false;
    }

    private StackLayout CreateLayout(RadioButton radioButton, Label label) => new() { Children = { radioButton, label }, Orientation = StackOrientation.Horizontal };


    private RadioWithValue<SyncRepeat> CreateButton(SyncRepeat value)
    {
        var radioButton = new RadioWithValue<SyncRepeat> { Data = value };
        radioButton.CheckedChanged += OnCheckedChanged;
        return radioButton;
    }

    private Label CreateLabel(string text) => new Label { Text = text };

    private async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioWithValue<SyncRepeat> radio)
        {
            var text = radio.Data switch
            {
                SyncRepeat.None => DoesNotRepeatLabel.Text,
                SyncRepeat.Dayly => EveryDayLabel.Text,
                SyncRepeat.WeekDays => EveryWeekLabel.Text,
                SyncRepeat.EveryMonth => EveryMonthLabel.Text,
                SyncRepeat.EveryYear => EveryYearLabel.Text,
                _ => throw new NotImplementedException(),
            };
            _createEvent.ConfigureSchedule.Text = text;
            _createEvent.ConfigureSchedule.Value = radio.Data;
        }
        await Navigation.PopAsync();
    }
}
