using System.Linq;
using EventKit;
using Foundation;
using GlobalToast;
using SyncMe.IOS.Extensions;
using SyncMe.Models;
using UserNotifications;
using SyncMe.Lib.Extensions;
using System;
using SyncMe.Extensions;
using Microsoft.Extensions.Logging;

namespace SyncMe.IOS;

internal class IOSAlarmService : IAlarmService
{
    private readonly EKEventStore _eventStore;
    private readonly IConfigRepository _configRepository;
    private readonly ILogger<IOSAlarmService> _logger;

    public IOSAlarmService(EKEventStore eventStore, IConfigRepository configRepository, ILogger<IOSAlarmService> logger)
    {
        _eventStore = eventStore;
        _configRepository = configRepository;
        _logger = logger;
    }

    public void SetAlarmForEvent(SyncEvent syncEvent)
    {
        if (syncEvent is null)
        {
            _logger.LogWarning($"{nameof(SetAlarmForEvent)} received null event.");
            return;
        }

        CreateCalendarEvent(syncEvent);

        string text = $"{syncEvent.Title} Scheduled on {syncEvent.Start}";
        _logger.LogInformation(text);
        Toast.MakeToast(text).Show();
    }

    private void CreateCalendarEvent(SyncEvent syncEvent)
    {
        var newEvent = EKEvent.FromStore(_eventStore);

        newEvent.StartDate = syncEvent.Start.DateTimeToNSDate();
        newEvent.EndDate = syncEvent.End.DateTimeToNSDate();
        newEvent.Title = syncEvent.Title;
        newEvent.Calendar = SetSyncMeCalendar(syncEvent);
        SetEventAlarm(syncEvent, newEvent);
        SetEventRecurrenceRule(syncEvent, newEvent);

        if (!_eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out NSError e))
        {
            _logger.LogError($"Could not save event: {e.Description}");
            Toast.MakeToast($"Could not save calendar event: {e.Description}").Show();
        }
    }

    private void SetEventAlarm(SyncEvent syncEvent, EKEvent newEvent)
    {
        if (syncEvent.TryGetNearestAlarmTime(out DateTime alarmTime))
        {
            newEvent.AddAlarm(EKAlarm.FromDate(alarmTime.DateTimeToNSDate()));
            SendNotificationRequest(syncEvent, alarmTime);
        }
    }

    private void SetEventRecurrenceRule(SyncEvent syncEvent, EKEvent newEvent)
    {
        var end = EKRecurrenceEnd.FromEndDate(syncEvent.End.DateTimeToNSDate());
        var rule = syncEvent.Repeat switch
        {
            SyncRepeat.Dayly => BuildRecurrenceRule(EKRecurrenceFrequency.Daily, 1, end),
            SyncRepeat.EveryWeek => BuildRecurrenceRule(EKRecurrenceFrequency.Weekly, 1, end),
            SyncRepeat.EveryMonth => BuildRecurrenceRule(EKRecurrenceFrequency.Monthly, 1, end),
            SyncRepeat.EveryYear => BuildRecurrenceRule(EKRecurrenceFrequency.Yearly, 1, end),
            SyncRepeat.None => null,
            SyncRepeat.WorkDays => null, //to be implemented
            SyncRepeat.EveryMinute => null, //To be implemented
            _ => throw new NotImplementedException()
        };

        if (rule is not null)
            newEvent.AddRecurrenceRule(rule);
    }

    private EKRecurrenceRule BuildRecurrenceRule(EKRecurrenceFrequency frequency, nint interval, EKRecurrenceEnd end)
        => new(frequency, interval, end);

    private EKCalendar SetSyncMeCalendar(SyncEvent syncEvent)
    {
        var parentNamespace = syncEvent.GetParentNamespace();
        var calendar = _eventStore.Calendars.FirstOrDefault(x => x.Title.Equals(parentNamespace));
        if (calendar != null)
        {
            _logger.LogInformation($"Found existing calendar for event: {syncEvent.Title}; Event namespace: {syncEvent.NamespaceKey}; Calendar: {calendar.Title}");
            return calendar;
        }

        _logger.LogInformation($"Creating new calendar: {parentNamespace}");
        var newCalendar = EKCalendar.FromEventStore(_eventStore);
        newCalendar.Title = parentNamespace;
        var existingSources = _eventStore.Sources.Select(x => new { x.Title, x.SourceType, x.SourceIdentifier }).ToList();
        foreach (var source in existingSources)
        {
            _logger.LogInformation($"Source: {source.Title}; Type: {source.SourceType}; Id: {source.SourceIdentifier}");
        }
        newCalendar.Source = _eventStore.Sources.FirstOrDefault(x => x.SourceType == EKSourceType.CalDav);
        try
        {
            var result = _eventStore.SaveCalendar(newCalendar, true, out NSError error);
            if (error is not null)
                _logger.LogError($"Could not save calendar: {error.Description}");
            return newCalendar;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception while saving calendar: {ex.Message}");
            throw;
        }
    }

    private void SendNotificationRequest(SyncEvent syncEvent, DateTime alarmTime)
    {
        var sound = !_configRepository.Get(ConfigKey.IsMute) ? UNNotificationSound.GetDefaultCriticalSound(100) : null;

        if (sound == null)
            _logger.LogInformation("Sound is off");
        else
            _logger.LogInformation("Sound is on");

        var content = new UNMutableNotificationContent
        {
            Title = syncEvent.Title,
            Body = "Upcoming event",
            Badge = 1,
            Subtitle = syncEvent.NamespaceKey,
            Sound = sound// GetCriticalSound("a.aiff")
        };

        var dateComponents = new NSDateComponents()
        {
            Second = alarmTime.Second,
            Minute = alarmTime.Minute,
            Hour = alarmTime.Hour,
            Day = alarmTime.Day,
            Month = alarmTime.Month,
            Year = alarmTime.Year,
            TimeZone = NSTimeZone.SystemTimeZone,
        };
        var trigger = UNCalendarNotificationTrigger.CreateTrigger(dateComponents, false);
        var request = UNNotificationRequest.FromIdentifier(syncEvent.Id.ToString(), content, trigger);
        UNUserNotificationCenter.Current.AddNotificationRequest(request, error =>
        {
            if (error != null)
            {
                _logger.LogError($"Could not send notification {error.Description}");
                Toast.MakeToast(error.Description).Show();
            }
        });
    }
}