namespace SyncMe.Lib.Services;

internal class BackgroundColorService : IBackgroundColorService
{
    private static Color _darkThemeColor = new Color(70, 69, 71);
    private static Color _whiteThemeColor = new Color(255, 255, 255);

    private const string _darkImage = "background.png";
    private const string _whiteImage = "background_white.png";

    public void UseWhiteTheme(params ContentPage[] pages)
    {
        SetTheme(_whiteThemeColor, _whiteImage, pages);
    }

    public void UseDarkTheme(params ContentPage[] pages)
    {
        SetTheme(_darkThemeColor, _darkImage, pages);
    }

    private void SetTheme(Color color, string image, IEnumerable<ContentPage> pages)
    {
        foreach (var page in pages)
        {
            page.BackgroundColor = color;
            page.BackgroundImageSource = image;
        }
    }
}
