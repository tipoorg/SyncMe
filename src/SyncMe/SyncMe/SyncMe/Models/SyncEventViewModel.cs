using System.ComponentModel;
using System.Windows.Input;

namespace SyncMe.Models;

public class SyncEventViewModel : INotifyPropertyChanged
{
    public SyncEvent SyncEvent { get; init; }
    public SyncEventViewModel()
    {
        SyncEvent = new SyncEvent
        {
            Namespace = new Namespace(),
            Schedule = new SyncSchedule(),
            Alert = new SyncAlert {  Reminders = new SyncReminder[1] }
        };
    }

    private bool _isAddEvenEnabled;
    public bool IsAddEvenEnabled
    {
        get { return _isAddEvenEnabled; }
        set
        {
            _isAddEvenEnabled = value;
            OnPropertyChanged(nameof(IsAddEvenEnabled));
        }
    }

    private string _scheduleButtonText = "Does Not Repeat";
    public string ScheduleButtonText
    {
        get { return _scheduleButtonText; }
        set
        {
            _scheduleButtonText = value;
            OnPropertyChanged(nameof(ScheduleButtonText));
        }
    }

    private string _alertButtonText = "At Event Time";
    public string AlertButtonText
    {
        get { return _alertButtonText; }
        set
        {
            _alertButtonText = value;
            OnPropertyChanged(nameof(AlertButtonText));
        }
    }

    public Guid Id { get; init; }
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

    public TimeSpan StartTime
    {
        get { return SyncEvent.Start.TimeOfDay; }
        set
        {
            if (SyncEvent.Start.TimeOfDay != value)
            {
                SyncEvent.Start = SyncEvent.Start.Date + value;
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

    public TimeSpan EndTime
    {
        get { return SyncEvent.End.TimeOfDay; }
        set
        {
            if (SyncEvent.End.TimeOfDay != value)
            {
                SyncEvent.End = SyncEvent.End.Date + value;
                OnPropertyChanged(nameof(EndTime));
            }
        }
    }

    public SyncReminder Notification
    {
        get { return SyncEvent.Alert.Reminders.First(); }
        set
        {
            if (SyncEvent.Alert.Reminders.First() != value)
            {
                SyncEvent.Alert.Reminders[0] = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    public SyncRepeat Schedule
    {
        get { return SyncEvent.Schedule.Repeat; }
        set
        {
            if (SyncEvent.Schedule.Repeat != value)
            {
                SyncEvent.Schedule.Repeat = value;
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
        get { return SyncEvent.Namespace.Title; }
        set
        {
            if (SyncEvent.Namespace.Title != value)
            {
                SyncEvent.Namespace.Title = value;
                OnPropertyChanged(nameof(Namespace));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
