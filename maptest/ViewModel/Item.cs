using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace maptest.ViewModel
{
    public class Item
    {
        public Position Position { get; set; }
        public string Type;
        public Item(Position position, string type,string name)
        {
            Position = position;
            Type = type;
        }
    }
}
