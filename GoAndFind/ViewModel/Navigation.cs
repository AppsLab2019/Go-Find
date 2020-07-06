using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GoAndFind.viewModel
{

    public class Navigation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                if (value == _image)
                    return;

                _image = value;
                OnPropertyChanged();
            }
        }

        public async void Find()
        {
            await GetPlayerPositon();
            PositionRefresh();
            FindClosest();
            Blinktime = 0.000001;
            StartBlinking();
        }
        public void Refreshlists(List<Position> positions)
        {
            Items = positions;
        }
        private List<Position> Items;
        public void FindClosest()
        {
            Position closest = Items[1];
            foreach (var item in Items)
            {
                if (DistanceBetween(closest,PlayerPosition) > DistanceBetween(item,PlayerPosition))
                    closest = item;
            }
            ClosestItem = closest;
        }
        public double Blinktime { get; set; }

        public Position ClosestItem { get; set; }
        public bool ItemIsClose { get; set; }
        public bool ItemIsFar { get; set; }

        private static Timer bTimer;

        public Position PlayerPosition { get; set; }

        public async Task<Position> GetPlayerPositon()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 30;

            var task = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            var location = task;

            return new Position(location.Latitude, location.Longitude);
        }
        public void PositionRefresh()
        {
            bTimer = new Timer();
            bTimer.Interval = 1500;

            // Hook up the Elapsed event for the timer. 
            bTimer.Elapsed += OnBTime;
            bTimer.Elapsed += CloseRef;

            // Have the timer fire repeated events (true is the default)
            bTimer.AutoReset = true;

            // Start the timer
            bTimer.Enabled = true;
        }
        private void CloseRef(Object source, ElapsedEventArgs e)
        {
            FindClosest();
        }
        private void OnBTime(Object source, ElapsedEventArgs e)
        {
            PlayerPosition = new Position(GetPlayerPositon().Result.Latitude, GetPlayerPositon().Result.Longitude);
        }
        private static Timer aTimer;
        public void StartBlinking()
        {
            aTimer = new Timer();
            aTimer.Interval = Blinktime;
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Elapsed += Blink;
            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;

        }
        private void Blink(Object source, ElapsedEventArgs e)
        {
            if (ItemIsClose)
                Image = "lampg.png";
            else
            {
                Image = "lampYellow.png";
                System.Threading.Thread.Sleep(400);
                Image = "lampOff.png";
            }
        }
        public double DistanceBetween(Position first,Position second)
        {
            return Math.Abs((Math.Abs(first.Latitude) - Math.Abs(second.Latitude)) + (Math.Abs(second.Longitude) - Math.Abs(first.Longitude)));
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Blinktime = DistanceBetween(ClosestItem, PlayerPosition);
            if (PlayerPosition.Latitude != 0 && PlayerPosition.Longitude != 0)
            {
                Blinktime *= 2000000;
                Blinktime += Blinktime;
            }
            aTimer.Interval = Blinktime;
            ItemControl();
        }
        private void ItemControl()
        {
            if (Blinktime < 400)
                ItemIsClose = true;
            else
                ItemIsClose = false;
            New();
        }
        public delegate void SpawnNewEventHandler();

        public event SpawnNewEventHandler Spawnew;
        public void New()
        {
            if (DistanceBetween(PlayerPosition,ClosestItem) > 0.003 || Items.Count < 4)
            {
                Spawnew();
            }
        } 
    }
}


