using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace SyncMe;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Task.Run(async () =>
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status is not PermissionStatus.Granted)
                {
                    var accepted = await DisplayAlert(
                        "Storage Permission Required",
                        "Please enable your storage permission, it will be used to store logs and crashes",
                        "ACCEPT",
                        "CANCEL");

                    if (accepted)
                    {
                        var results = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "CANCEL");
            }
        });
    }
}
