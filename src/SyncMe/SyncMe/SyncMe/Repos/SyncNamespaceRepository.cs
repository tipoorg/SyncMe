using System.Threading;
using SyncMe.Models;

namespace SyncMe.Repos;

public class SyncNamespaceRepository : ISyncNamespaceRepository
{
    private static int _idCounter = 0;
    private readonly Dictionary<string, Namespace> _existingNamespaces = new();

    public SyncNamespaceRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        _existingNamespaces.Add("Test", CreateNamespace("Test"));
        _existingNamespaces.Add("Another", CreateNamespace("Another"));
        _existingNamespaces.Add("Work1", CreateNamespace("Work1"));
        _existingNamespaces.Add("Work1.Team1", CreateNamespace("Another"));
        _existingNamespaces.Add("Work1.Team1.Project1", CreateNamespace("Another"));
        _existingNamespaces.Add("Work1.Team1.Project2", CreateNamespace("Another"));
        _existingNamespaces.Add("Work1.Team2", CreateNamespace("Another"));
        _existingNamespaces.Add("Work2", CreateNamespace("Another", false, DateTime.Now.AddDays(1)));
        _existingNamespaces.Add("Work2.Team1", CreateNamespace("Another", false, DateTime.Now.AddDays(1)));
        _existingNamespaces.Add("Work2.Team2", CreateNamespace("Another", false, DateTime.Now.AddDays(1)));
    }

    private void Increment(ref int counter) => Interlocked.Increment(ref counter);

    private Namespace CreateNamespace(string title, bool isActive = true, DateTime turnOnDate = new())
    {
        Increment(ref _idCounter);
        return new Namespace 
        { 
            Title = title, 
            IsActive = isActive, 
            TurnOnDate = turnOnDate 
        };
    }

    public Dictionary<string, Namespace> GetAllSyncNamespaces() =>
        _existingNamespaces;

    public void AddSyncNamespace(string name, bool isActive = true)
    {
        if (_existingNamespaces.ContainsKey(name))
            return;

        _existingNamespaces.Add(name, new Namespace { Title = name, IsActive = isActive });
    }

    public bool TryGetSyncNamespace(string name, out Namespace existingNamespace)
    {
        return _existingNamespaces.TryGetValue(name, out existingNamespace);
    }
}
