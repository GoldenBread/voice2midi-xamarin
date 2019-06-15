using System;
using System.Collections.Generic;
using Plugin.AudioRecorder;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class RecordVoicePage : ContentPage
    {
        AudioRecorderService _recorder;
        AudioPlayer _player;

        public RecordVoicePage()
        {
            InitializeComponent();
            _recorder = new AudioRecorderService
            {
                TotalAudioTimeout = TimeSpan.FromSeconds(10),
                StopRecordingOnSilence = false
            };
            //_recorder.
            _player = new AudioPlayer();

            _ = AskPermissionsAsync();
        }

        private async System.Threading.Tasks.Task AskPermissionsAsync()
        {
            Console.WriteLine("Asking permission");
            try
            {
                Console.WriteLine("Asking permission 2");
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
                Console.WriteLine("Asking permission 3");
                if (status != PermissionStatus.Granted)
                {
                    Console.WriteLine("Asking permission 4");
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                    {
                        await DisplayAlert("Need microphone", "Le mic", "OK");
                    }
                    Console.WriteLine("Asking permission 5");

                    Dictionary<Permission, PermissionStatus> results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                    Console.WriteLine(results);
                    status = results[Permission.Microphone];
                }
                Console.WriteLine("Asking permission 6");
                Console.WriteLine(status);

                if (status == PermissionStatus.Granted)
                {
                    //Query permission
                    Console.WriteLine("PermissionsStatus.Granted");
                }
                else if (status != PermissionStatus.Unknown)
                {
                    Console.WriteLine("PermissionsStatus.Uknown");
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

        async void RecordBtn_Pressed(object sender, System.EventArgs e)
        {
            Console.WriteLine("BtnPressed");
            await _recorder.StartRecording();
            NowRecordingLbl.IsVisible = true;
        }

        async void RecordBtn_Released(object sender, System.EventArgs e)
        {
            Console.WriteLine("BtnReleased");
            await _recorder.StopRecording();
            NowRecordingLbl.IsVisible = false;
        }
    }
}
