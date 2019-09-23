using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using voice2midi.Models;

namespace IVoice2midi.Interfaces
{
    public interface IVoice2midiAPI
    {
        [Multipart]
        [Post("/upload")]
        Task<List<FileModelShort>> UploadGenerate([AliasAs("file")] StreamPart stream);

        [Get("/files/list")]
        Task<List<List<FileModelShort>>> SoundList();
    }
}
