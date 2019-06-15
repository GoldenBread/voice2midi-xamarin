using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVoice2midi.Interfaces;
using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RecordVoicePage());
        }

        async void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            //Voice2midiService service = new Voice2midiService("http://192.168.2.246:5000");
            Voice2midiService service = new Voice2midiService("http://vps662256.ovh.net:5000");
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                string fileName = fileData.FileName;
                string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);

                System.Console.WriteLine("File name chosen: " + fileName);
                System.Console.WriteLine("File data: " + contents);

                var uploadTask = service.Upload_f(new StreamPart(fileData.GetStream(), "oui.wav", "audio/x-wav"));

                var task = await uploadTask;
                Console.WriteLine(task);
                string json = await task.Content.ReadAsStringAsync();
                Console.WriteLine(json);
                SoundLinkList m = JsonConvert.DeserializeObject<SoundLinkList>(json);
                Console.WriteLine(m.mp3Link);

                await Navigation.PushAsync(new PlayPage
                {
                    BindingContext = m
                });

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }
    }
}
