using System;
using System.Threading.Tasks;
using IVoice2midi.Interfaces;
using Newtonsoft.Json;
using Refit;
using voice2midi.Models;

namespace voice2midi.Services
{
    public class Voice2midiService
    {
        private readonly IVoice2midiAPI _api;

        public Voice2midiService(string baseUrl)
        {
            _api = RestService.For<IVoice2midiAPI>(baseUrl);
        }

        public async Task<SoundLinkList> Upload_Generate_Sound(StreamPart stream)
        {
            var task = await _api.UploadSound(stream).ConfigureAwait(false);

            Console.WriteLine(task);
            string json = await task.Content.ReadAsStringAsync();
            Console.WriteLine(json);
            SoundLinkList soundLink = JsonConvert.DeserializeObject<SoundLinkList>(json);

            return soundLink;
        }
    }
}
