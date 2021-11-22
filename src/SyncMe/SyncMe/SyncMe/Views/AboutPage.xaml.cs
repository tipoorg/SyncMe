namespace SyncMe.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    async void OnButtonClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://aka.ms/xamarin-quickstart");
    }
}
