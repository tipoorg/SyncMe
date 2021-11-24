using SyncMe.Models;
using System.Windows.Input;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels;

internal class CalendarPageViewModel
{
    public CalendarPageViewModel()
    {
        DayTappedCommand = new Command<DateTime>((date) => DayTappedEvent.Invoke(date, this));
        DayTappedEvent += DayTappedd;
    }

    public ICommand DayTappedCommand { get; set; }

    private static event EventHandler<CalendarPageViewModel> DayTappedEvent;

    public EventCollection Events
    {
        get => events;
        set => events = value;
    }

    EventCollection events = new EventCollection()
    {
        [DateTime.Now] = new List<EventModel>
        {
            new EventModel { Description = "Happy day", Name = "Birthday"},
            new EventModel { Description = "Very happy day", Name = "New year"}
        },
        [DateTime.Now.AddDays(5)] = new List<EventModel>
        {
            new EventModel { Name = "Win", Description = "Our win day" },
            new EventModel { Name = "Winter", Description = "Very cold time day" }
        },
        [DateTime.Now.AddDays(-4)] = new List<EventModel>
        {
            new EventModel { Name = "Key", Description = "Very amazing key day" }
        },
    };

    private async void DayTappedd(object sender, CalendarPageViewModel item)
    {
        var date = (DateTime)sender;
         var message = $"Received tap event from date: {date}";
         await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");
    }

};

