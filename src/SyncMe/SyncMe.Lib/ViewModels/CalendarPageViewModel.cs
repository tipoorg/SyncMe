using SyncMe.Lib.Services;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels;

public class CalendarPageViewModel : BaseViewModel
{
    private readonly ISyncEventsService _syncEventsService;

    public CalendarPageViewModel(
        ISyncEventsService syncEventsService)
    {
        _syncEventsService = syncEventsService;
    }

    public void InitEventsCollection()
    {
        Events.Clear();
        Events.AddSafe(LoadEvents(CurrentMonthYear.Month, CurrentMonthYear.Year));
    }

    private EventCollection LoadEvents(int month, int year)
    {
        var events = _syncEventsService.SearchSyncEvents(new() { StartMonth = month, StartYear = year })
            .Where(x => x.Start.Date.Month == month && x.Start.Date.Year == year)
            .ToEventCollection(k => k.Start.Date, e => new SyncEventViewModel(e));

        return events;
    }

    private DateTime _currentMonthYear = DateTime.Now;
    public DateTime CurrentMonthYear
    {
        get => _currentMonthYear;
        set
        {
            Events.AddSafe(LoadEvents(value.Month, value.Year));
            ChangeProperty(ref _currentMonthYear, value, nameof(CurrentMonthYear));
        }
    }

    public EventCollection Events { get; } = new EventCollection();
}
