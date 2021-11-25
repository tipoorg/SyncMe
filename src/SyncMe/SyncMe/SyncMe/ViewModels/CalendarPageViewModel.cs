using System.ComponentModel;
using SyncMe.Models;
using SyncMe.Repos;
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
        _syncEventsRepository = syncEventsRepository;
        _notificationsSwitcherRepository = notificationsSwitcherRepository;
        _syncEventsRepository.OnSyncEventsUpdate += OnSyncEventsUpdate;
        
        NotificationsSwitcher = _notificationsSwitcherRepository.State;
        Events = GetEventsFromRepository();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private EventCollection GetEventsFromRepository()
    {
        var events = _syncEventsRepository.GetAllSyncEvents()
            .ToLookup(k => k.Start.Date, Convert);

        var result = new EventCollection();
        foreach (var e in events)
        {
            result.Add(e.Key, e.ToList());
        }

        return result;
    }

    private static SyncEventViewModel Convert(SyncEvent e) => new() { Description = e.Namespace.Title, Name = e.Title, StartDate = e.Start };

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

    private void OnSyncEventsUpdate(object sender, EventArgs e)
    {
        Events = GetEventsFromRepository();
    }
}
