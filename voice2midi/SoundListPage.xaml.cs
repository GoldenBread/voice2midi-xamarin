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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _ = Update_ListAsync();
        }

        private void OnItemListTap(object sender, EventArgs e)
        {
            ItemTappedEventArgs item = (ItemTappedEventArgs)e;
            List<FileModelShort> selectedList = (List<FileModelShort>)item.Item;

            if (selectedList != null)
            {
                Navigation.PushAsync(new PlayPage(selectedList));
            }
        }

        private async Task Update_ListAsync()
        {
            var soundLinkLists = await _service.Sound_List();

            _soundLinkLists = new ObservableCollection<List<FileModelShort>>(soundLinkLists);
            SoundListView.ItemsSource = _soundLinkLists;
            //SoundListView.ItemsSource = soundLinkLists;
        }
    }
}
