namespace SyncMe.Services;

public interface ISyncNamespaceService
{
    public IReadOnlyCollection<(string FullName, bool IsActive, bool HasChildren)> GetAll(string namespaceName);

    public bool UpdateStatusWithChildrens(string FullName, bool IsActive, DateTime turnOnDate = default);

    public bool HasChildren(string fullName);
    public void Add(string fullName);
    public bool ParentIsSuspended(string fullname);

    public IReadOnlyCollection<(string FullName, bool IsActive, bool HasChilde)> GetFirstLevel();
    public IReadOnlyCollection<(string FullName, bool IsActive, bool HasChildren)> GetFirstChildren(string fullName);

    public void RemoveWithChildren(string fullName);
}

