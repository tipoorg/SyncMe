using SyncMe.Models;
using SyncMe.Repos;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels;

public class CalendarPageViewModel : INotifyPropertyChanged
{
    private readonly ISyncEventsRepository _syncEventsRepository;
    private readonly INotificationsSwitcherRepository _notificationsSwitcherRepository;

    public CalendarPageViewModel(
        ISyncEventsRepository syncEventsRepository,
        INotificationsSwitcherRepository notificationsSwitcherRepository)
    {
        DayTappedCommand = new Command<DateTime>((date) => DayTappedEvent.Invoke(date, this));
        DayTappedEvent += DayTappedd;
        _syncEventsRepository = syncEventsRepository;
        _notificationsSwitcherRepository = notificationsSwitcherRepository;
        NotificationsSwitcher = _notificationsSwitcherRepository.State;
        _events = GetEventsFromRepository();
        _syncEventsRepository.OnAddSyncEvent +=OnAddSyncEvent;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ICommand DayTappedCommand { get; set; }

    private static event EventHandler<CalendarPageViewModel> DayTappedEvent;

    private async void DayTappedd(object sender, CalendarPageViewModel item)
    {
        var date = (DateTime)sender;
        var message = $"Received tap event from date: {date}";
        await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");
    }

    private EventCollection GetEventsFromRepository()
    {
        var now = DateTime.Now;
        var events = _syncEventsRepository.GetAllSyncEvents()
            .Where(x => x.Start.Year == now.Year && x.Start.Month == now.Month)
            .ToLookup(k => k.Start.Date, Convert);

        var result = new EventCollection();
        foreach (var e in events)
        {
            result.Add(e.Key, e.ToList());
        }

        return result;
    }

    private static SyncEventViewModel Convert(SyncEvent e) => new() { Description = e.Description, Name = e.Title };

    private EventCollection _events;

    public EventCollection Events
    {
        get => _events;
        set
        {
            if (_events != value)
            {
                _events = value;
                OnPropertyChanged("Events");
            }
            _events = value;
        }
    }

    private bool _notificationsSwitcher;

    public bool NotificationsSwitcher
    {
        get => _notificationsSwitcher;
        set
        {
            if (_notificationsSwitcher != value)
            {
                _notificationsSwitcherRepository.State = value;
                _notificationsSwitcher = value;
                OnPropertyChanged("NotificationsSwitcher");
            }
        }
    }

    private void OnAddSyncEvent(object sender, Guid e)
    {
        if (_syncEventsRepository.TryGetSyncEvent(e, out var syncEvent))
        {
            var key = syncEvent.Start.Date;

            if (Events.TryGetValue(key, out var dayEvents))
            {
                Events[key] = dayEvents
                    .OfType<SyncEventViewModel>()
                    .Append(Convert(syncEvent))
                    .ToList();
            }
            else
            {
                Events.Add(syncEvent.Start.Date, new List<SyncEventViewModel>
                {
                    Convert(syncEvent)
                });
            }

        }
    }
}
