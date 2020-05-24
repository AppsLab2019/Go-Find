using Android.Graphics;
using GoAndFind.NewFolder;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Color = Xamarin.Forms.Color;

namespace GoAndFind.ViewModel
{
    class Hint
    {
        private Item LegendaryItem;
        private int Span { get; set; } = 100;
        public bool HintExist { get; set; }
        public void CreateHint(List<Item> items, Map map, Position PlayerPosition)
        {
            if (LegendaryExist(items))
            {
                CreateCircle(map);
            }
            else
            {
                var spawn = new Spawn();
                var position = spawn.PositionSpawn(PlayerPosition);
                items.Add(spawn.SpawnLegendaryItem(position));
            }
        }
        public bool LegendaryExist(List<Item> items)
        {
            foreach(var item in items)
            {
                if (item.Type == "Legendary")
                {
                    LegendaryItem = item;
                    return true;
                }
            }
            return false;
        }
        public void RemoveHint(Map map)
        {
            map.Circles.Clear();
            map.Pins.Clear();
        }
        private Position CreateCenterPosition()
        {
            var rnd = new Random();
            double rlon = rnd.Next(-Span, Span);
            double lon = rlon / 40000;
            double rlat = rnd.Next(-Span, Span);
            double lat = rlat / 40000;
            return new Position(LegendaryItem.Position.Latitude + lat, LegendaryItem.Position.Longitude + lon);
        }
        
        public void CreateCircle(Map map)
        {
            if(Span != 20)
                Span = Span - 20;
            else
            {

            }
            if (map.Circles.Count > 0)
                map.Circles.Clear();
            
            var Circle = new Circle()
            {
                Radius = Distance.FromMeters(Span * 4),
                Center = CreateCenterPosition(),
                StrokeColor = Color.FromHex("#88FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromHex("#88FFC0CB")
            };
            map.Circles.Add(Circle);
            HintExist = true;
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(LegendaryItem.Position.Latitude, LegendaryItem.Position.Longitude),
                Label = "Legendary Item"
            };
            map.Pins.Add(pin);
        }

    }
}
