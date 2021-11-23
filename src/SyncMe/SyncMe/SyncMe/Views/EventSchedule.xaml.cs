using SyncMe.Elements;
using SyncMe.Models;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EventSchedule : ContentPage
{
    private readonly ButtonWithValue<Repeat> _scheduleButton;

    public Label DoesNotRepeatLabel { get; set; }
    public RadioWithValue<Repeat> DoesNotRepeat { get; init; }
    public Label EveryDayLabel { get; set; }
    public RadioWithValue<Repeat> EveryDay { get; init; }
    public Label EveryWeekLabel { get; set; }
    public RadioWithValue<Repeat> EveryWeek { get; init; }
    public Label EveryMonthLabel { get; set; }
    public RadioWithValue<Repeat> EveryMonth { get; init; }
    public Label EveryYearLabel { get; set; }
    public RadioWithValue<Repeat> EveryYear { get; init; }

    public EventSchedule(ButtonWithValue<Repeat> button)
    {
        InitializeComponent();
        DoesNotRepeat = CreateButton(Repeat.None);
        EveryDay = CreateButton(Repeat.Dayly);
        EveryWeek = CreateButton(Repeat.WeekDays);
        EveryMonth = CreateButton(Repeat.EveryMonth);
        EveryYear = CreateButton(Repeat.EveryYear);
        DoesNotRepeatLabel = CreateLabel("Does not repeat");
        EveryDayLabel = CreateLabel("Every day");
        EveryWeekLabel = CreateLabel("Every week");
        EveryMonthLabel = CreateLabel("Every month");
        EveryYearLabel = CreateLabel("Every year");

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
        _scheduleButton = button;
    }

    private StackLayout CreateLayout(RadioButton radioButton, Label label) => new() { Children = { radioButton, label }, Orientation = StackOrientation.Horizontal };


    private RadioWithValue<Repeat> CreateButton(Repeat value)
    {
        var radioButton = new RadioWithValue<Repeat> { Value = value };
        radioButton.CheckedChanged += OnCheckedChanged;
        return radioButton;
    }

    private Label CreateLabel(string text) => new Label { Text = text };

    private async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioWithValue<Repeat> radio)
        {
            var text = radio.Value switch
            {
                Repeat.None => DoesNotRepeatLabel.Text,
                Repeat.Dayly => EveryDayLabel.Text,
                Repeat.WeekDays => EveryWeekLabel.Text,
                Repeat.EveryMonth => EveryMonthLabel.Text,
                Repeat.EveryYear => EveryYearLabel.Text,
                _ => throw new NotImplementedException(),
            };
            _scheduleButton.Text = text;
        }
        await Navigation.PopAsync();
    }
}
