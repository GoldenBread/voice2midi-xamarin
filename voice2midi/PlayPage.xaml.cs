﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        List<FileModelShort> _soundLinks;

        public PlayPage(List<FileModelShort> soundLinks)
        {
            InitializeComponent();
            _soundLinks = soundLinks;
            _player = CrossSimpleAudioPlayer.Current;


            _ = RequestPermission.Check_Permission_Async(
                Permission.Storage,
                PermissionBtn,
                new Button[] { DownloadMp3Btn, DownloadMidiBtn, DownloadWavBtn });

            SetupDownloadPath();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await InitAudioPlayer();
        }

        private async Task InitAudioPlayer()
        {
            var audioStream = Get_Stream(GetDownloadLink(".mp3").Url);
            if (audioStream != null)
            {
                var loadPlayerAsync = Task.Run(() => _player.Load(audioStream));// Vary on connection speed
                await loadPlayerAsync.ContinueWith(previous => PlayMusicBtnEnabler(audioStream));
            }
        }

        private void PlayMusicBtnEnabler(Stream audioStream)
        {
            Console.WriteLine("audioStream.CanRead" + audioStream.CanRead);
            Device.BeginInvokeOnMainThread(() =>
            {
                if (audioStream != null && audioStream.CanRead)
                {
                    PlayMusicBtn.IsEnabled = true;
                }
                else
                {
                    PlayMusicBtn.IsEnabled = false;
                }
            });
        }

        private void SetupDownloadPath()
        {
            string path = DependencyService.Get<IDownload>().GetDefaultPath();
            DownloadPathInfoLbl.Text = path;
        }

        public void PlayBtn_Clicked(object sender, EventArgs e)
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

        public void DownloadBtn_Clicked(object sender, EventArgs e)
        {
            Button _sender = (Button)sender;

            if (_sender.Id == DownloadMp3Btn.Id)
                DependencyService.Get<IDownload>().DownloadUrl(GetDownloadLink(".mp3"));
            else if (_sender.Id == DownloadMidiBtn.Id)
                DependencyService.Get<IDownload>().DownloadUrl(GetDownloadLink(".mid"));
            else if (_sender.Id == DownloadWavBtn.Id)
                DependencyService.Get<IDownload>().DownloadUrl(GetDownloadLink(".wav"));
        }

        private FileUrlStruct GetDownloadLink(string fileExtension)
        {
            string baseUrl = (string)Application.Current.Resources["voice2midi_base_url"];
            long id = _soundLinks
                .Where(x => x.FileExtension == fileExtension)
                .Select(x => x.Id)
                .First();
            return new FileUrlStruct{ Url = $"{baseUrl}/api/files/{id}/download", FileExtension = fileExtension };
        }

        public async void PermissionBtn_Clicked(object sender, EventArgs e)
        {
            await RequestPermission.Ask_Permissions_Async(
                Permission.Storage,
                PermissionBtn,
                new Button[] { DownloadMp3Btn, DownloadMidiBtn, DownloadWavBtn });
        }

    }
}
