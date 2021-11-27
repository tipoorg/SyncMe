using LiteDB;

namespace SyncMe.DataAccess.Repos;

internal class SoundSwitcherRepository : ISoundSwitcherRepository
{
    private const string _collectionKey = "SoundStateCollection";

    private readonly ILiteCollection<BsonDocument> _soundState;

    public SoundSwitcherRepository(ILiteDatabase database)
    {
        _soundState = database.GetCollection(_collectionKey);
    }

    public void Mute()
    {
        _soundState.Insert(new BsonDocument());
    }

    public void SetSound()
    {
        _soundState.DeleteAll();
    }

    public bool GetIsMuteState()
    {
        return _soundState.Count() > 0;
    }
}
