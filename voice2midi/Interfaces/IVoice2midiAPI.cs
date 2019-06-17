using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using voice2midi.Models;

namespace IVoice2midi.Interfaces
{
    public interface IVoice2midiAPI
    {
        [Multipart]
        [Post("/upload_generate")]
        Task<SoundLinkList> UploadGenerate([AliasAs("file")] StreamPart stream);

        [Get("/sound_list")]
        Task<SoundLinkLists> SoundList();
    }
}
