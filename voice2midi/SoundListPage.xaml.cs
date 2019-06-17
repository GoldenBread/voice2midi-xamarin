using System;
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
        ObservableCollection<SoundLinkList> _soundLinkLists;
        public ObservableCollection<SoundLinkList> SoundLinkLists { get { return _soundLinkLists; } }//Bind with ListView

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
            SoundLinkList selectedList = (SoundLinkList)item.Item;

            if (selectedList != null)
            {
                Navigation.PushAsync(new PlayPage(selectedList));
            }
        }

        private async Task Update_ListAsync()
        {
            SoundLinkLists soundLinkLists = await _service.Sound_List();

            _soundLinkLists = new ObservableCollection<SoundLinkList>(soundLinkLists.soundLinkLists);
            SoundListView.ItemsSource = _soundLinkLists;
        }
    }
}
