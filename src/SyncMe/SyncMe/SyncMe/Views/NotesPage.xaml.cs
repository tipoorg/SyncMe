using System.IO;
using System.Reactive.Linq;

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

        SetAlarmClicks = Observable
            .FromEventPattern(SetAlarmButton, nameof(Button.Clicked))
            .Select(x => int.TryParse(editor.Text, out var times) ? times : 1);
    }

    public IObservable<int> SetAlarmClicks { get; }

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
