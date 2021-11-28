using Android.Media;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal sealed class AndroidAlarmPlayer : IAlarmPlayer
{
    private readonly MediaPlayer _mediaPlayer;

    public AndroidAlarmPlayer()
    {
        _mediaPlayer = new MediaPlayer();
    }

    public void PlayAlarm()
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
