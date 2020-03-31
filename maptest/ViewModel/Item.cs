using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace maptest.NewFolder
{
    class Item
    {
        public Position ItemSpawn(Position playerposition)
        {

            var rnd = new Random();
            double rlon = rnd.Next(-100, 100);
            double lon = rlon / 40000;
            double rlat = rnd.Next(-100, 100);
            double lat = rlat / 40000;
            return new Position(playerposition.Latitude + lat, playerposition.Longitude + lon);
        }

        public List<Position> Loot(int count, Position playerposition)
        {
            var loot = new Position();
            var itemlist = new List<Position>();
            for (int i = 0; i <= 1; i++)
            {
                loot = (new Position(playerposition.Latitude, playerposition.Longitude));
                for (int c = 0; c <= count; c++)
                {
                    var lastitem = new Position(ItemSpawn(loot).Latitude,ItemSpawn(loot).Longitude);
                    itemlist.Add(lastitem);
                    loot = lastitem;
                }             
            }
            return itemlist;
        }

        //public Item ItemGet(Position playerposition; Position  )

    }
}


