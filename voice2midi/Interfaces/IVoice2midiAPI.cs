using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace IVoice2midi.Interfaces
{
    public interface IVoice2midiAPI
    {
        [Multipart]
        [Post("/upload_generate")]
        Task<HttpResponseMessage> UploadSound([AliasAs("file")] StreamPart stream);
    }
}
