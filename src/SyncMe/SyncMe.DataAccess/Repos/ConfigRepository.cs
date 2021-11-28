using LiteDB;
using SyncMe.Models;

namespace SyncMe.DataAccess.Repos;

internal class ConfigRepository : IConfigRepository
{
    public record BoolConfigValue(ConfigKey Key, bool Value);

    private readonly ILiteCollection<BoolConfigValue> _configs;

    public ConfigRepository(ILiteDatabase database)
    {
        _configs = database.GetCollection<BoolConfigValue>();
        BsonMapper.Global.Entity<BoolConfigValue>().Id(x => x.Key);
    }

    public void Set(ConfigKey key, bool value)
    {
        _configs.Upsert(new BoolConfigValue(key, value));
    }

    public bool Get(ConfigKey key)
    {
        var result = _configs.FindById(key.ToString());
        return result?.Value ?? false;
    }
}
