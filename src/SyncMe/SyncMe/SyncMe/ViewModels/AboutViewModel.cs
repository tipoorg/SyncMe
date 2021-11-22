using System.Windows.Input;
using Xamarin.Essentials;

namespace SyncMe.ViewModels;

public class AboutViewModel : BaseViewModel
{
    public AboutViewModel()
    {
        Title = "About";
        OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
    }

    public ICommand OpenWebCommand { get; }
}
