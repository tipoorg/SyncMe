using Android.Content;
using Android.Media;

namespace SyncMe.Droid.Alarm;

internal sealed class AndroidAlarmPlayer
{
    private readonly MediaPlayer _mediaPlayer;

    private static AndroidAlarmPlayer _instance;
    public static AndroidAlarmPlayer Instance => _instance ??= new AndroidAlarmPlayer();

    private AndroidAlarmPlayer()
    {
        _mediaPlayer = new MediaPlayer();
    }

    public void PlayAlarm(Context context)
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
}
