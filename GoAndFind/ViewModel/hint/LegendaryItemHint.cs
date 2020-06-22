using Android.Renderscripts;
using GoAndFind.NewFolder;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using Color = Xamarin.Forms.Color;
using Map = Xamarin.Forms.GoogleMaps.Map;

namespace GoAndFind.viewModel
{
    public class LegendaryItemHint : Hint
    {
        public bool LegendaryHintExist;
        public Item LegendaryItem;
        private string StrokeColourFromHex = "00000";
        private string FillColourFromHex = "88FFC00B";
        public LegendaryItemHint(Map map,List<Item> items,List<LegendaryItemHint> CurrentHints)
        {
            Span = 100;
            if (LegendaryExist(items,CurrentHints))
            {
                CreateHint(map, LegendaryItem, StrokeColourFromHex, FillColourFromHex,false);
            }
        }
        private bool LegendaryExist(List<Item> items, List<LegendaryItemHint> CurrentHints)
        {
            var busyitems = new List<Item>();
            foreach (var hint in CurrentHints)
            {
                busyitems.Add(hint.LegendaryItem);
            }
            foreach(var item in items)
            {
                if (!busyitems.Contains(item))
                {
                    LegendaryItem = item;
                    LegendaryHintExist = true;
                    return true;
                }
            }
            LegendaryHintExist = false;
            return false;
        }
        public bool CloserCircle(Map map)
        {
            if (Span > 30)
            {
                Span = Span - 25;
                CreateHint(map, LegendaryItem, StrokeColourFromHex, FillColourFromHex, true);
                return true;
            }
            else
                return false;
        }

    }
}
