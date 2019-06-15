using System;
namespace voice2midi.Models
{
    public class Sound
    {
        enum Formats { WAV, MP3, MIDI };

        public string filename { get; set; }
        public string path { get; set; }

        public Sound()
        {
        }
    }
}
