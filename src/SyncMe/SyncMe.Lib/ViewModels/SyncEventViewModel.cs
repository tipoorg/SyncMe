using System.ComponentModel;
using SyncMe.Extensions;
using SyncMe.Models;

namespace SyncMe.ViewModels;

public class SyncEventViewModel : INotifyPropertyChanged
{
    public SyncEvent SyncEvent { get; init; }
    public SyncEventViewModel()
    {
        SyncEvent = new SyncEvent
        {
            Start = DateTime.Now,
            End = DateTime.Now.AddHours(1),
            Reminder = SyncReminder.AtEventTime
        };

        var currentHour = TimeSpan.FromHours(DateTime.Now.TimeOfDay.Hours);
        StartTime = Trim(currentHour.Add(TimeSpan.FromHours(1)));
        EndTime = Trim(currentHour.Add(TimeSpan.FromHours(2)));
        ScheduleButtonText = SyncRepeat.None.GetDescription();
        AlertButtonText = SyncReminder.AtEventTime.GetDescription();
    }

    private TimeSpan Trim(TimeSpan timeSpan) => timeSpan.Days > 0 ? TimeSpan.FromHours(23) : timeSpan;

    private string _scheduleButtonText;
    public string ScheduleButtonText
    {
        get { return _scheduleButtonText; }
        set
        {
            _scheduleButtonText = value;
            OnPropertyChanged(nameof(ScheduleButtonText));
        }
    }

    private string _alertButtonText;
    public string AlertButtonText
    {
        get { return _alertButtonText; }
        set
        {
            _alertButtonText = value;
            OnPropertyChanged(nameof(AlertButtonText));
        }
    }

    public string Name { get; init; }
    public string Description { get; init; }

    public DateTime StartDate
    {
        get { return SyncEvent.Start; }
        set
        {
            if (SyncEvent.Start != value)
            {
                SyncEvent.Start = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }

    private TimeSpan _startTime;
    public TimeSpan StartTime
    {
        get { return _startTime; }
        set
        {
            if (_startTime != value)
            {
                _startTime = value;
                SyncEvent.Start = SyncEvent.Start.Date.Add(value);
                OnPropertyChanged(nameof(StartTime));
            }
        }
    }

    public DateTime EndDate
    {
        get { return SyncEvent.End; }
        set
        {
            if (SyncEvent.End != value)
            {
                SyncEvent.End = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }

    private TimeSpan _endTime;
    public TimeSpan EndTime
    {
        get { return _endTime; }
        set
        {
            if (_endTime != value)
            {
                _endTime = value;
                SyncEvent.End = SyncEvent.End.Date.Add(value);
                OnPropertyChanged(nameof(EndTime));
            }
        }
    }

    public SyncReminder Notification
    {
        get { return SyncEvent.Reminder; }
        set
        {
            if (SyncEvent.Reminder != value)
            {
                SyncEvent.Reminder = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    public SyncRepeat Schedule
    {
        get { return SyncEvent.Repeat; }
        set
        {
            if (SyncEvent.Repeat != value)
            {
                SyncEvent.Repeat = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    public string Title
    {
        get { return SyncEvent.Title; }
        set
        {
            if (SyncEvent.Title != value)
            {
                SyncEvent.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    public string Namespace
    {
        get { return SyncEvent.NamespaceKey; }
        set
        {
            if (SyncEvent.NamespaceKey != value)
            {
                SyncEvent.NamespaceKey = value;
                OnPropertyChanged(nameof(Namespace));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
