using System;
using System.IO;
using System.Linq;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
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

        public void Define_Path()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file => {
                Console.WriteLine(">>ANDROID<<");
                string fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                string fullPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
                Console.WriteLine(fullPath);
                return fullPath;
            });
        }
    }
}
