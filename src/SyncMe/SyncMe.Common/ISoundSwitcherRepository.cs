namespace SyncMe;

public interface ISoundSwitcherRepository
{
    bool GetIsMuteState();
    void Mute();
    void SetSound();
}
