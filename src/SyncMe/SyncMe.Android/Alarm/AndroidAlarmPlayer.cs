using Android.Content;
using Android.Media;

namespace SyncMe.Droid.Alarm;

internal sealed class AndroidAlarmPlayer : IAndroidAlarmPlayer
{
    private readonly MediaPlayer _mediaPlayer;
    private readonly INotificationsSwitcherRepository _notificationsSwitcherRepository;

    public AndroidAlarmPlayer(INotificationsSwitcherRepository notificationsSwitcherRepository)
    {
        _mediaPlayer = new MediaPlayer();
        _notificationsSwitcherRepository = notificationsSwitcherRepository;
        _notificationsSwitcherRepository.OnStateChanged += OnStateChanged;
    }

    public void PlayAlarm(Context context)
    {
        if (_notificationsSwitcherRepository.State)
        {
            var soundUri = RingtoneManager.GetActualDefaultRingtoneUri(context, RingtoneType.Alarm);

            try
            {
                _mediaPlayer.Reset();
                _mediaPlayer.SetDataSource(context, soundUri);
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
