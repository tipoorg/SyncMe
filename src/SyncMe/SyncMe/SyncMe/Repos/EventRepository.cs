using System.Collections.ObjectModel;
using SyncMe.Models;

namespace SyncMe.Repos;

public class EventRepository
{
    public static ObservableCollection<Event> Events = new ObservableCollection<Event>();
}
