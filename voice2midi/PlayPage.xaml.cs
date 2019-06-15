using System;
using System.IO;
using System.Net;
using Plugin.AudioRecorder;
using Plugin.SimpleAudioPlayer;
using voice2midi.Models;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class PlayPage : ContentPage
    {
        ISimpleAudioPlayer _player;

        public PlayPage(SoundLinkList soundLinks)
        {
            InitializeComponent();
            _player = CrossSimpleAudioPlayer.Current;

            var audioStream = Get_Stream(soundLinks.mp3Link);
            _player.Load(audioStream);
        }

        void Handle_Clicked(object sender, EventArgs e)
        {
            _player.Play();
        }

        protected Stream Get_Stream(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
