using System;
using System.IO;
using Plugin.AudioRecorder;
using Plugin.Permissions.Abstractions;
using Refit;
using voice2midi.Models;
using voice2midi.Services;
using voice2midi.SystemConf;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class SourceVoicePage : ContentPage
    {
        AudioRecorderService _recorder;
        AudioPlayer _player;
        Voice2midiService _service;
        StreamPart _audioSource;

        public SourceVoicePage(StreamPart fileStream = null)
        {
            InitializeComponent();

            Bind_From_File(fileStream);

            _recorder = new AudioRecorderService
            {
                TotalAudioTimeout = TimeSpan.FromSeconds(10),
                StopRecordingOnSilence = false
            };

            _player = new AudioPlayer();
            _player.FinishedPlaying += Player_Finished_Playing;

            _ = RequestPermission.Check_Permission_Async(Permission.Microphone, PermissionBtn, ConvertBtn);

            string baseUrl = (string)Application.Current.Resources["voice2midi_base_url"];
            _service = new Voice2midiService(baseUrl);

        }

        private void Bind_From_File(StreamPart fileStream) //The page can be called with a file already selected
        {
            if (fileStream != null)
            {
                FiledLoadedLbl.Text = fileStream.FileName;
                FileInfoLayout.IsVisible = true;

                ConvertBtn.IsEnabled = true;

                _audioSource = fileStream;
            }
        }

        async void ConvertBtn_Clicked(object sender, EventArgs e)
        {
            Edit_Loading_Indicator(true);
            SoundLinkList soundLinkList = await _service.Upload_Generate_Sound(_audioSource);
            Edit_Loading_Indicator(false);

            if (soundLinkList != null)
            {
                await Navigation.PushAsync(new PlayPage(soundLinkList));
            }
        }

        void PlayBtn_Clicked(object sender, EventArgs e)
        {
            var filePath = ((FileStream)(_audioSource?.Value))?.Name; // Null conditionnal operator in case if _audioSource is null. Stream cast to FileStream to get the Name property
            Edit_InfoLabel(true, "Now Playing...");
            if (filePath != null)
            {
                _player.Play(filePath);
            }
        }

        void Player_Finished_Playing(object sender, EventArgs e)
        {
            Edit_InfoLabel(false);
        }

        async void RecordBtn_Pressed(object sender, EventArgs e)
        {
            if (FileInfoLayout.IsVisible) // If we start recording, record sound will overwrite the eventual loaded file
            {
                FileInfoLayout.IsVisible = false;
            }

            await _recorder.StartRecording();
            Edit_InfoLabel(true, "Now Recording...");
        }

        async void RecordBtn_Released(object sender, EventArgs e)
        {
            await _recorder.StopRecording();
            Edit_InfoLabel(false);
            ConvertBtn.IsEnabled = true;
            PlayBtn.IsEnabled = true;

            _audioSource = new StreamPart(_recorder.GetAudioFileStream(), "source.wav", "audio/x-wav");
        }

        async void PermissionBtn_Clicked(object sender, EventArgs e)
        {
            await RequestPermission.Ask_Permissions_Async(Permission.Microphone, PermissionBtn, ConvertBtn);
        }

        private void Edit_InfoLabel(bool visibility, string msg = "")
        {
            InfoLbl.Text = msg;
            InfoLbl.IsVisible = visibility;
        }

        private void Edit_Loading_Indicator(bool visibility)
        {
            LoadingIdctr.IsRunning = visibility;
            InfoLbl.IsVisible = visibility;
            InfoLbl.Text = "Loading...";
        }

        protected override void OnAppearing() // When coming from PlayPage, the audio_source stream closes. Disabling ConvertBtn to prevent errors.
        {
            if (!(_audioSource?.Value.CanRead ?? false)) // Null coalescing operator mixed with null conditional operator
            {
                PlayBtn.IsEnabled = false;
                ConvertBtn.IsEnabled = false;
                FileInfoLayout.IsVisible = false;
            }
        }

    }
}
