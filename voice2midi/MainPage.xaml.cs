using System;
using System.ComponentModel;
using System.IO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Toast;
using Refit;
using voice2midi.Models;
using voice2midi.Services;
using Xamarin.Forms;

namespace voice2midi
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        async void Handle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SourceVoicePage());
        }

        async void Handle_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                {
                    return; // user canceled file picking
                }
                if (Path.GetExtension(fileData.FilePath) != ".wav")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CrossToastPopUp.Current.ShowToastError("Accept only .wav files");
                    });
                    return;
                }

                await Navigation.PushAsync(new SourceVoicePage(new StreamPart(fileData.GetStream(), fileData.FileName, "audio/x-wav")));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }
    }
}
