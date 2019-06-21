using System;
using System.IO;
using System.Linq;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Plugin.Toast;
using voice2midi.DependencyServices;
using voice2midi.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Download_Android))]
namespace voice2midi.Droid
{
    public class Download_Android: IDownload
    {
        public Download_Android()
        {
        }

        public string Define_Default_Path()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file => {
                string fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                string fullPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
                return fullPath;
            });

            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
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
                            CrossToastPopUp.Current.ShowToastError("Download canceled");
                            break;
                        case DownloadFileStatus.FAILED:
                            CrossToastPopUp.Current.ShowToastError("Download failed");
                            break;
                    }
                });


            };
            downloadManager.Start(file);

        }
    }
}
