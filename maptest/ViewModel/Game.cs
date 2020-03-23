using maptest.NewFolder;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace maptest.ViewModel
{
    public class Game
    {
        private readonly Item item = new Item();
        public List<Position> Items { get; set; }
        public List<Position> Bandits { get; set; }
        
        public void SpawnItems(Position location)
        {
            var items = item.Loot(5, new Position(location.Latitude, location.Longitude));

            foreach (var loot in items)
            {
                Items.Add(loot);
            }
        }
        public void MakeBandits()
        {
            int a = 0;
            foreach (var h in Items)
            {
                a++;
                if(a == 3)
                {
                    a = 0;
                    Bandits.Add(h);
                    Items.Remove(h);

                }
            }
        } 
    }
}
