using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using maptest.NewFolder;
using maptest.ViewModel;
using System.Threading;

namespace maptest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly Navigation viewModel;
        private List<Position> Items { get; set; }
        private List<Position> Bandits { get; set; }
        private List<Position> All { get; set; }
        public MainPage()
        {
            InitializeComponent();
            viewModel = new Navigation();
            All = new List<Position>();
            Items = new List<Position>();
            Bandits = new List<Position>();
            BindingContext = viewModel;
            map.MapType = MapType.Street;
            map.IsShowingUser = true;

            GetStartet();
        }

        public void SpawnItems(Position location)
        {
            var item = new Item();
            var items = item.Loot(5, new Position(location.Latitude, location.Longitude));

            foreach (var loot in items)
            {
                Items.Add(loot);
                All.Add(loot);
            }
        }
        public void MakeBandits()
        {
            int a = 0;
            foreach (var h in Items.ToList())
            {
                a++;
                if (a == 3)
                {
                    a = 0;
                    Bandits.Add(h);
                    Items.Remove(h);
                }
            }
        }
        private async void GetStartet()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            var task = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            var location = task;

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.444));
            map.MoveToRegion(mapSpan);

            SpawnItems(new Position(location.Latitude, location.Longitude));

            MakeBandits();
            MarkItems(All);
            viewModel.Refreshlists(All);
            viewModel.Find();
            AutoSpawn(viewModel);
        }
        public void MarkItems(List<Position> locations)
        {
            string item;
            foreach (var loot in locations.ToList())
            {
                if (Items.Contains(loot))
                {
                    item = "Item";
                }
                else
                {
                    item = "Bandit";
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Items.Contains(loot))
                        map.Pins.Add(new Pin
                        {
                            Label = item,
                            Position = new Position(loot.Latitude, loot.Longitude),
                        }); ;
                });
            }
        }
        public void AutoSpawn(Navigation nav)
        {
            nav.Spawnew += () => SpawnItems(nav.PlayerPosition);
            nav.Spawnew += () => MarkItems(All);
        }
        public void ButtonOnClicked(object sender, EventArgs e)
        {
            if (viewModel.ItemIsClose)
            {
                All.Remove(viewModel.ClosestItem);
                viewModel.Refreshlists(All);
                viewModel.FindClosest();
                if(Items.Contains(viewModel.ClosestItem))
                    DisplayAlert("Alert", "You have collected item", "OK");
                if(Bandits.Contains(viewModel.ClosestItem))
                    DisplayAlert("Alert", "You have been ambushed", "OK");
            }
        }
    }
}
