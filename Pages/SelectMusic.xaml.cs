using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chris40
{
    /// <summary>
    /// Select what music will be used (removable storage or local storage) and provide copy/delete to local storage
    /// </summary>
    public sealed partial class SelectMusic : Page
    {
        StorageFolder ourFolder = ApplicationData.Current.LocalFolder;

        public SelectMusic()
        {
            this.InitializeComponent();
        }

        private async void btnSelectMusicFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StatusList.Items.Clear();
                StorageFolder removableStorage = KnownFolders.RemovableDevices;
                IReadOnlyList<StorageFolder> folderList = await removableStorage.GetFoldersAsync();
                StorageFolder pdfFolder = null;

                if (chkDeleteLocal.IsChecked ?? false)
                {
                    StatusList.Items.Add("Clearing local files...");

                    var localFiles = await ourFolder.GetFilesAsync();

                    foreach (var localFile in localFiles)
                    {
                        await localFile.DeleteAsync(StorageDeleteOption.Default);
                    }

                }

                foreach (var folder in folderList)
                {
                    if (pdfFolder != null) break;  //we have found a directory of PDFs

                    StatusList.Items.Add("Folder: " + folder.Name);
                    IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                    foreach (StorageFile file in files)
                    {
                        if (file.FileType == ".pdf")  //we have a PDF!
                        {
                            if (pdfFolder == null) pdfFolder = folder;

                            if (chkCopyToLocal.IsChecked ?? false)
                            {
                                await file.CopyAsync(ourFolder, file.Name, NameCollisionOption.ReplaceExisting);
                            }
                            else break;  //no need to go through them all here
                        }
                    }
                }

                App.SheetMusic.Clear();
                if (pdfFolder != null) await App.LoadSheetMusic(pdfFolder);
                int musicCount = App.SheetMusic.Count;

                if (musicCount > 0)
                    StatusList.Items.Add($"{musicCount} remote storage files ready.");
                else
                    StatusList.Items.Add("No music available!");
            }
            catch (Exception ex)
            {
                StatusList.Items.Add(ex.Message);
            }
        }

        private async void btnUseLocalStorage_Click(object sender, RoutedEventArgs e)
        {
            StatusList.Items.Clear();

            try
            {
                App.SheetMusic.Clear();
                await App.LoadSheetMusic(ourFolder);
                int musicCount = App.SheetMusic.Count;

                if (musicCount > 0)
                    StatusList.Items.Add($"{musicCount} local storage files ready.");
                else
                    StatusList.Items.Add("No music available!");
            }
            catch (Exception ex)
            {
                StatusList.Items.Add(ex.Message);
            }
        }
    }
}
