using System;
using System.IO;
using Foundation;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Plugin.Toast;
using voice2midi.DependencyServices;
using voice2midi.iOS;
using voice2midi.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Download_iOS))]
namespace voice2midi.iOS
{
    public class Download_iOS: IDownload
    {
        public string GetDefaultPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void SetFileExtension(string fileExtension)
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file => {
                string fileName = (new NSUrl(file.Url, false)).LastPathComponent;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName + fileExtension);
            });
        }


        public void DownloadUrl(FileUrlStruct fileUrl)
        {
            SetFileExtension(fileUrl.FileExtension);
            var downloadManager = CrossDownloadManager.Current;

            var file = downloadManager.CreateDownloadFile(fileUrl.Url);
            file.PropertyChanged += (sender, _e1) =>
            {
                DownloadFileImplementation dfi = (DownloadFileImplementation)sender;

                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (dfi.Status)
                    {
                        case DownloadFileStatus.RUNNING:
                            CrossToastPopUp.Current.ShowToastSuccess($"Download running... {Math.Floor(dfi.TotalBytesWritten * 100 / dfi.TotalBytesExpected)}%");
                            break;
                        case DownloadFileStatus.COMPLETED:
                            CrossToastPopUp.Current.ShowToastSuccess("Download completed");
                            break;
                        case DownloadFileStatus.CANCELED:
                            CrossToastPopUp.Current.ShowToastSuccess("Download canceled");
                            break;
                        case DownloadFileStatus.FAILED:
                            CrossToastPopUp.Current.ShowToastSuccess("Download failed");
                            break;
                    }
                });


            };
            downloadManager.Start(file);
        }
    }
}
