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
        [Post("/api/files/upload")]
        Task<FileUploadModel> Upload([AliasAs("file")] StreamPart stream);

        [Get("/api/files/list")]
        Task<List<List<FileModelShort>>> SoundList();

        [Get("/api/files/{id}/list")]
        Task<List<FileModelShort>> SoundListSameSource(long id);

        [Get("/api/transcriber/generate/{format}/{id}")]// format: "midi" ou "mp3"
        Task<FileGenerationModel> Generate(string format, long id);
    }
}
