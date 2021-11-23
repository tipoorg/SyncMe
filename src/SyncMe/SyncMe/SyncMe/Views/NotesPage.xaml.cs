using System.IO;
using System.Reactive.Linq;
using SyncMe.Models;

namespace SyncMe.Views;

public partial class NotesPage : ContentPage
{
    private readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");

    public NotesPage()
    {
        InitializeComponent();

        // Read the file.
        if (File.Exists(_fileName))
        {
            editor.Text = File.ReadAllText(_fileName);
        }

        ScheduledEvents = Observable
            .FromEventPattern(SetAlarmButton, nameof(Button.Clicked))
            .Select(x => new Schedule(Repeat.Every10Seconds, int.TryParse(editor.Text, out var times) ? times : 1))
            .Select(x => new SyncEvent(1, "My First Event", "", default, x, default, Status.Active));
    }

    public IObservable<SyncEvent> ScheduledEvents { get; }

    void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Save the file.
        File.WriteAllText(_fileName, editor.Text);
    }

    void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Delete the file.
        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }
        editor.Text = string.Empty;
    }
}
