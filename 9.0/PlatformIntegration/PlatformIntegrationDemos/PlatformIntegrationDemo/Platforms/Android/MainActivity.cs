using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Microsoft.Maui.Storage;

namespace PlatformIntegrationDemo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
[IntentFilter(
    new[] { Intent.ActionSend },
    Categories = new[] { Intent.CategoryDefault },
    DataMimeType = "text/plain"
)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Intent?.Action == Intent.ActionSend)
        {
            var uri = (Android.Net.Uri)Intent.GetParcelableExtra(Intent.ExtraStream);
            if (uri != null)
            {
                var path = GetFilePathFromUri(uri);
                // Store path for use in MAUI UI
                Preferences.Set("SharedFilePath", path);
            }
        }
    }

    private string GetFilePathFromUri(Android.Net.Uri uri)
    {
        if (uri == null) return null;
        using var input = ContentResolver.OpenInputStream(uri);
        var tempFile = System.IO.Path.Combine(FileSystem.CacheDirectory, "shared.txt");
        using var output = System.IO.File.Create(tempFile);
        input.CopyTo(output);
        return tempFile;
    }

    protected override void OnResume()
    {
        base.OnResume();

        Microsoft.Maui.ApplicationModel.Platform.OnResume(this);
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);

        Microsoft.Maui.ApplicationModel.Platform.OnNewIntent(intent);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Microsoft.Maui.ApplicationModel.Platform.ActivityStateChanged -= Platform_ActivityStateChanged;
    }

    void Platform_ActivityStateChanged(object sender, Microsoft.Maui.ApplicationModel.ActivityStateChangedEventArgs e) =>
        Toast.MakeText(this, e.State.ToString(), ToastLength.Short).Show();

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Microsoft.Maui.ApplicationModel.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "xamarinessentials")]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
{
}
