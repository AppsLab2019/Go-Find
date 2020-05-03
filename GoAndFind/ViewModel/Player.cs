using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Timers;

namespace GoAndFind.ViewModel
{
    public class Player
    {
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public List<string> Inventory;
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
                Inventory.Clear();
        }
        public void Heal(string item)
        {
            if (Health >= MaxHealth)
                Health = MaxHealth;
            else
            {
                if (item == "liquor")
                {
                    if (Health >= MaxHealth)
                        Health = MaxHealth;
                    else
                    {
                        HealthChange(1, true);
                        Inventory.Remove(item);
                    }
                }
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
                Health++;
            }
        }
    }
}

