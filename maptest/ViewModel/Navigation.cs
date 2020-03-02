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
            var player = new Player();
            player.PositionRefresh();
            Blinktime = 1000;
            StartBlinking();
        }

        private double Blinktime { get; set; }

        private Position Player { get => new Player().PlayerPosition; }
        private Position ClosestItem { get; set; }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Blinktime = Math.Abs((ClosestItem.Latitude + ClosestItem.Longitude) - (Player.Longitude + Player.Latitude));
            Blinktime = Blinktime * 500;
        }

        private static Timer aTimer;
        public void StartBlinking()
        {
            aTimer = new Timer();
            aTimer.Interval = 100;
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
            System.Threading.Thread.Sleep(Convert.ToInt32(Blinktime));
        }
    }
}

