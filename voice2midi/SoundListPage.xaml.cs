using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using voice2midi.Models;
using voice2midi.Services;
using Xamarin.Forms;

namespace voice2midi
{
    public partial class SoundListPage : ContentPage
    {
        Voice2midiService _service;
        ObservableCollection<List<FileModelShort>> _soundLinkLists;
        public ObservableCollection<List<FileModelShort>> SoundLinkLists { get { return _soundLinkLists; } }//Bind with ListView

        public SoundListPage()
        {
            InitializeComponent();

            string baseUrl = (string)Application.Current.Resources["voice2midi_base_url"];
            _service = new Voice2midiService(baseUrl);

            SoundListView.ItemTapped += OnItemListTap;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Update_ListAsync();
        }

        private async void OnItemListTap(object sender, EventArgs e)
        {
            ItemTappedEventArgs item = (ItemTappedEventArgs)e;
            List<FileModelShort> selectedList = (List<FileModelShort>)item.Item;

            if (selectedList != null)
            {
                await Navigation.PushAsync(new PlayPage(selectedList));
            }
        }

        private async Task Update_ListAsync()
        {
            var soundLinkLists = await _service.SoundList();
            _soundLinkLists = new ObservableCollection<List<FileModelShort>>(soundLinkLists);
            SoundListView.ItemsSource = _soundLinkLists;
            //SoundListView.ItemsSource = soundLinkLists;
        }
    }
}
