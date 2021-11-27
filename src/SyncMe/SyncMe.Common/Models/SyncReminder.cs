using System.ComponentModel;

namespace SyncMe.Models;

public enum SyncReminder
{
    [Description("At event time")]
    AtEventTime = 0,
    [Description("1 minutes before")]
    Before1Min = 1,
    [Description("5 minutes before")]
    Before5Min = 5,
    [Description("10 minutes before")]
    Before10Min = 10,
    [Description("15 minutes before")]
    Before15Min = 15,
    [Description("30 minutes before")]
    Before30Min = 30,
    [Description("1 hour before")]
    Before1Hour = 60,
    [Description("2 hours before")]
    Before2Hour = 120,
    [Description("1 day before")]
    DayBefore = 1440,
    [Description("2 days before")]
    TwoDaysBefore = 2880,
    [Description("1 week before")]
    OneWeekBefore = 10080
}