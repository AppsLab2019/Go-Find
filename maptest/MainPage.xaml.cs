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
        public MainPage()
        {
            InitializeComponent();
            map.MapType = MapType.Street;
            map.IsShowingUser = true;

            GetPosition();


        }

        private async void GetPosition()
        {
            var navigation = new Navigation();

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            var task = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            var location = task;

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.444));
            map.MoveToRegion(mapSpan);


            var item = new Item();


            var itemloc = new Position(item.ItemSpawn(location).Longitude, item.ItemSpawn(location).Latitude);


            Device.BeginInvokeOnMainThread(() =>
            {
                map.Pins.Add(new Pin
                {
                    Label = "Item",
                    Position = itemloc

                }); ;
            });
            Find(itemloc);
        }
        public double GetTime()
        {
            double miliseconds = DateTime.Now.Millisecond;
            double seconds = DateTime.Now.Second;
            double minutes = DateTime.Now.Minute;
            return ((minutes * 60) + seconds) * 100 + miliseconds;
        }
        public void Find(Position item)
        {
            var player = new Player();
            double time = GetTime();
            double refresh = time + 300;
            double longitude = 0;
            double latitude = 0;
            double blinktime = 0;

            Device.StartTimer(new TimeSpan(50), () =>
            {
                Task.Run(() =>
                {

                    time = GetTime();

                    if (blinktime < 0)
                    {
                        blinktime = -blinktime;
                    }
                    time = GetTime();
                    if (time <= blinktime)
                    {
                        button.BackgroundColor = Color.White;
                        time = GetTime();
                    }
                    time = GetTime();
                    if (time > blinktime)
                    {
                        button.BackgroundColor = Color.Gold;
                        blinktime = time + ((item.Latitude + item.Longitude) - (longitude + latitude)) * 2;
                        time = GetTime();
                    }
                    if (refresh <= time)
                    {
                        longitude = player.PlayerPositon().Result.Longitude;
                        latitude = player.PlayerPositon().Result.Latitude;
                        refresh = time + 300;
                        time = GetTime();
                    }
                    time = GetTime();

                });
                return true; // runs again, or false to stop
            });


        }
    }
}
