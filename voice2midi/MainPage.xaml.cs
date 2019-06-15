﻿using System;
using System.ComponentModel;
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

        async void Handle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SourceVoicePage());
        }

        async void Handle_Clicked_1(object sender, EventArgs e)
        {
            string baseUrl = (string)Application.Current.Resources["voice2midi_base_url"];
            Voice2midiService service = new Voice2midiService(baseUrl);
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                await Navigation.PushAsync(new SourceVoicePage
                {
                    BindingContext = new StreamPart(fileData.GetStream(), fileData.FileName, "audio/x-wav")
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }
    }
}
