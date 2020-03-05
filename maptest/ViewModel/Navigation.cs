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

        public void Find(Position item)
        {
            ClosestItem = item;  
            PositionRefresh();
            System.Threading.Thread.Sleep(4000);
            Blinktime = 0.000001;
            StartBlinking();
        }
        
        private double Blinktime { get; set; }


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
            bTimer.Interval = 2000;

            // Hook up the Elapsed event for the timer. 
            bTimer.Elapsed += OnBTime;

            // Have the timer fire repeated events (true is the default)
            bTimer.AutoReset = true;

            // Start the timer
            bTimer.Enabled = true;

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
            System.Threading.Thread.Sleep(500);
            Color = Color.White;
            aTimer.Interval = Blinktime;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Blinktime = Math.Abs((ClosestItem.Latitude + ClosestItem.Longitude) - (PlayerPosition.Longitude + PlayerPosition.Latitude));
            if(Blinktime < 1)
                Blinktime = Blinktime * 1500000;
        }
        private void ItemGTFO(Position ClosestItem, Position PlayerPosition)
        {
            if(ClosestItem.Longitude + ClosestItem.Latitude == PlayerPosition.Longitude + PlayerPosition.Latitude)
            {
               
            }
        }
    }
}


