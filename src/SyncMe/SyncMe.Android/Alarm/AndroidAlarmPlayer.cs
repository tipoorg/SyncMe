using Android.Media;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal sealed class AndroidAlarmPlayer : IAlarmPlayer
{
    private readonly MediaPlayer _mediaPlayer;
    private readonly ISoundSwitcherRepository _soundSwitcherRepository;

    public AndroidAlarmPlayer(ISoundSwitcherRepository soundSwitcherRepository)
    {
        _mediaPlayer = new MediaPlayer();
        _soundSwitcherRepository = soundSwitcherRepository;
    }

    public void PlayAlarm()
    {
        if (!_soundSwitcherRepository.GetIsMuteState())
        {
            var soundUri = RingtoneManager.GetActualDefaultRingtoneUri(AndroidApp.Context, RingtoneType.Alarm);

            try
            {
                _mediaPlayer.Reset();
                _mediaPlayer.SetDataSource(AndroidApp.Context, soundUri);
                _mediaPlayer.SetAudioAttributes(GetAudio());
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public void StopPlaying()
    {
        _mediaPlayer.Stop();
    }

    private static AudioAttributes GetAudio()
    {
        return new AudioAttributes.Builder()
            .SetUsage(AudioUsageKind.Alarm)
            .Build();
    }

    private void OnStateChanged(object sender, bool newState)
    {
        if (newState is false)
            _mediaPlayer.Stop();
    }
}
