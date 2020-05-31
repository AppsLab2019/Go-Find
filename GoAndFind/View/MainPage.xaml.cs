
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using GoAndFind.NewFolder;
using GoAndFind.hint;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace GoAndFind
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly Navigation Navigator;
        private Player Player;
        private List<Item> Items { get; set; }
        private List<Item> LegendaryItems { get; set; }
        private List<Position> All { get; set; }
        private Hint hint = new Hint();

        public MainPage()
        {
            InitializeComponent();


            Navigator = new Navigation();
            All = new List<Position>();
            Items = new List<Item>();
            LegendaryItems = new List<Item>();
            Bandits = new List<Item>();
            BanditHints = new List<BanditHint>();
            LegendaryItemHints = new List<LegendaryItemHint>();
            Player = new Player(3);
            hint = new Hint();
            Healthammount.Text = Player.Health.ToString();
            BindingContext = Navigator;
            ChangeHealthammount(Player);
            SetMap();

            Player.Inventory.Add("liquor");
            Player.Inventory.Add("liquor");
            Player.Inventory.Add("liquor");
            Player.Inventory.Add("Dead man's macaroni");
            Player.Inventory.Add("Erasing wand");
            Player.Inventory.Add("Hopefull stick of gloominess");
            Player.Inventory.Add("piece of map");
            Player.Inventory.Add("Bandit letter");

            GetStartet();
        }

        private void SetMap()
        {
            Map.MapType = MapType.Street;
            Map.MyLocationEnabled = true;

            var assembly = Assembly.GetExecutingAssembly();
            using var resource = assembly.GetManifestResourceStream("GoAndFind.mapstyle.json");
            using var reader = new StreamReader(resource);
            Map.UiSettings.ZoomControlsEnabled = false;
            //Map.UiSettings.ZoomGesturesEnabled = false;

            var json = reader.ReadToEnd();
            Map.MapStyle = MapStyle.FromJson(json);

           
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
            var all = new List<Position>();
            all = spawn.PositionsSpawn(8, new Position(location.Latitude, location.Longitude));
            foreach (var pos in all)
                All.Add(pos);
            var items = spawn.SpawnItems(All);
            foreach (var item in items)
                Items.Add(item);
            SpawnLegendaryItem();
            LegendaryControl();
            MarkItems();
        }
        public void SpawnLegendaryItem()
        {
            var spawn = new Spawn();
            var position = spawn.PositionSpawn(Navigator.PlayerPosition);
            var legendaryItem = spawn.SpawnLegendaryItem(position);
            Items.Add(legendaryItem);
            All.Add(legendaryItem.Position);
            LegendaryControl();
            BanditCondtrol();
        }
        private void LegendaryControl()
        {
            foreach(var item in Items)
            {
                if(item.Type == "Legendary")
                {
                    LegendaryItems.Add(item);
                }
            }
        }
        private void BanditCondtrol()
        {
            foreach(var item in Items)
            {
                if (item.Type == "Bandit")
                {
                    Bandits.Add(item);
                }
            }
        }
        private async void GetStartet()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            var location = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.1)));

            SpawnAll(new Position(location.Latitude, location.Longitude));
            SpawnAll(new Position(location.Latitude, location.Longitude));
            

            Navigator.Refreshlists(All);
            Navigator.Find();
            AutoSpawn(Navigator);
        }
        public void AutoSpawn(Navigation nav)
        {
            nav.Spawnew += () => SpawnNewItems(nav);
        }
        public void SpawnNewItems(Navigation nav)
        {
            SpawnAll(nav.PlayerPosition);
            Navigator.Refreshlists(All);
        }
        public List<LegendaryItemHint> LegendaryItemHints;
        public void CreateLegendaryHint(Player player)
        {
            var rnd = new Random();
            if (LegendaryItemHints.Count == 0)
            {
                LegendaryItemHints.Add(new LegendaryItemHint(Map, LegendaryItems,LegendaryItemHints));
                //player.Inventory.Remove("Piece of map");        
            }
            else
            {
                foreach (var hint in LegendaryItemHints)
                {
                    if (hint.CloserCircle(Map) && rnd.Next(0, 100) > 50)
                    {
                        return;
                        //player.Inventory.Remove("Piece of map");
                    }
                    else if (LegendaryItemHints.Count < LegendaryItems.Count)
                    {
                        var newHint = new LegendaryItemHint(Map, LegendaryItems, LegendaryItemHints);
                        if (newHint.LegendaryHintExist)
                        {
                            LegendaryItemHints.Add(newHint);
                            //player.Inventory.Remove("Piece of map");
                            return;
                        }
                        else 
                        {
                            SpawnLegendaryItem();
                            return;
                        }                    
                    }
                }
            }
        }
        public void MarkItems()
        {
            foreach(var item in Items)
            {
                var Pin = new Pin()
                {
                    Type = PinType.Place,
                    Position = item.Position,
                    Label = item.Name
                };
                Map.Pins.Add(Pin);
            }
        }
        public void RemoveLegendaryHint(List<LegendaryItemHint> legendaryHints,Item collectedItem)
        {
            if (collectedItem.Type == "Legendary")
            {
                foreach (var hint in legendaryHints)
                {
                    if (hint.LegendaryItem.Position == collectedItem.Position)
                    {
                        Map.Circles.Remove(hint.CurrentCircle);
                    }
                }
            }
        }
        public List<BanditHint> BanditHints;
        public List<Item> Bandits;
        public void CreateBanditHint(Player player)
        {
            var rnd = new Random();
            if(BanditHints.Count == 0)
                BanditHints.Add( new BanditHint(Map, Bandits, BanditHints));
            int hintsCount = BanditHints.Count;
            foreach (var bandit in Bandits)
            {
                if (rnd.Next(0, 100) < 20)
                {
                    if (BanditHints.Count < Bandits.Count)
                    {
                        BanditHints.Add(new BanditHint(Map, Bandits, BanditHints));
                    }
                    foreach (var hint in BanditHints)
                    {
                        if (rnd.Next(0, 100) < 40)
                            hint.CloserCircle(Map);
                    }
                }
            }
            if(hintsCount == BanditHints.Count)
            {
                DisplayAlert(null, "Nothing usefull here", "ok");
            }
            else
            {
                DisplayAlert(null, "You found out where bandits could be", "ok");
            }
        }
        public void RemoveBanditHint(List<BanditHint> bandithints,Item collectedItem)
        {
            if (collectedItem.Type == "Bandit")
            {
                foreach (var hint in bandithints)
                {
                    if (hint.Bandit.Position == collectedItem.Position)
                    {
                        Map.Circles.Remove(hint.CurrentCircle);
                    }
                }
            }
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
            if (Navigator.ItemIsClose)
            {
                item = ItemIs(Navigator.ClosestItem, Items);
                if (item.Type.Contains("Bandit"))
                {
                    Ambush(item);
                }
                else if (item.Type.Contains("Bait"))
                {
                    await DisplayAlert("You have been kidnaped", null, "OK");
                }
                else
                {
                    await DisplayAlert("Alert", "You collected " + item.Ammount + " " + item.Name, "OK");
                    for (int a = 0; a < item.Ammount; a++)
                        Player.Inventory.Add(item.Name);
                    RemoveLegendaryHint(LegendaryItemHints, item);
                }
                All.Remove(Navigator.ClosestItem);
                Navigator.Refreshlists(All);
                Navigator.FindClosest();
            }
        }
        public async void InventoryClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Inventory", "Cancel", null, Player.ShowInventory(Player));
            Debug.WriteLine("Action: " + action);

            if (action != "Cancel" && action != null)
            {
                action = action.Remove(0, 2);
                bool answer = await DisplayAlert("Question?", "Are you sure to use the " + action, "Yes", "No");
                if (answer)
                {
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
                        SpawnNewItems(Navigator);

                    }
                    if (action == "Dead man's macaroni")
                    {
                        Player.Heal(action, Player.MaxHealth - Player.Health);

                    }
                    if (action == "piece of map")
                    {
                        CreateLegendaryHint(Player);
                    }
                    if (action == "Bandit letter")
                    {
                        CreateBanditHint(Player);
                    }
                }
            }
        }
    } 
}
