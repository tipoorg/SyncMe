namespace SyncMe.Lib.Services;

public interface IBackgroundColorService
{
    void UseDarkTheme(params ContentPage[] pages);
    void UseWhiteTheme(params ContentPage[] pages);
}

