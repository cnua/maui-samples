using Microsoft.Maui.Controls;
using System.IO;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // Subscribe to file received event (platform-specific implementation required)
        FileReceiver.OnFileReceived += HandleFileReceived;
    }

    private void HandleFileReceived(string filePath)
    {
        // Read and display the file contents
        var chatText = File.ReadAllText(filePath);
        ChatDisplay.Text = chatText;
    }
}