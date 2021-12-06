using Android.Media;
using Microsoft.Extensions.Logging;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal sealed class AndroidAlarmPlayer : IAlarmPlayer
{
    private readonly MediaPlayer _mediaPlayer;
    private readonly ILogger<AndroidAlarmPlayer> _logger;

    public AndroidAlarmPlayer(ILogger<AndroidAlarmPlayer> logger)
    {
        _mediaPlayer = new MediaPlayer();
        _logger = logger;
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
            _logger.LogInformation("Alarm played");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
