using System;
using System.Collections.Generic;
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

        public async Task<FileUploadModel> UploadSound(StreamPart stream)
        {
            try
            {
                FileUploadModel fileUploadModel = await _api.Upload(stream).ConfigureAwait(false);
                return fileUploadModel;
            }
            catch (Exception e)
            {
                RemoteServerError(e);
                return null;
            }
        }

        public async Task<FileGenerationModel> GenerateSound(string format, long id)
        {
            try
            {
                FileGenerationModel generationModel = await _api.Generate(format, id).ConfigureAwait(false);
                return generationModel;
            }
            catch (Exception e)
            {
                RemoteServerError(e);
                return null;
            }
        }

        public async Task<List<List<FileModelShort>>> SoundList()
        {
            try
            {
                var soundLinks = await _api.SoundList().ConfigureAwait(false);
                return soundLinks;
            }
            catch (Exception e)
            {
                RemoteServerError(e);
                return null;
            }
        }

        public async Task<List<FileModelShort>> SoundListSameSource(long id)
        {
            try
            {
                var soundLinks = await _api.SoundListSameSource(id).ConfigureAwait(false);
                return soundLinks;
            }
            catch (Exception e)
            {
                RemoteServerError(e);
                return null;
            }
        }

        private void RemoteServerError(Exception e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                CrossToastPopUp.Current.ShowToastError($"Error from remote server: \n{e.Message}", ToastLength.Long);
            });
        }
    }
}
