using System;
using System.IO;
using Foundation;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Plugin.Toast;
using voice2midi.DependencyServices;
using voice2midi.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(Download_iOS))]
namespace voice2midi.iOS
{
    public class Download_iOS: IDownload
    {
        public Download_iOS()
        {
        }

        public string Define_Default_Path()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file => {
                string fileName = (new NSUrl(file.Url, false)).LastPathComponent;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            });

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public void DownloadUrl(string url)
        {
            var downloadManager = CrossDownloadManager.Current;
            var file = downloadManager.CreateDownloadFile(url);
            file.PropertyChanged += (sender, _e1) =>
            {
                DownloadFileImplementation dfi = (DownloadFileImplementation)sender;

                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (dfi.Status)
                    {
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
