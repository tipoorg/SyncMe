using System.Collections.ObjectModel;
using SyncMe.Models;

namespace SyncMe.Repos;

public class EventRepository
{
    public static ObservableCollection<SyncEvent> Events = new ObservableCollection<SyncEvent>();
}
