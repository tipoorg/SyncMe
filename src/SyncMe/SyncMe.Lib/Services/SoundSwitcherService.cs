namespace SyncMe.Lib.Services;

internal class SoundSwitcherService : ISoundSwitcherService
{
    private readonly IAlarmPlayer _alarmPlayer;
    private readonly ISoundSwitcherRepository _soundSwitcherRepository;

    public SoundSwitcherService(
        IAlarmPlayer alarmPlayer,
        ISoundSwitcherRepository soundSwitcherRepository)
    {
        _alarmPlayer = alarmPlayer;
        _soundSwitcherRepository = soundSwitcherRepository;
    }

    public void SetSound()
    {
        _soundSwitcherRepository.SetSound();
    }

    public void Mute()
    {
        _soundSwitcherRepository.Mute();
        _alarmPlayer.StopPlaying();
    }

    public bool IsMute()
    {
        return _soundSwitcherRepository.GetIsMuteState();
    }
}
