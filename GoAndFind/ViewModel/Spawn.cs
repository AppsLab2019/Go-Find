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
            foreach (var h in Items.ToList())
            {
                while (true)
                {
                    //Bandits
                    if (Chance(30))
                    {
                        if (Chance(70))
                        {
                            items.Add(new Item(h, "Bandit", "Causual Bandit", rnd.Next(1, 3)));
                            break;
                        }
                        else
                        {
                            items.Add(new Item(h, "Bandit", "veteran Bandit", rnd.Next(1, 2)));
                            break;
                        }
                    }
                    //Frndzalica
                    else if (Chance(40))
                    {
                        items.Add(new Item(h, "Healing", "liquor", rnd.Next(1, 2)));
                        break;
                    }
                    //armour
                    else if (Chance(30))
                    {
                        items.Add(new Item(h, "Upgrade", "armour", 1));
                        break;
                    }
                    else if (Chance(30))
                    {
                        items.Add(new Item(h, "Changer", "Hopefull stick of gloominess", 1));
                    }
                    //Legendary
                    else if (Chance(10))
                    {
                        items.Add(new Item(h, "Legendary", SpawnLegendaryItem(), 1));
                        break;
                    }
                }
            }
            return items;
        }
        private bool Chance(int chance)
        {
            var rnd = new Random();
            return chance > rnd.Next(0, 100);
        }
        private string SpawnLegendaryItem() 
        {
            var rand = new Random();
            var Legendary = new List<string>();
            Legendary.Add("Reaper's knife");
            Legendary.Add("Erasing wand");
            Legendary.Add("Hopefull stick of gloominess");
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


