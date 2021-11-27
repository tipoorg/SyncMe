using System.ComponentModel;

namespace SyncMe.Models;

public enum SyncRepeat
{
    [Description("Does not repeat")]
    None,
    [Description("Daily")]
    Dayly,
    [Description("Work Days")]
    WorkDays,
    [Description("Every week")]
    EveryWeek,
    [Description("Every Month")]
    EveryMonth, 
    [Description("Every Year")]
    EveryYear,
    [Description("Every Minute")]
    EveryMinute
}
