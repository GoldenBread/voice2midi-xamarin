using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SimpleAudioPlayer;
using Refit;
using voice2midi.Models;
using voice2midi.Services;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class SourceVoicePage : ContentPage
    {
        AudioRecorderService _recorder;
        ISimpleAudioPlayer _player;
        Voice2midiService _service;
        StreamPart _audioSource;

        public SourceVoicePage()
        {
            InitializeComponent();

            BindFromFile();

            _recorder = new AudioRecorderService
            {
                TotalAudioTimeout = TimeSpan.FromSeconds(10),
                StopRecordingOnSilence = false
            };

            _player = CrossSimpleAudioPlayer.Current;
            _player.PlaybackEnded += Player_Finished_Playing;

            _ = Check_Mic_Permission_Async();

            string baseUrl = (string)Application.Current.Resources["voice2midi_base_url"];
            _service = new Voice2midiService(baseUrl);
        }

        private void BindFromFile() //The page can be called with a file already selected
        {
            if (BindingContext != null)
            {
                var stream = (StreamPart)BindingContext;
                FiledLoadedLbl.Text = stream.FileName;
                FileInfoLayout.IsVisible = true;

                _audioSource = stream;
            }
        }

        private async Task Check_Mic_Permission_Async()
        {
            PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
            if (status == PermissionStatus.Granted)
            {
                RecordBtn.IsEnabled = true;
                PermissionBtn.IsEnabled = false;
            }
        }

        private async Task Ask_Mic_Permissions_Async()
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
                if (status != PermissionStatus.Granted)
                {
                    Dictionary<Permission, PermissionStatus> results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                    status = results[Permission.Microphone];
                }
                Console.WriteLine("\nPermissionsStatus");
                Console.WriteLine(status);
                if (status == PermissionStatus.Granted)
                {
                    Console.WriteLine("PermissionsStatus.Granted");
                    RecordBtn.IsEnabled = true;
                    PermissionBtn.IsEnabled = false;
                }
                else
                {
                    Console.WriteLine("Permission not granted");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception attempting to ask permission:\n{ex}\n");
                await Navigation.PopAsync();
            }

        }

        async void ConvertBtn_Clicked(object sender, EventArgs e)
        {
            SoundLinkList soundLinkList = await _service.Upload_Generate_Sound(_audioSource);

            await Navigation.PushAsync(new PlayPage
            {
                BindingContext = soundLinkList
            });
        }

        void PlayBtn_Clicked(object sender, EventArgs e)
        {
            var stream = _audioSource?.Value; // Null conditionnal operator in case if _audioSource is null
            Edit_InfoLabel(true, "Now Playing...");
            if (stream != null)
            {
                _player.Load(stream);
                _player.Play();
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

            Console.WriteLine("BtnPressed");
            await _recorder.StartRecording();
            Edit_InfoLabel(true, "Now Recording...");
        }

        async void RecordBtn_Released(object sender, EventArgs e)
        {
            Console.WriteLine("BtnReleased");
            await _recorder.StopRecording();
            Edit_InfoLabel(false);
            ConvertBtn.IsEnabled = true;

            _audioSource = new StreamPart(_recorder.GetAudioFileStream(), "source.wav", "audio/x-wav");
        }

        async void PermissionBtn_Clicked(object sender, EventArgs e)
        {
            await Ask_Mic_Permissions_Async();
        }

        private void Edit_InfoLabel(bool visibility, string msg = "")
        {
            InfoLbl.Text = msg;
            InfoLbl.IsVisible = visibility;
        }
    }
}
