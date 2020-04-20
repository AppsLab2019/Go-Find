using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using maptest.ViewModel;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace maptest.NewFolder
{
    class Spawn
    {
        public Position PositionSpawn(Position playerposition)
        {

            var rnd = new Random();
            double rlon = rnd.Next(-100, 100);
            double lon = rlon / 40000;
            double rlat = rnd.Next(-100, 100);
            double lat = rlat / 40000;
            return new Position(playerposition.Latitude + lat, playerposition.Longitude + lon);
        }

        public List<Position> PositionsSpawn(int count, Position playerposition)
        {
            var loot = new Position();
            var itemlist = new List<Position>();
            for (int i = 0; i <= 1; i++)
            {
                loot = (new Position(playerposition.Latitude, playerposition.Longitude));
                for (int c = 0; c <= count; c++)
                {
                    var lastitem = new Position(PositionSpawn(loot).Latitude,PositionSpawn(loot).Longitude);
                    itemlist.Add(lastitem);
                    loot = lastitem;
                }             
            }
            return itemlist;
        }
        public List<Item> SpawnItems(List<Position> Items)
        {
            var items = new List<Item>();        
            double a = 0;
            double b = Items.Count;          
            foreach (var h in Items.ToList())
            {
                a++;
                if ((a / b) * 100 <= 30)
                    items.Add(new Item(h, "EasyBandit"));
                else if ((a / b) * 100 <= 50)
                    items.Add(new Item(h, "MediumBandit"));
                else if ((a / b) * 100 > 50 && (a / b) * 100 <= 80)
                    items.Add(new Item(h, "Frndžalica"));
                else
                    items.Add(new Item(h, "Armour"));
            }
            return items;
        }
        
    }
}


