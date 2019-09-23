using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVoice2midi.Interfaces;
using Plugin.Toast;
using Plugin.Toast.Abstractions;
using Refit;
using voice2midi.Models;
using Xamarin.Forms;

namespace voice2midi.Services
{
    public class Voice2midiService
    {
        private readonly IVoice2midiAPI _api;

        public Voice2midiService(string baseUrl)
        {
            _api = RestService.For<IVoice2midiAPI>(baseUrl);
        }

        public async Task<List<FileModelShort>> Upload_Generate_Sound(StreamPart stream)
        {
            try
            {
                List<FileModelShort> soundLink = await _api.UploadGenerate(stream).ConfigureAwait(false);
                return soundLink;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossToastPopUp.Current.ShowToastError($"Error from remote server: \n{e.Message}", ToastLength.Long);
                });
                return null;
            }
        }

        public async Task<List<List<FileModelShort>>> Sound_List()
        {
            try
            {
                var soundLinks = await _api.SoundList().ConfigureAwait(false);
                return soundLinks;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossToastPopUp.Current.ShowToastError($"Error from remote server: \n{e.Message}", ToastLength.Long);
                });
                return null;
            }
        }
    }
}
