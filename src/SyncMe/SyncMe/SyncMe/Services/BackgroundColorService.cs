using SyncMe.Views;
namespace SyncMe.Services;

public class BackgroundColorService : IBackgroundColorService
{
    private readonly ContentPage _calendarPage;
    private readonly ContentPage _namespacePage;
    private readonly ContentPage _identityPage;

    private const string blackTheme = "#464547";
    private const string whiteTheme = "#FFFFFF";

    private const string darkImage = "background.png";
    private const string whiteImage = "background_white.png";

    public BackgroundColorService(CalendarPage page1, NamespaceManagmentPage page2, IdentityProvidersPage page3)
    {
        _calendarPage = page1;
        _namespacePage = page2;
        _identityPage = page3;
    }

    public void ChangeTheme(bool isWhite)
    {
        if (isWhite)
        {
            SetColor(whiteTheme);
            _identityPage.BackgroundImageSource = whiteImage;
            _namespacePage.BackgroundImageSource = whiteImage;
        }
        else
        {
            _identityPage.BackgroundImageSource = darkImage;
            _namespacePage.BackgroundImageSource = darkImage;
            SetColor(blackTheme);
        }
    }

    private void SetColor(string color)
    {
        
        byte r = (byte)Convert.ToUInt32(color.Substring(1, 2), 16);
        byte g = (byte)Convert.ToUInt32(color.Substring(3, 2), 16);

        byte b = (byte)(Convert.ToUInt32(color.Substring(5, 2), 16));

        var newColor = Color.FromRgb(r, g, b);

        _calendarPage.BackgroundColor = newColor;
        _identityPage.BackgroundColor = newColor;
        _namespacePage.BackgroundColor = newColor;

    }
}
