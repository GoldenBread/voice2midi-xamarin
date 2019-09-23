using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace voice2midi.Models
{
    public class FileModelShort
    {
/*        [JsonProperty(PropertyName = "sound_id")]
        public string soundId { get; set; }

        [JsonProperty(PropertyName = "original_wav_link")]
        public string originalWavLink { get; set; }

        [JsonProperty(PropertyName = "mp3_link")]
        public string mp3Link { get; set; }

        [JsonProperty(PropertyName = "midi_link")]
        public string midiLink { get; set; }
*/
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "fileExtension")]
        public string FileExtension { get; set; }

        [JsonProperty(PropertyName = "sourceId")]
        public string SourceId { get; set; }
    }
}
