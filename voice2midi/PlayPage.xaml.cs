using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using voice2midi.Models;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class PlayPage : ContentPage
    {
        public PlayPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var soundLinks = (SoundLinkList)BindingContext;

            var audioStream = Get_Stream(soundLinks.mp3Link);

            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(audioStream);
            player.Play();
        }

        protected Stream Get_Stream(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
