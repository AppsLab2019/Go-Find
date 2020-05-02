using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAndFind.ViewModel;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace GoAndFind.NewFolder
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
            var rnd = new Random();
            var items = new List<Item>();        
            double a = 0;
            double b = Items.Count;
            foreach (var h in Items.ToList())
            {
                var chance = rnd.Next(0, 100);
                a++;

                //Bandits
                if (chance <= 35)
                {
                    if (chance < 20)
                        items.Add(new Item(h, "Bandit", "Causual Bandit", rnd.Next(1, 3)));
                    else if (chance < 35)
                        items.Add(new Item(h, "Bandit", "Veteran Bandit", rnd.Next(1, 2)));
                }

                //Frndzalica
                else if (chance < 60)
                    items.Add(new Item(h, "Healing", "Firewater", rnd.Next(0, 2)));

                //Armour
                else if (chance < 98)
                    items.Add(new Item(h, "upgrade", "Armour", 1));

                //Legendary
                else if (chance == 99)
                {
                    items.Add(new Item(h, "Legendary", SpawnLegendaryItem(), 1));

                }
                else if (chance == 100 && GameUpgrade == true)
                { 
                    //items.Add(new Item(h, "Master", SpawnNewItems(), 1));
                }                    
                    
            }
            
            return items;
        }
        public string SpawnLegendaryItem() 
        {
            var rand = new Random();
            var Legendary = new List<string>();
            Legendary.Add("Meč hrdlorez");
            Legendary.Add("Kniha múdrostí");
            Legendary.Add("Palička nádeje");
            return Legendary[(rand.Next(1, Legendary.Count))];
        }
        public void SpawnNewItems()
        {
            var rand = new Random();
            var Master = new List<string>();
            Master.Add("Shield Of The Hero");
        }
        public bool GameUpgrade = false;
    }
}


