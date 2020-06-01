using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAndFind.hint;
using Plugin.Geolocator;
using Xamarin.Forms.GoogleMaps;

namespace GoAndFind.NewFolder
{
    class Spawn
    {
        public Position PositionSpawn(Position playerposition)
        {

            var rnd = new Random();
            double rlon = rnd.Next(-160, 160);
            double lon = rlon / 40000;
            double rlat = rnd.Next(-160, 160);
            double lat = rlat / 40000;
            return new Position(playerposition.Latitude + lat, playerposition.Longitude + lon);
        }

        public List<Position> PositionsSpawn(int count, Position playerposition)
        {
            var loot = new Position();
            var itemlist = new List<Position>();
                loot = (new Position(playerposition.Latitude, playerposition.Longitude));
            for (int c = 0; c <= count; c++)
            {
                var lastitem = new Position(PositionSpawn(loot).Latitude, PositionSpawn(loot).Longitude);
                itemlist.Add(lastitem);
                loot = lastitem;
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
                    if (Chance(20))
                    {
                        if (Chance(70))
                        {
                            items.Add(new Item(h, "Bandit", "Causual Bandit", rnd.Next(1, 4)));
                            break;
                        }
                        else
                        {
                            items.Add(new Item(h, "Bandit", "Veteran Bandit", rnd.Next(1, 3)));
                            break;
                        }
                    }
                    //Frndzalica
                    else if (Chance(20))
                    {
                        items.Add(new Item(h, "Healing", "Liquor", rnd.Next(1, 2)));
                        break;
                    }
                    //Ňuchač
                    else if (Chance(20))
                    {
                        items.Add(new Item(h, "Hint", "Piece of map", 1));
                        break;
                    }
                    //armour
                    else if (Chance(10))
                    {
                        items.Add(new Item(h, "Upgrade", "Armour", 1));
                        break;
                    }
                    else if (Chance(15))
                    {
                        items.Add(new Item(h, "Changer", "Hopefull stick of gloominess", 1));
                    }
                    else if (Chance(20))
                    {
                        items.Add(new Item(h, "Bait", "JustKidding", 1));
                    }
                    else if (Chance(20))
                    {
                        items.Add(new Item(h, "Hint", "Bandit letter",1));
                    }
                    //Legendary
                    else if (Chance(10))
                    {
                        items.Add(SpawnLegendaryItem(h));
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
        public Item SpawnLegendaryItem(Position position) 
        {
            var rand = new Random();
            var Legendary = new List<string>();
            Legendary.Add("Dead man's macaroni");
            Legendary.Add("Erasing wand");
            Legendary.Add("Dead man's Sword");
            return new Item(position,"Legendary",Legendary[(rand.Next(0, Legendary.Count))],1);
        }
    }
}


