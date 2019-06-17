using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace voice2midi.Models
{
    public class SoundLinkList
    {
        [JsonProperty(PropertyName = "sound_id")]
        public string soundId { get; set; }

        [JsonProperty(PropertyName = "original_wav_link")]
        public string originalWavLink { get; set; }

        [JsonProperty(PropertyName = "mp3_link")]
        public string mp3Link { get; set; }

        [JsonProperty(PropertyName = "midi_link")]
        public string midiLink { get; set; }
    }
}
