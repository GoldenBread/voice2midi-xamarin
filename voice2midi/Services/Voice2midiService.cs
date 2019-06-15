using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using IVoice2midi.Interfaces;
using Refit;

namespace voice2midi.Services
{
    public class Voice2midiService
    {
        private readonly HttpClient _httpClient;
        private readonly IVoice2midiAPI _api;

        public Voice2midiService(string baseUrl)
        {
            _httpClient = new HttpClient();
            _api = RestService.For<IVoice2midiAPI>(baseUrl);
        }

        public async Task<HttpResponseMessage>  Upload_f(StreamPart stream)
        {
            return await _api.UploadSound(stream).ConfigureAwait(false);
        }
    }
}
