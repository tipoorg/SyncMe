using SyncMe.Models;

namespace SyncMe
{
    public interface INotificationManager
    {
        void Show(SyncAlarm syncAlarm);
        void Cancel(int id);
    }
}