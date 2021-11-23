using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;
public record Identity(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentity;

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage()
    {
        InitializeComponent();
        BindingContext = this;

        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x =>
            {
                SwitchLayouts();
            });

        _addOutlookIdentity = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .Select(x => x) // open browser and await task
            .Subscribe(x =>
            {
                SwitchLayouts();
                Identities.Add(new Identity("My Identitiy"));
            });
    }

    private void SwitchLayouts()
    {
        AddIdentity.IsVisible = !AddIdentity.IsVisible;
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentity.Dispose();
    }
}
