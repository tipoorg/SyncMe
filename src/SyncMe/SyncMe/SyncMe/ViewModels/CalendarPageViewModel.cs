using System.ComponentModel;
using System.Text;
using System.Windows.Input;
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

        DayTappedCommand = new Command<DateTime>((date) => DayTappedEvent.Invoke(date, this));
        DayTappedEvent += DayTappedd;
        _syncEventsRepository.OnSyncEventsUpdate += OnSyncEventsUpdate;
        
        NotificationsSwitcher = _notificationsSwitcherRepository.State;
        Events = GetEventsFromRepository();
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
        var message = string.Empty;
        if( _events.TryGetValue(date, out System.Collections.ICollection values))
        {
            var builder = new StringBuilder();
            builder.Append("My events for this date: ");
            builder.Append($"{Environment.NewLine}");
            foreach (var value in values)
            {
                var syncedEvent = value as SyncEventViewModel;
                if (syncedEvent != null)
                {
                    builder.Append(syncedEvent.Name);
                    builder.Append($"{Environment.NewLine}");
                }
            }

            message = builder.ToString();
        }
        else
        {
            message = $"Received tap event from date: {date}";
        }

        await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");
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

    private void OnSyncEventsUpdate(object sender, EventArgs e)
    {
        Events = GetEventsFromRepository();
    }
}
