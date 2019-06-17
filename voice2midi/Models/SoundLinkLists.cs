using System;
using Newtonsoft.Json;

namespace voice2midi.Models
{
    public class SoundLinkLists
    {
        [JsonProperty(PropertyName = "sound_link_lists")]
        public SoundLinkList[] soundLinkLists { get; set; }
    }
}
