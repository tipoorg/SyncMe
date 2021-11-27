namespace SyncMe;

public interface INotificationsSwitcherRepository
{
    bool State { get; set; }

    event EventHandler<bool> OnStateChanged;
}
