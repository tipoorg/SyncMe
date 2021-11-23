using System.IO;
using System.Reactive.Linq;
using SyncMe.Providers.OutlookProvider;

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

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Save the file.
        try
        {
            await (Application.Current as App).SignIn();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Authentication Error", ex.Message, "OK");
        }

        var events = await new OutlookProvider(App.GraphClient, "alexandr_pobezhimov@epam.com").GetEventsAsync();

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
