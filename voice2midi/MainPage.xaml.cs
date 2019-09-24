using System;
using System.ComponentModel;
using System.IO;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Toast;
using Refit;
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

        async void RecordBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SourceVoicePage());
        }

        async void SendFileBtn_Clicked(object sender, EventArgs e)
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

        async void ListSoundsBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SoundListPage());
        }

    }
}
