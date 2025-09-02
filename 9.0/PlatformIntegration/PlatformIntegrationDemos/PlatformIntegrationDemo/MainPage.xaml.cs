public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var sharedFilePath = Preferences.Get("SharedFilePath", null);
        if (!string.IsNullOrEmpty(sharedFilePath) && File.Exists(sharedFilePath))
        {
            ChatDisplay.Text = File.ReadAllText(sharedFilePath);
        }
    }
}