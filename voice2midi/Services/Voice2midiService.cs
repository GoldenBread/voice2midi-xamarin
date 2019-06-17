using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IVoice2midi.Interfaces;
using Newtonsoft.Json;
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

        public async Task<SoundLinkList> Upload_Generate_Sound(StreamPart stream)
        {
            try
            {
                SoundLinkList soundLink = await _api.UploadGenerate(stream).ConfigureAwait(false);
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

        public async Task<SoundLinkLists> Sound_List()
        {
            try
            {
                SoundLinkLists soundLinks = await _api.SoundList().ConfigureAwait(false);
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
