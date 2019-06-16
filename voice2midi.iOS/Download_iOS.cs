using System;
using System.IO;
using Foundation;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
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
                Console.WriteLine(">>IOS<<");
                string fileName = (new NSUrl(file.Url, false)).LastPathComponent;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            });

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public void DownloadUrl(string url)
        {
            throw new NotImplementedException();
        }
    }
}
