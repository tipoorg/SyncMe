using System.ComponentModel;
using SyncMe.Models;
using Xamarin.Plugin.Calendar.Models;

namespace SyncMe.ViewModels;

public class CalendarPageViewModel : INotifyPropertyChanged
{
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISoundSwitcherService _soundSwitcherService;

    public IBackgroundColorService BackgroundColorService { get; set; }

    public CalendarPageViewModel(
        ISyncEventsService syncEventsService,
        ISoundSwitcherService soundSwitcherService)
    {
        _syncEventsService = syncEventsService;
        _soundSwitcherService = soundSwitcherService;
        _syncEventsService.OnSyncEventsUpdate += OnSyncEventsUpdate;


        SoundSwitcher = !_soundSwitcherService.IsMute();
        ThemeSwitcher = true;
        Events = LoadEvents();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private EventCollection LoadEvents()
    {
        var events = _syncEventsService.GetAllSyncEvents()
            .ToLookup(k => k.Start.Date, Convert);

        var result = new EventCollection();
        foreach (var e in events)
        {
            result.Add(e.Key, e.ToList());
        }

        return result;
    }

    private static SyncEventViewModel Convert(SyncEvent e) => new() { Description = e.NamespaceKey, Name = e.Title, StartDate = e.Start };

    private EventCollection _events;

    public EventCollection Events
    {
        get => _events;
        set
        {
            if (_events != value)
            {
                _events = value;
                OnPropertyChanged("Events");
            }
            _events = value;
        }
    }

    private bool _soundSwitcher;

    public bool SoundSwitcher
    {
        get => _soundSwitcher;
        set
        {
            if (_soundSwitcher != value)
            {
                if (value is true) _soundSwitcherService.SetSound();
                else _soundSwitcherService.Mute();

                _soundSwitcher = value;
                OnPropertyChanged("SoundSwitcher");
            }
        }
    }

    private bool _themeSwitcher;

    public bool ThemeSwitcher
    {
        get => _themeSwitcher;
        set
        {
            if (_themeSwitcher != value)
            {
                _themeSwitcher = value;
                BackgroundColorService?.ChangeTheme(value);
                OnPropertyChanged(nameof(ThemeSwitcher));
            }
        }
    }

    private void OnSyncEventsUpdate(object sender, EventArgs e)
    {
        Events = LoadEvents();
    }
}
