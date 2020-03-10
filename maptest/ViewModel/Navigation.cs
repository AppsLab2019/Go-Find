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
using Xamarin.Forms.Maps;

namespace maptest.ViewModel
{

    public class Navigation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                if (value == _color)
                    return;

                _color = value;
                OnPropertyChanged();
            }
        }

        public void Find(List<Position> items)
        {
            Items = items;
            PositionRefresh();
            System.Threading.Thread.Sleep(4000);
            Blinktime = 0.000001;
            StartBlinking();
            FindClosest();
        }
        private void FindClosest()
        {
            Position closest = Items[1];
            foreach (var item in Items)
            {
                if (Math.Abs(closest.Latitude) + Math.Abs(closest.Longitude) - Math.Abs(PlayerPosition.Latitude) + Math.Abs(PlayerPosition.Longitude) > Math.Abs(item.Longitude) + Math.Abs(item.Latitude) - Math.Abs(PlayerPosition.Latitude) + Math.Abs(PlayerPosition.Longitude))
                    closest = item;
            }
            ClosestItem = closest;
        }
        private double Blinktime { get; set; }

        public List<Position> Items { get; set; }
        private Position ClosestItem { get; set; }

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
            Color = Color.Gold;
            System.Threading.Thread.Sleep(300);
            Color = Color.White;    
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Blinktime = Math.Abs((Math.Abs(ClosestItem.Latitude) + Math.Abs(ClosestItem.Longitude)) - (Math.Abs(PlayerPosition.Longitude) + Math.Abs(PlayerPosition.Latitude)));
            if (Blinktime != Math.Abs( ClosestItem.Latitude) + Math.Abs(ClosestItem.Longitude) || Blinktime != Math.Abs(PlayerPosition.Longitude) + Math.Abs(PlayerPosition.Latitude))
                Blinktime = Blinktime * 200000;
            else
            {
                Blinktime = Blinktime * 100;
            }
            aTimer.Interval = Blinktime;
        }
        /*private bool CollectItem(Object source, ElapsedEventArgs e)
        {
            if(PlayerPosition.Latitude + PlayerPosition.Longitude + 0.0005 >  ClosestItem.Latitude + ClosestItem.Longitude | ClosestItem.Latitude + ClosestItem.Longitude > PlayerPosition.Latitude + PlayerPosition.Longitude - 0.0005 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    }
}


