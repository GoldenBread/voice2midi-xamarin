using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Plugin.AudioRecorder;
using Plugin.SimpleAudioPlayer;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Refit;
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
            if (audioStream != null)
            {
                _player.Load(audioStream);
            }
        }

        void Handle_Clicked(object sender, EventArgs e)
        {
            _player.Play();
        }

        protected Stream Get_Stream(String url)
        {
            HttpWebResponse response;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossToastPopUp.Current.ShowToastError($"Error from remote server: \n{e.Message}", ToastLength.Long);
                });
                return null;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return response.GetResponseStream();
        }
    }
}
