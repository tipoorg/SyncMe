namespace SyncMe.Lib.Repos;

internal class NotificationsSwitcherRepository : INotificationsSwitcherRepository
{
    public event EventHandler<bool> OnStateChanged;

    private bool _state = true;
    public bool State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnStateChanged?.Invoke(this, value);
            }
        }
    }
}
