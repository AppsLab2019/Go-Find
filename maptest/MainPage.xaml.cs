﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using maptest.NewFolder;
using maptest.ViewModel;
using System.Threading;
using System.Diagnostics;

namespace maptest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly Navigation viewModel;
        private Player Player;
        private List<Item> Items { get; set; }
        private List<Position> All { get; set; }

        public MainPage()
        {
            InitializeComponent();

            viewModel = new Navigation();
            All = new List<Position>();
            Items = new List<Item>();
            Player = new Player(3);
            Healthammount.Text = Player.Health.ToString();
            BindingContext = viewModel;
            map.MapType = MapType.Street;
            map.IsShowingUser = true;
            ChangeHealthammount(Player);

            Player.Inventory.Add("Frndžalica");
            Player.Inventory.Add("Frndžalica");
            Player.Inventory.Add("Frndžalica");
         
            GetStartet();
        }
        public void ChangeHealthammount(Player player)
        {
            player.Change += Player_Change;
        }

        private void Player_Change()
        {
            Healthammount.Text = Player.Health.ToString();
        }

        public void SpawnAll(Position location)
        {
            var spawn = new Spawn();
            All = spawn.PositionsSpawn(8, new Position(location.Latitude, location.Longitude));
            Items = spawn.SpawnItems(All);
        }
        private async void GetStartet()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            var task = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            var location = task;

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.444));
            map.MoveToRegion(mapSpan);

            SpawnAll(new Position(location.Latitude, location.Longitude));


            MarkItems(Items);
            viewModel.Refreshlists(All);
            viewModel.Find();
            AutoSpawn(viewModel);
        }
        public void MarkItems(List<Item> items)
        {
            string item = "Item";
            foreach (var loot in items.ToList())
            {
                if (loot.Type == "Frndžalica")
                    item = "Frndžalica";
                else if (loot.Type == "EasyBandit")
                    item = "EasyBandit";
                else if (loot.Type == "MediumBandit")
                    item = "MediumBandit";
                else if (loot.Type == "Armour")
                    item = "Armour";
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (Items.Contains(loot))
                        map.Pins.Add(new Pin
                        {
                            Label = item,
                            Position = new Position(loot.Position.Latitude, loot.Position.Longitude),
                        }); ;
                });
            }
        }
        public string ItemIs(Position position, List<Item> items)
        {
            string itemis = "";
            foreach (var item in items)
            {               
                if (item.Position == position)
                    itemis = item.Type;
            }
            return itemis;
        }
        public void AutoSpawn(Navigation nav)
        {
            nav.Spawnew += () => SpawnAll(nav.PlayerPosition);
            nav.Spawnew += () => MarkItems(Items);
            nav.Spawnew += () => viewModel.Refreshlists(All);
        }
        public async void ButtonOnClicked(object sender, EventArgs e)
        {
            string item;
            bool fight = true;
            if (viewModel.ItemIsClose)
            {
                item = ItemIs(viewModel.ClosestItem, Items);
                if (item.Contains("Bandit"))
                {
                    await DisplayAlert("Alert", "You've been ambushed", "OK");
                    if (Player.Inventory.Contains("Frndžalica") && item.Contains("Easy"))
                    {
                        fight = await DisplayAlert("Question?", "Those bandits look friendly, we may be friends", "Offer Frndžalica?", "Fight");
                    }
                    if(fight)
                    Player.Hurt(1);
                }
                else
                {
                    await DisplayAlert("Alert", "You've collected " + item, "OK");
                    Player.Inventory.Add(item);
                }
                All.Remove(viewModel.ClosestItem);
                viewModel.Refreshlists(All);
                viewModel.FindClosest();
            }
        }
        public async void InventoryClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null,ShowInventory(Player));
            Debug.WriteLine("Action: " + action);
            if (action != "Cancel")
            {
                bool answer = await DisplayAlert("Question?", "Are you sure to use the " + action, "Yes", "No");
                if (action == "Frndžalica" && answer)
                {
                    Player.Heal(action);
                }
                if (action == "Armour")
                {
                    Player.PlayerUpgrade(action);
                }

            }
        }
        public string[] ShowInventory(Player player)
        {
            string[] items;
            var Frndzalica = 0;
            var Armour = 0;
            foreach(var item in player.Inventory)
            {
                if (item == "Frndžalica")
                    Frndzalica++;
                if (item == "Armour")
                    Armour++;
            }
            items = new string[] { Frndzalica.ToString() + " Frndžalica" , Armour.ToString() + " Armour" };
            return items;
        }
    }
}
