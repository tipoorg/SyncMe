using SyncMe.Elements;
using SyncMe.Models;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EventSchedule : ContentPage
{
    private readonly ButtonWithValue<Repeat> _scheduleButton;

    public RadioWithValue<Repeat> DoesNotRepeat { get; init; }
    public RadioWithValue<Repeat> EveryDay { get; init; }
    public RadioWithValue<Repeat> EveryWeek { get; init; }
    public RadioWithValue<Repeat> EveryMonth { get; init; }
    public RadioWithValue<Repeat> EveryYear { get; init; }
    public RadioWithValue<Repeat> Custom { get; init; }

    public EventSchedule(ButtonWithValue<Repeat> button)
    {
        InitializeComponent();
        DoesNotRepeat = CreateAndSubscribe(Repeat.None);
        EveryDay = CreateAndSubscribe(Repeat.Dayly);
        EveryWeek = CreateAndSubscribe(Repeat.WeekDays);
        EveryMonth = CreateAndSubscribe(Repeat.EveryMonth);
        EveryYear = CreateAndSubscribe(Repeat.EveryYear);

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());

        grid.Children.AddVertical(new View[] { DoesNotRepeat, new Label { Text = "Does not repeat" } } );
        grid.Children.AddVertical(new View[] { EveryDay, new Label { Text = "Every day" } });
        grid.Children.AddVertical(new View[] { EveryWeek, new Label { Text = "Every week" } });
        grid.Children.AddVertical(new View[] { EveryMonth, new Label { Text = "Every month" } });
        grid.Children.AddVertical(new View[] { EveryYear, new Label { Text = "Every year" } });

        Content = grid;
        _scheduleButton = button;
    }

    private RadioWithValue<Repeat> CreateAndSubscribe(Repeat value)
    {
        var radioButton = new RadioWithValue<Repeat> { Value = value };
        radioButton.CheckedChanged += OnCheckedChanged;
        return radioButton;
    }

    private async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioWithValue<Repeat> radio)
        {
            _scheduleButton.Value = radio.Value;
        }
        await Navigation.PopAsync();
    }
}
