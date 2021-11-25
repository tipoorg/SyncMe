using SyncMe.Repos;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels;

public class CalendarPageViewModel : INotifyPropertyChanged
{
    private readonly INotificationsSwitcherRepository _notificationsSwitcherRepository;

    public CalendarPageViewModel(INotificationsSwitcherRepository notificationsSwitcherRepository)
    {
        DayTappedCommand = new Command<DateTime>((date) => DayTappedEvent.Invoke(date, this));
        DayTappedEvent += DayTappedd;
        _notificationsSwitcherRepository = notificationsSwitcherRepository;
        NotificationsSwitcher = _notificationsSwitcherRepository.State;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

    private async void DayTappedd(object sender, CalendarPageViewModel item)
    {
        var date = (DateTime)sender;
        var message = $"Received tap event from date: {date}";
        await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");
    }

    private bool _notificationsSwitcher;

    public bool NotificationsSwitcher
    {
        set
        {
            if (_notificationsSwitcher != value)
            {
                _notificationsSwitcherRepository.State = value;
                _notificationsSwitcher = value;
                OnPropertyChanged("NotificationsSwitcher");
            }
        }
        get
        {
            return _notificationsSwitcher;
        }
    }
}
