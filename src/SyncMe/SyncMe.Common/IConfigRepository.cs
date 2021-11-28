using SyncMe.Models;

namespace SyncMe;

public interface IConfigRepository
{
    bool Get(ConfigKey key);
    void Set(ConfigKey key, bool value);
}
