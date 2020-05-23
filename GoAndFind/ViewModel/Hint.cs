﻿using Android.Graphics;
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
        private int Span = 80;
        public bool HintExist { get; set; }
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
        public void CreateHint(Map map)
        {
            if (map.Circles.Count > 0)
                map.Circles.Clear();
            var Circle = new Circle()
            {
                Radius = Distance.FromMeters(Span * 3),
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