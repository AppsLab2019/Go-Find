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
                });;
            });
            Device.StartTimer(new TimeSpan(0), () =>
              {
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      Find(itemloc);
                  });
                  return true;
              });
            

        } 
        public void Find(Position item)
        {
            var player = new Player();
            double time = DateTime.Now.Millisecond;
            double refresh = DateTime.Now.Millisecond + 200;
            double longitude = item.Longitude;
            double latitude = item.Latitude; 
            double blinktime = time;
            Device.StartTimer(new TimeSpan(200), () =>
            {
                // do something every 60 seconds
                Task.Run(() =>
                {
                    while (true)
                    {
                        time = DateTime.Now.Millisecond;
                        if (blinktime < 0)
                        {
                            blinktime = -blinktime;
                        }

                        if (time <= blinktime)
                        {
                            button.BackgroundColor = Color.Gold;
                        }
                        else if (time >= blinktime + 1000)
                        {
                            button.BackgroundColor = Color.White;
                            blinktime = time + (((item.Latitude + item.Longitude) - (longitude + latitude)) / 2);
                        }
                        if (refresh <= time)
                        {
                            longitude = player.PlayerPositon().Result.Longitude;
                            latitude = player.PlayerPositon().Result.Latitude;
                            refresh = DateTime.Now.Millisecond + 200;
                        }
                    }
                });
                return false; // runs again, or false to stop
            });
        
        }
    }
}
