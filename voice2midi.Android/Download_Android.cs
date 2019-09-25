using System;
using System.IO;
using System.Linq;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Plugin.Toast;
using voice2midi.DependencyServices;
using voice2midi.Droid;
using voice2midi.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(Download_Android))]
namespace voice2midi.Droid
{
    public class Download_Android: IDownload
    {
/*
        public string Define_Default_Path()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file => {
                string fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                string fullPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
                return fullPath;
            });

            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }*/

        public string GetDefaultPath()
        {
            return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
        }

        private void SetFilenameExtension(string fileExtension)
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file => {
                string fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                string fullPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName + fileExtension);
                return fullPath;
            });
        }

        public void DownloadUrl(FileUrlStruct fileUrl)
        {
            SetFilenameExtension(fileUrl.FileExtension);
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
                            CrossToastPopUp.Current.ShowToastWarning("Download canceled");
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
