using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace maptest.ViewModel
{
    class Item
    {
        public Position Position { get; set; }
        public string Type;
    }
    class Bandit : Item
    {
        public Bandit(Position position)
        {
            Position = position;
            Type = "Bandit";
        }
    }
    class Frndžalica : Item
    {
        public Frndžalica(Position position)
        {
            Position = position;
            Type = "Frndžalica";
        }
    }
    class OXYDŽEM : Item
    {
        public OXYDŽEM(Position position)
        {
            Position = position;
            Type = "OXYDŽEM";
        }
    }
    class BAŠTASMASLEM : Item
    {
        public BAŠTASMASLEM(Position position) 
        {
            Position = position;
            Type = "BAŠTASMASLEM";
        }
    }
}
