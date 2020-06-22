using Android.Graphics;
using GoAndFind.NewFolder;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Color = Xamarin.Forms.Color;

namespace GoAndFind.viewModel
{
    public class Hint
    {
        protected int Span { get; set; }
        public Circle CurrentCircle;
        public Position CreateCenterPosition(Item item)
        {
            var rnd = new Random();
            double rlon = rnd.Next(-Span, Span);
            double lon = rlon / 40000;
            double rlat = rnd.Next(-Span, Span);
            double lat = rlat / 40000;
            return new Position(item.Position.Latitude + lat, item.Position.Longitude + lon);
        }
       
        protected void CreateHint(Map map,Item Item, string Strokecolorfromhex,string FillColorFromHex,bool CircleExist)
        {
            if(CircleExist)
            {
                map.Circles.Remove(CurrentCircle);
            }
            var Circle = new Circle()
            {
                Radius = Distance.FromMeters(Span * 5),
                Center = CreateCenterPosition(Item),
                StrokeColor = Color.FromHex(Strokecolorfromhex),
                StrokeWidth = 8,
                FillColor = Color.FromHex(FillColorFromHex)
            };
            CurrentCircle = Circle;
            map.Circles.Add(Circle);
            var Pin = new Pin()
            {
                Type = PinType.Place,
                Label = "item"
            };
            map.Pins.Add(Pin);
        }

    }
}
