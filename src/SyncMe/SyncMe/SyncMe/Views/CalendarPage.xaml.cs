using SyncMe.Models;
using SyncMe.ViewModels;
using Xamarin.Forms.Xaml;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();
            BindingContext = new CalendarPageViewModel();
            AddEvent.Clicked += AddEvent_Clicked;
        }

        public async void AddEvent_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Create:", "Cancell", null, "Event", "Namespace");

            switch (action)
            {
                case "Namespace":
                    await Navigation.PushModalAsync(new NamespaceManagmentPage());
                    break;
            }
        }

    }
}