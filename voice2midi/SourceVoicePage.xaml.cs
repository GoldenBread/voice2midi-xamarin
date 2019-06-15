using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class SourceVoicePage : ContentPage
    {
        AudioRecorderService _recorder;
        AudioPlayer _player;

        public SourceVoicePage()
        {
            InitializeComponent();

            _recorder = new AudioRecorderService
            {
                TotalAudioTimeout = TimeSpan.FromSeconds(10),
                StopRecordingOnSilence = false
            };

            _player = new AudioPlayer();
            _player.FinishedPlaying += Player_Finished_Playing;

            _ = Check_Mic_Permission_Async();
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

        void StopBtn_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        void PlayBtn_Clicked(object sender, System.EventArgs e)
        {
            var filePath = _recorder.FilePath;
            if (filePath != null)
            {
                _player.Play(filePath);
            }
        }

        void Player_Finished_Playing(object sender, EventArgs e)
        {
            Edit_InfoLabel(false);
        }

        async void RecordBtn_Pressed(object sender, System.EventArgs e)
        {
            Console.WriteLine("BtnPressed");
            await _recorder.StartRecording();
            Edit_InfoLabel(true, "Now Recording...");
        }

        async void RecordBtn_Released(object sender, System.EventArgs e)
        {
            Console.WriteLine("BtnReleased");
            await _recorder.StopRecording();
            Edit_InfoLabel(false);
        }

        async void PermissionBtn_Clicked(object sender, System.EventArgs e)
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
