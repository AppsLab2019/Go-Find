﻿using System;
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

            BindingContext = viewModel;
            map.MapType = MapType.Street;
            map.IsShowingUser = true;

            //health.Text = player.Health.ToString();
            GetStartet();
        }

        public void SpawnAll(Position location)
        {
            var spawn = new Spawn();
            All = spawn.PositionsSpawn(5, new Position(location.Latitude, location.Longitude));
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
            string item;
            foreach (var loot in items.ToList())
            {
                if (loot.Type == "Frndžalica")
                    item = "Frndžalica";
                else
                    item = "Bandit";
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
        }
        public void ButtonOnClicked(object sender, EventArgs e)
        {
            string item;
            if (viewModel.ItemIsClose)
            {
                item = ItemIs(viewModel.ClosestItem, Items);
                if (item == "Bandit")
                {
                    DisplayAlert("Alert", "You have been ambushed", "OK");
                    Player.Health--;
                }
                else
                    DisplayAlert("Alert", "You have collected " + item, "OK");
                All.Remove(viewModel.ClosestItem);
                viewModel.Refreshlists(All);
                viewModel.FindClosest();
            }
        }
        public async void InventoryClicked(object sender, EventArgs e)
        {
            
            string action = await DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, Player.Inventory.ToArray());
            Debug.WriteLine("Action: " + action);
            if (action != "Cancel")
            {
                bool answer = await DisplayAlert("Question?", "Are you sure to use the item", "Yes", "No");
                if (action == "First aid")
                {
                    Player.Health++;
                }
            }
        }
    }
}
