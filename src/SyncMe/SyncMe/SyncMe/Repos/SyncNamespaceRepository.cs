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
        _existingNamespaces.Add("Work", CreateNamespace("Work"));
        _existingNamespaces.Add("Work.Team1", CreateNamespace("Team1"));
        _existingNamespaces.Add("Work.Team2", CreateNamespace("Team2"));
        _existingNamespaces.Add("Russia", CreateNamespace("Russia", false));
        _existingNamespaces.Add("Russia.Holidays", CreateNamespace("Holidays", false));
        _existingNamespaces.Add("Personal", CreateNamespace("Another", false));
        _existingNamespaces.Add("Personal.Birthday", CreateNamespace("Another", false));
        _existingNamespaces.Add("Outlook", CreateNamespace("Outlook"));
    }

    private void Increment(ref int counter) => Interlocked.Increment(ref counter);

    private Namespace CreateNamespace(string title, bool isActive = true, DateTime? turnOnDate = null)
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
