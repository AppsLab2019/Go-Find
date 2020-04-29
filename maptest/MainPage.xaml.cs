
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
using System.IO;

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

            Player.Inventory.Add("Firewater");
            Player.Inventory.Add("Firewater");
            Player.Inventory.Add("Firewater");
            Player.Inventory.Add("Armour");


            GetStartet();
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
                item = loot.Name;
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
        public void AutoSpawn(Navigation nav)
        {
            nav.Spawnew += () => SpawnAll(nav.PlayerPosition);
            nav.Spawnew += () => MarkItems(Items);
            nav.Spawnew += () => viewModel.Refreshlists(All);
        }
        public async void ButtonOnClicked(object sender, EventArgs e)
        {
            Item item;
            bool fight = false;
            if (viewModel.ItemIsClose)
            {
                item = ItemIs(viewModel.ClosestItem, Items);
                if (item.Type.Contains("Bandit"))
                {
                    await DisplayAlert("Alert", "You've been ambushed by " + item.Ammount + item.Name, "OK");
                    if (Player.Inventory.Contains("Firewater") && item.Name.Contains("Causual"))
                    {
                        fight = await DisplayAlert("Question?", "Those bandits look friendly, we may be friends", "Offer Firewater?", "Fight");
                    }
                    if (!fight)
                    {
                        if (item.Name.Contains("Veteran"))
                            Player.Hurt(item.Ammount * 2);
                        else
                            Player.Hurt(item.Ammount);
                    }
                    else
                        Player.Inventory.Remove(item.Name);
                }
                else
                {
                    await DisplayAlert("Alert", "You've collected " + item.Name, "OK");
                    Player.Inventory.Add(item.Name);
                }
                All.Remove(viewModel.ClosestItem);
                viewModel.Refreshlists(All);
                viewModel.FindClosest();
            }
        }
        public async void InventoryClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Inventory", "Cancel", null,ShowInventory(Player));
            Debug.WriteLine("Action: " + action);
            if (action != "Cancel")
            {
                action = action.Remove(0, 2);
                bool answer = await DisplayAlert("Question?", "Are you sure to use the " + action, "Yes", "No");
                if (action == "Firewater" && answer)
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
            var items = new List<string>();
            foreach (var item in player.Inventory)
            {
                int a = 0;
                foreach (var sameitem in player.Inventory)
                {
                    if (sameitem == item)
                        a++;
                }
                if(a != 0 && !items.Contains(a + " " + item))
                    items.Add(a + " " + item);
            }
            return items.ToArray();
        }
        public void GameUpgrade()
        {
            if(Player.Inventory.Contains("Meč hrdlorez"))
            {
                if(Player.Inventory.Contains("Palička nádeje"))
                {
                    if (Player.Inventory.Contains("Kniha múdrostí"))
                    {
                        DisplayAlert("Alert","You've collected all legendary items, now let the game upgrade" , "ok");
                        //SpawnNewItems
                    }
                }
            }
        }
    }
}
