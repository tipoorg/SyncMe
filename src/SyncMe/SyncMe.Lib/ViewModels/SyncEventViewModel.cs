using SyncMe.Extensions;
using SyncMe.Models;

namespace SyncMe.ViewModels;

public class SyncEventViewModel : BaseViewModel
{
    public SyncEventViewModel()
    {
        var currentDay = DateTime.Now;
        var currentHour = TimeSpan.FromHours(currentDay.TimeOfDay.Hours);
        StartDate = currentDay;
        EndDate = currentDay;
        StartTime = Trim(currentHour.Add(TimeSpan.FromHours(1)));
        EndTime = Trim(currentHour.Add(TimeSpan.FromHours(2)));
        ScheduleButtonText = SyncRepeat.None.GetDescription();
        AlertButtonText = SyncReminder.AtEventTime.GetDescription();
    }

    private TimeSpan Trim(TimeSpan timeSpan) => timeSpan.Days > 0 ? TimeSpan.FromHours(23) : timeSpan;

    private string _scheduleButtonText;
    public string ScheduleButtonText
    {
        get => _scheduleButtonText;
        set => ChangeProperty(ref _scheduleButtonText, value, nameof(ScheduleButtonText));
    }

    private string _alertButtonText;
    public string AlertButtonText
    {
        get => _alertButtonText;
        set => ChangeProperty(ref _alertButtonText, value, nameof(AlertButtonText));
    }

    public Guid Id { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (_startDate != value)
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }

    private TimeSpan _startTime;
    public TimeSpan StartTime
    {
        get => _startTime;
        set
        {
            if (_startTime != value)
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }
    }

    private DateTime _endDate;
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate != value)
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }

    private TimeSpan _endTime;
    public TimeSpan EndTime
    {
        get => _endTime;
        set
        {
            if (_endTime != value)
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }
    }

    private SyncReminder _notification;
    public SyncReminder Notification
    {
        get => _notification;
        set
        {
            if (_notification != value)
            {
                _notification = value;
                OnPropertyChanged(nameof(Notification));
            }
        }
    }

    private SyncRepeat _schedule;
    public SyncRepeat Schedule
    {
        get => _schedule;
        set
        {
            if (_schedule != value)
            {
                _schedule = value;
                OnPropertyChanged(nameof(Schedule));
            }
        }
    }

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    private string _namespace;
    public string Namespace
    {
        get => _namespace;
        set
        {
            if (_namespace != value)
            {
                _namespace = value;
                OnPropertyChanged(nameof(Namespace));
            }
        }
    }

    public bool IsDeleteButtonVisible
    {
        get => ExternalId is null;
    }
    public string StartTimeString
    {
        get => StartDate.Add(StartTime).ToShortTimeString();
    }

    public SyncEvent ToSyncEvent()
    {
        var start = StartDate.Date.Add(StartTime);
        var end = EndDate.Date.Add(EndTime);

        return new SyncEvent
        {
            NamespaceKey = Namespace,
            Title = Title,
            End = end,
            Start = start,
            Repeat = Schedule,
            Reminder = Notification,
            Status = SyncStatus.Active,
            Description = Description,
            ExternalId = ExternalId,
            Id = Id,
        };
    }

    public static SyncEventViewModel FromSyncEvent(SyncEvent syncEvent)
    {
        return new SyncEventViewModel
        {
            Title = syncEvent.Title,
            Namespace = syncEvent.NamespaceKey,
            StartDate = syncEvent.Start.Date,
            StartTime = syncEvent.Start.TimeOfDay,
            EndTime = syncEvent.End.TimeOfDay,
            EndDate = syncEvent.End.Date,
            Notification = syncEvent.Reminder,
            Schedule = syncEvent.Repeat,
            Description = syncEvent.NamespaceKey,
            ExternalId = syncEvent.ExternalId,
            Id = syncEvent.Id,
            Name = syncEvent.Title
        };
    }
}
