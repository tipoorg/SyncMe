using System;
using Foundation;

namespace SyncMe.IOS.Extensions;

public static class NSDataExtensions
{
    public static NSDate DateTimeToNSDate(this DateTime date)
    {
        if (date.Kind == DateTimeKind.Unspecified)
            date = DateTime.SpecifyKind(date, DateTimeKind.Local);
        return (NSDate)date;
    }

    public static DateTime NSDateToDateTime(this NSDate date)
    {
        return ((DateTime)date).ToLocalTime();
    }
}
