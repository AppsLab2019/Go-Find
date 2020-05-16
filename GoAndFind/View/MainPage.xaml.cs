
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using GoAndFind.NewFolder;
using GoAndFind.ViewModel;
using System.Diagnostics;
using System.Reflection;

namespace GoAndFind
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
            ChangeHealthammount(Player);
            SetMap();

            Player.Inventory.Add("liquor");
            Player.Inventory.Add("liquor");
            Player.Inventory.Add("liquor");
            Player.Inventory.Add("Dead man's macaroni");
            Player.Inventory.Add("Erasing wand");
            Player.Inventory.Add("Hopefull stick of gloominess");

            GetStartet();
        }

        private void SetMap()
        {
            Map.MapType = MapType.Street;
            Map.MyLocationEnabled = true;
            
        }
        /*public void SaveInventory()
        {
            File.WriteAllLines("Inventory", Player.Inventory.ToArray());
        }
        public void LoadInventory()
        {
            string[] inventory = File.ReadAllLines("Inventory");
            foreach (var item in inventory)
            {
                Player.Inventory.Add(item);
            }
        }*/
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

            var location = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.444));
            Map.MoveToRegion(mapSpan);

            SpawnAll(new Position(location.Latitude, location.Longitude));


            viewModel.Refreshlists(All);
            viewModel.Find();
            AutoSpawn(viewModel);
        }
        public void AutoSpawn(Navigation nav)
        {
            nav.Spawnew += () => SpawnNewItems(nav);
        }
        public void SpawnNewItems(Navigation nav)
        {
            SpawnAll(nav.PlayerPosition);
            viewModel.Refreshlists(All);
        }
        public async void Ambush(Item item)
        {
            bool friend = false;
            var fight = new MiniGame();
            await DisplayAlert("Watchout!", "You are ambushed by " + item.Ammount + " " + item.Name, "OK");
            if (Player.Inventory.Contains("liquor") && item.Name.Contains("Causual"))
            {
                friend = await DisplayAlert("Question?", "Those bandits look friendly, we may be friends", "Offer liquor", "Fight");
            }
            if (!friend)
            {
                for (int a = 0; a < item.Ammount; a++)
                {
                    await fight.Fight(this, item.Name, Player);
                    if (fight.ErasingWandUsed)
                        Player.Inventory.Remove("Erasing wand");
                    if (!fight.Win)
                    {
                        Player.Hurt(1);
                    }
                }
            }
            else
                Player.Inventory.Remove("liquor");
        }
        public Item ItemIs(Position position, List<Item> items)
        {
            Item itemis = null;
            foreach (var item in items)
            {
                if (item.Position == position)
                    itemis = item;
            }
            return itemis;
        }
        public async void ButtonOnClicked(object sender, EventArgs e)
        {
            Item item;
            if (viewModel.ItemIsClose)
            {
                item = ItemIs(viewModel.ClosestItem, Items);
                if (item.Type.Contains("Bandit"))
                {
                    Ambush(item);
                }
                else
                {
                    await DisplayAlert("Alert", "You collected " + item.Ammount + " " + item.Name, "OK");
                    for (int a = 0; a < item.Ammount; a++)
                        Player.Inventory.Add(item.Name);
                }
                All.Remove(viewModel.ClosestItem);
                viewModel.Refreshlists(All);
                viewModel.FindClosest();
            }
        }
        public async void InventoryClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Inventory", "Cancel", null, Player.ShowInventory(Player));
            Debug.WriteLine("Action: " + action);
            if (action != "Cancel")
            {
                action = action.Remove(0, 2);
                bool answer = await DisplayAlert("Question?", "Are you sure to use the " + action, "Yes", "No");
                if (action == "liquor" && answer)
                {
                    Player.Heal(action, 1);
                }
                if (action == "armour")
                {
                    Player.PlayerUpgrade(action);
                }
                if (action == "Hopefull stick of gloominess")
                {
                    SpawnNewItems(viewModel);
                    Player.Inventory.Remove(action);
                }
                if (action == "Dead man's macaroni")
                {
                    Player.Heal(action, Player.MaxHealth - Player.Health);
                    Player.Inventory.Remove(action);
                }
                if (action == "piece of map")
                {

                    Player.Inventory.Remove(action);
                }
            }
        }
    } 
}
