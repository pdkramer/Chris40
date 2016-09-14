using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Storage;

namespace Chris40
{
    /// <summary>
    /// Main page
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string BLANKS = "                    ";
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.SheetMusic.Count == 0) await App.LoadSheetMusic(ApplicationData.Current.LocalFolder);
            PlayTile.NotificationHeader = App.SheetMusic.Count;

            var musicList = App.SheetMusic;
            if (musicList.Count > 0) Song1.Text = (musicList[0].Name + BLANKS).Substring(0, 20);
            if (musicList.Count > 1) Song2.Text = (musicList[1].Name + BLANKS).Substring(0, 20);
            if (musicList.Count > 2) Song3.Text = (musicList[2].Name + BLANKS).Substring(0, 20);
        }
    }
}
