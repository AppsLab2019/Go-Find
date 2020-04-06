using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Timers;

namespace maptest.ViewModel
{
    class Player
    {
        public int Health { get; set; }
        public List<string> invetory {get; set;}
        public Player(int health)
        {
            Health = health;
        }
    }
}
