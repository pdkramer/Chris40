using Chris40.Models;
using DevExpress.Mvvm.UI;
using DevExpress.UI.Xaml.Controls.Internal.Pdf;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Chris40
{
    /// <summary>
    /// Let's rock!
    /// </summary>
    public sealed partial class PlayPage : Page
    {
        IEnumerable<MusicInfo> MusicList
        {
            get
            {
                return App.SheetMusic;
            }
        }

        public PlayPage()
        {
            this.InitializeComponent();
            Loaded += PlayPage_Loaded;
        }

        private void PlayPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.SheetMusic.Count == 0) this.Frame.Navigate(typeof(SelectMusic)); //can't play anything until source
        }

        private void PDFMusicViewer_Loaded(object sender, RoutedEventArgs e)
        {
            //We're not supporting bookmarks yet, so hide the button from the UI
            var overlay = LayoutTreeHelper.GetVisualChildren(sender as FrameworkElement).Where(c => c is PdfViewerOverlayControl).FirstOrDefault();
            var bookmarkContent = PdfViewerControlLocalizer.GetString(DevExpress.Core.Localization.PdfViewerStringID.BookmarkButtonText);
            var toggle = LayoutTreeHelper.GetVisualChildren(overlay).Where(x => x is ToggleButton && (((ToggleButton)x).Content as string) == bookmarkContent).FirstOrDefault() as ToggleButton;
            if (toggle != null) toggle.Visibility = Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void MusicGrid_SelectedItemChanged(object sender, DevExpress.UI.Xaml.Grid.SelectedItemChangedEventArgs e)
        {
            MusicInfo newMusic = (MusicInfo)e.NewItem;
            PDFMusicViewer.DocumentSource = newMusic.FileName;
            PDFMusicViewer.Focus(FocusState.Programmatic);
        }
    }
}
