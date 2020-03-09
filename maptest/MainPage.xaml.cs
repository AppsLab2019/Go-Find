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

namespace maptest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly Navigation viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new Navigation();
            BindingContext = viewModel;


            map.MapType = MapType.Street;
            map.IsShowingUser = true;

            GetStartet();


        }

        private async void GetStartet()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            var task = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            var location = task;

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.444));
            map.MoveToRegion(mapSpan);


            var item = new Item();


            var items = new List<Position>();
            items = item.Loot(20, new Position(location.Latitude,location.Longitude));

            foreach (var loot in items)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    map.Pins.Add(new Pin
                    {

                        Label = "Item",
                        Position = new Position(loot.Latitude,loot.Longitude),

                    }); ;
                });
            }
            viewModel.Find(items);
        }

    }
}
