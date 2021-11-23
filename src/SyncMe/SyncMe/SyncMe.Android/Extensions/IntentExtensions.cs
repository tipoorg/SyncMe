using Android.Content;
using Newtonsoft.Json;

namespace SyncMe.Droid.Extensions;

public static class IntentExtensions
{
    public static Intent PutExtra<TExtra>(this Intent intent, TExtra extra)
    {
        var key = nameof(TExtra);
        var json = JsonConvert.SerializeObject(extra);
        return intent.PutExtra(key, json);
    }

    public static TExtra GetExtra<TExtra>(this Intent intent)
    {
        var key = nameof(TExtra);
        var json = intent.GetStringExtra(key);
        return JsonConvert.DeserializeObject<TExtra>(json);
    }
}
