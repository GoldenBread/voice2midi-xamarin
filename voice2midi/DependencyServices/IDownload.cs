using System;
using voice2midi.Models;

namespace voice2midi.DependencyServices
{
    public interface IDownload
    {
        string GetDefaultPath();

        void DownloadUrl(FileUrlStruct fileUrl);
    }
}
