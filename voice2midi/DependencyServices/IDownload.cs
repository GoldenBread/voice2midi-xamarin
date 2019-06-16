using System;
namespace voice2midi.DependencyServices
{
    public interface IDownload
    {
        string Define_Default_Path();

        void DownloadUrl(string url);
    }
}
