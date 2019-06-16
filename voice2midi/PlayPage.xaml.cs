using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SimpleAudioPlayer;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using voice2midi.DependencyServices;
using voice2midi.Models;
using voice2midi.SystemConf;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class PlayPage : ContentPage
    {
        ISimpleAudioPlayer _player;
        SoundLinkList _soundLinks;

        public PlayPage(SoundLinkList soundLinks)
        {
            InitializeComponent();
            _soundLinks = soundLinks;
            _player = CrossSimpleAudioPlayer.Current;

            var audioStream = Get_Stream(soundLinks.mp3Link);
            if (audioStream != null)
            {
                _player.Load(audioStream);
            }

            _ = RequestPermission.Check_Permission_Async(Permission.Storage, PermissionBtn, DownloadBtn);
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

        void Handle_Clicked_1(object sender, EventArgs e)
        {
            DependencyService.Get<IDownload>().Define_Path();

            var downloadManager = CrossDownloadManager.Current;
            var file2 = downloadManager.CreateDownloadFile(_soundLinks.mp3Link);
            downloadManager.Start(file2);
        }

        async void PermissionBtn_Clicked(object sender, EventArgs e)
        {
            await RequestPermission.Ask_Permissions_Async(Permission.Storage, PermissionBtn, DownloadBtn);
        }

    }
}
