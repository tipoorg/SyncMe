namespace SyncMe.Lib.Services;

internal class SyncNamespaceService : ISyncNamespaceService
{
    private readonly ISyncNamespaceRepository _namespaceRepository;

    public SyncNamespaceService(ISyncNamespaceRepository namespaceRepository)
    {
        _namespaceRepository = namespaceRepository;
    }

    public void Add(string fullName)
    {
        _namespaceRepository.AddSyncNamespace(fullName, !ParentIsSuspended(fullName));
    }

    public IReadOnlyCollection<(string FullName, bool IsActive, bool HasChildren)> GetAll(string namespaceName)
    {
        return _namespaceRepository.GetAllSyncNamespaces().Select(s => (s.Key, s.Value.IsActive, HasChildren(s.Key))).ToList();
    }

    public IReadOnlyCollection<(string FullName, bool IsActive, bool HasChildren)> GetFirstChildren(string fullName)
    {
        return _namespaceRepository
            .GetAllSyncNamespaces()
            .Where(n => n.Key.Count(s => s == '.') == fullName.Count(s => s == '.') + 1 && n.Key.StartsWith($"{fullName}."))
            .Select(s => (s.Key, s.Value.IsActive, HasChildren(s.Key)))
            .ToList();
    }

    public IReadOnlyCollection<(string FullName, bool IsActive, bool HasChilde)> GetFirstLevel()
    {
        return _namespaceRepository
            .GetAllSyncNamespaces()
            .Where(s => !s.Key.Contains('.'))
            .Select(s => (s.Key, s.Value.IsActive, HasChildren(s.Key)))
            .ToList();
    }

    public bool HasChildren(string fullName)
    {
        return _namespaceRepository.GetAllSyncNamespaces().Any(n => n.Key.Contains($"{fullName}."));
    }

    public bool UpdateStatusWithChildrens(string fullName, bool isActive, DateTime? turnOnDate = default)
    {
        if (ParentIsSuspended(fullName) && isActive) return false;

        _namespaceRepository
            .GetAllSyncNamespaces()
            .Where(s => s.Key.Contains($"{fullName}"))
            .ToList()
            .ForEach(a =>
            {
                a.Value.IsActive = isActive;
                a.Value.TurnOnDate = turnOnDate;
            });

        return true;
    }

    public bool ParentIsSuspended(string fullname)
    {
        var lastDotIndex = fullname.LastIndexOf('.');

        if (lastDotIndex == -1) return false;

        var parentKey = fullname.Substring(0, lastDotIndex);
        return !_namespaceRepository.GetAllSyncNamespaces()[parentKey].IsActive;
    }

    public void RemoveWithChildren(string fullName)
    {
        var namespaces = _namespaceRepository.GetAllSyncNamespaces();
        while (namespaces.Any(s => s.Key.Contains(fullName)))
        {
            namespaces.Remove(namespaces.First(s => s.Key.Contains(fullName)).Key);
        }
    }

    public bool IsNamespaceActive(string fullName)
    {
        if (_namespaceRepository.TryGetSyncNamespace(fullName, out var syncNamespace))
        {
            return syncNamespace.IsActive || DateTime.Now > syncNamespace.TurnOnDate;
        }

        return true;
    }
}

