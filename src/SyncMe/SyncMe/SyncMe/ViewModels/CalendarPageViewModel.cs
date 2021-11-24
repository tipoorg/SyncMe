using System.ComponentModel;
using SyncMe.Models;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels;

internal class CalendarPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    EventCollection events = new EventCollection()
    {
        [DateTime.Now] = new List<SyncEventViewModel>
        {
            new SyncEventViewModel { Description = "Happy day", Name = "Birthday"},
            new SyncEventViewModel { Description = "Very happy day", Name = "New year"}
        },
        [DateTime.Now.AddDays(5)] = new List<SyncEventViewModel>
        {
            new SyncEventViewModel { Name = "Win", Description = "Our win day" },
            new SyncEventViewModel { Name = "Winter", Description = "Very cold time day" }
        },
        // 3 days ago
        [DateTime.Now.AddDays(-4)] = new List<SyncEventViewModel>
        {
            new SyncEventViewModel { Name = "Key", Description = "Very amazing key day" }
        },
    };

    public EventCollection Events
    {
        get => events;
        set => events = value;
    }
};

