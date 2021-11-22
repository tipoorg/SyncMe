namespace SyncMe.Services;

public interface INotificationManager<TContext>
{
    void Show(string title, string message, TContext context);
    void Cancel(int notificationId);
}
