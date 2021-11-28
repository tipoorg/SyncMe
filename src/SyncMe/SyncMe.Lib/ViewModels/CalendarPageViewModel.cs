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
        Events = LoadEvents();
    }

    private EventCollection LoadEvents()
    {
        var events = _syncEventsService.GetAllSyncEvents()
            .ToLookup(k => k.Start.Date, e => new SyncEventViewModel(e));

        var result = new EventCollection();
        foreach (var e in events)
        {
            result.Add(e.Key, e.ToList());
        }

        return result;
    }

    private EventCollection _events;
    public EventCollection Events
    {
        get => _events;
        set => ChangeProperty(ref _events, value, nameof(Events));
    }
}
