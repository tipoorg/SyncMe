namespace SyncMe.Views;

public partial class EventAlert : ContentPage
{
    public EventAlert()
    {
        InitializeComponent();
        Content = new StackLayout
        {
            Children =
            {
                new Button { Text = "Text" }
            }
        };
    }

    async void OnButtonClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://aka.ms/xamarin-quickstart");
    }
}
