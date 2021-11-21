using System.Collections.Generic;

namespace CustomAlarm.MAUI.Pages;

public partial class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        InitializeComponent();

        var grid = new Grid
        {
            new ListView
            {
                ItemsSource = new List<string>
                {
                    "1", "2"
                }
            },
        };
        grid.AddRowDefinition(new RowDefinition());

        Content = grid;
    }
}
