using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace maptest.ViewModel
{
    public class Navigation : INotifyPropertyChanged
    {
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                if (value == _color)
                    return;

                _color = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private double GetTime()
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
                        Color = Color.White;
                        time = GetTime();
                    }
                    time = GetTime();
                    if (time > blinktime)
                    {
                        Color = Color.Gold;
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
