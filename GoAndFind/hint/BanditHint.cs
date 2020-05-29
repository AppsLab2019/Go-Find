using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GoAndFind.hint
{
    public class BanditHint : Hint
    {
        public bool BanditHintExist;
        public Item Bandit;
        public BanditHint(Map map, List<Item> items, List<BanditHint> CurrentHints)
        {
            Span = 60;
            if (BanditExist(items, CurrentHints))
            {
                CreateHint(map, Bandit, "#88FF0000", "#88FFC0CB", false);
            }
        }
        private bool BanditExist(List<Item> bandits, List<BanditHint> CurrentHints)
        {
            var busyitems = new List<Item>();
            foreach (var hint in CurrentHints)
            {
                busyitems.Add(hint.Bandit);
            }
            foreach (var bandit in bandits)
            {
                if (!busyitems.Contains(bandit))
                {
                    Bandit = bandit;
                    BanditHintExist = true;
                    return true;
                }
            }
            BanditHintExist = false;
            return false;
        }
        public bool CloserCircle(Map map)
        {
            if (Span > 30)
            {
                Span = Span - 25;
                CreateHint(map, Bandit, "#88FF0000", "#88FFC0CB", true);
                return true;
            }
            else
                return false;
        }
    }
}
