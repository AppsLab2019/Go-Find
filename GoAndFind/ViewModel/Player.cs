using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms.GoogleMaps;
using System.Timers;

namespace GoAndFind.hint
{
    public class Player
    {
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public List<string> Inventory { get; set; }
        public Player(int health)
        {
            Inventory = new List<string>();
            Health = health;
            MaxHealth = health;
        }
        public void Hurt(int damage)
        {
            HealthChange(damage, false);
            if (Health <= 0)
            {
                if (Inventory.Contains("Dead man's macaroni"))
                {
                    App.Current.MainPage.DisplayAlert(null, "You are going to die, but there's a light of hope, you can use Dead man's macaroni to stay alive.", "use it");
                    HealthChange(MaxHealth, true);
                    Inventory.Remove("Dead man's macaroni");
                }
                else
                {
                    App.Current.MainPage.DisplayAlert(null, "You died, your inventory is now clear, good luck next time", "ok");
                    Inventory.Clear();
                }
            }
        }
        public void Heal(string item, int ammount)
        {
            if (Health >= MaxHealth)
                Health = MaxHealth;
            else
            {
                HealthChange(ammount, true);
                Inventory.Remove(item);
            }
        }

        public void HealthChange(int ammount, bool heal)
        {
            if (heal)
                Health += ammount;
            else
                Health += -ammount;
            Change();
        }
        public delegate void HealthChanged();
        public event HealthChanged Change;

        public void PlayerUpgrade(string item)
        {
            if (item == "armour")
            {
                MaxHealth++;
                HealthChange(1, true);
                Inventory.Remove(item);
            }
        }
        public string[] ShowInventory(Player player)
        {
            var items = new List<string>();
            foreach (var item in player.Inventory)
            {
                int a = 0;
                foreach (var sameitem in player.Inventory)
                {
                    if (sameitem == item)
                        a++;
                }
                if (a != 0 && !items.Contains(a + " " + item))
                    items.Add(a + " " + item);
            }
            return items.ToArray();
        } 
    }
}

