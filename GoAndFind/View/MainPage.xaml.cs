
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using GoAndFind.NewFolder;
using GoAndFind.viewModel;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Xamarin.Essentials;
using System.Threading;

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
        public List<Item> LegendaryItems { get; set; }
        private List<Position> All { get; set; }
        private Saving Saving = new Saving();

        public MainPage()
        {
            InitializeComponent();
            Player = Saving.LoadPlayer();
            Items = Saving.LoadItems();
            LegendaryItemHints = Saving.LoadLegendaryItemHints();
            BanditHints = Saving.LoadBanditHints();


            Navigator = new Navigation();
            All = new List<Position>();
            LegendaryItems = new List<Item>();
            Bandits = new List<Item>();

            ControAll();

            Healthammount.Text = Player.Health.ToString();
            BindingContext = Navigator;
            ChangeHealthammount(Player);
            SetMap();

            Player.Inventory.Add("Marker");
            Player.Inventory.Add("Ambush me");

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
        public void ChangeHealthammount(Player player)
        {
            player.Change += Player_Change;
        }

        private void Player_Change()
        {
            Healthammount.Text = Player.Health.ToString();
        }
        private void ControAll()
        {
            foreach(var item in Items)
            {
                All.Add(item.Position);
            }
            LegendaryControl();
            BanditCondtrol();
            ControlHints();
        }
        private void ControlHints()
        {
            foreach (var hint in LegendaryItemHints)
                Map.Circles.Add(hint.CurrentCircle);
            foreach (var hint in BanditHints)
                Map.Circles.Add(hint.CurrentCircle);
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
            BanditCondtrol();
            Saving.SaveItems(Items);
        }
        public void SpawnLegendaryItem()
        {
            var spawn = new Spawn();
            var position = spawn.PositionSpawn(Navigator.PlayerPosition);
            var legendaryItem = spawn.SpawnLegendaryItem(position);
            Items.Add(legendaryItem);
            All.Add(legendaryItem.Position);
            LegendaryItems.Add(legendaryItem);
            Saving.SaveItems(Items);
        }
        private void LegendaryControl()
        {
            foreach (var item in Items)
            {
                if (item.Type == "Legendary")
                {
                    LegendaryItems.Add(item);
                }
            }
        }
        private void BanditCondtrol()
        {
            foreach (var item in Items)
            {
                if (item.Type == "Bandit")
                {
                    Bandits.Add(item);
                }
            }
        }
        private async void GetStartet()
        {
            if (await Permissions.CheckStatusAsync<Permissions.LocationAlways>() != PermissionStatus.Granted)
                await Permissions.RequestAsync<Permissions.LocationAlways>();
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            var location = await locator.GetPositionAsync(new TimeSpan(0, 0, 1));

            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(0.1)));

            if (Items.Count == 0)
                SpawnAll(new Position(location.Latitude,location.Longitude));

            Navigator.Refreshlists(All);
            Navigator.Find();
            AutoSpawn(Navigator);
        }
        public void AutoSpawn(Navigation nav)
        {
            nav.Spawnew += () => SpawnNewItems(nav);
            DisplayAlert("spawn", "new items spawned", "ok");
        }
        public void SpawnNewItems(Navigation nav)
        {
            SpawnAll(nav.PlayerPosition);
            Navigator.Refreshlists(All);
        }
        public List<LegendaryItemHint> LegendaryItemHints;
        public List<Item> CloseLegendaryItems;
        public void FindCloseLegendaryItems()
        {
            CloseLegendaryItems = new List<Item>();
            foreach(var item in LegendaryItems)
            {
                if (Navigator.DistanceBetween(item.Position, Navigator.PlayerPosition) < 0.01)
                    CloseLegendaryItems.Add(item);
            }
        }
        public void CreateLegendaryHint()
        {
            FindCloseLegendaryItems();
            var rnd = new Random();
            var hintscount = LegendaryItemHints.Count;
            if (LegendaryItemHints.Count != 0)
            {
                foreach (var hint in LegendaryItemHints)
                {
                    if (rnd.Next(0, 100) > 10 && Navigator.DistanceBetween(hint.LegendaryItem.Position,Navigator.PlayerPosition) < 0.01)
                    {
                        if (hint.CloserCircle(Map))
                        {
                            DisplayAlert(null, "You found something more about " + hint.LegendaryItem.Name + " location", "ok");
                            Saving.SaveLegendaryHints(LegendaryItemHints);
                            Player.Inventory.Remove("Piece of map");
                            return;
                        }
                    }
                }
            }
            var lhint = new LegendaryItemHint(Map, CloseLegendaryItems, LegendaryItemHints);
            if (lhint.LegendaryHintExist)
            {
                LegendaryItemHints.Add(lhint);
            }
            else if (hintscount == LegendaryItemHints.Count)
            {
                DisplayAlert(null, "Nothing usefull here", "ok");
                SpawnLegendaryItem();
            }
            Player.Inventory.Remove("Piece of map");
            Saving.SaveLegendaryHints(LegendaryItemHints);
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
                LegendaryItems.Remove(collectedItem);
                Saving.SaveLegendaryHints(LegendaryItemHints);
            }
        }
        public List<BanditHint> BanditHints;
        public List<Item> Bandits;
        public void CreateBanditHint(Player player)
        {
            var rnd = new Random();
            int hintsCount = BanditHints.Count;
            bool closerhint = false;
            if (BanditHints.Count == 0 && Bandits.Count > 0)
                BanditHints.Add( new BanditHint(Map, Bandits, BanditHints));
            foreach (var bandit in Bandits)
            {
                if (rnd.Next(0, 100) < 10)
                {
                    if (BanditHints.Count < Bandits.Count)
                    {
                        var bantitHint = new BanditHint(Map, Bandits, BanditHints);
                        if (bantitHint.BanditHintExist)
                            BanditHints.Add(bantitHint);
                    }
                    foreach (var hint in BanditHints)
                    {
                        if (rnd.Next(0, 100) < 40)
                        {
                            hint.CloserCircle(Map);
                            closerhint = true;
                        }
                    }
                }
            }
            if(hintsCount == BanditHints.Count && !closerhint)
            {
                DisplayAlert(null, "Nothing usefull here", "ok");
            }
            else
            {
                DisplayAlert(null, "You found out where bandits could be", "ok");
            }
            Player.Inventory.Remove("Bandit letter");
            Saving.SaveBanditHints(BanditHints);
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
                Saving.SaveBanditHints(BanditHints);
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
        public async void ButtonOnClicked(object sender, EventArgs e)
        {

            Item item;
            if (Navigator.ItemIsClose)
            {
                item = ItemIs(Navigator.ClosestItem, Items);
                if (item.Type.Contains("Bandit"))
                {
                    var mg = new MiniGame();
                    Player = await mg.Ambush(item, Player);
                    
                }
                else if (item.Type.Contains("Bait"))
                {
                    await DisplayAlert("Nothing here", null, "OK");
                }
                else
                {
                    await DisplayAlert("Alert", "You collected " + item.Ammount + " " + item.Name, "OK");
                    for (int a = 0; a < item.Ammount; a++) 
                    {
                        Player.Inventory.Add(item.Name);
                        if(Player.Inventory.Contains("Dead man's sword"))
                        {
                            var rand = new Random();  
                            if (rand.Next(1, 3) == 3)
                                Player.Inventory.Add(item.Name);

                        }
                    }                       
                    RemoveLegendaryHint(LegendaryItemHints, item);
                }
                All.Remove(Navigator.ClosestItem);
                Navigator.Refreshlists(All);
                Navigator.FindClosest();
                Saving.SavePlayer(Player);
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
                    if (action == "Liquor" && answer)
                    {
                        Player.Heal(action, 1);

                    }
                    if  (action.ToLower() == "stolen dinner")
                    {
                        Player.SlowlyHeal(action);

                    }
                    if (action == "Armour")
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
                    if (action == "Piece of map")
                    {
                        CreateLegendaryHint();

                    }
                    if (action == "Bandit letter")
                    {
                        CreateBanditHint(Player);

                    }
                    if  (action == "Marker")
                    {
                        MarkItems();

                    }
                    if  (action == "Ambush me")
                    {
                        new MiniGame().Ambush(new Item(Navigator.PlayerPosition, "Bandit", "Causual Bandit", 2) , Player);

                    }
                    if (action == "Sleepy Heroic firefly") 
                    {
                        await DisplayAlert("firefly", "The firefly is sleeping, It'll wake up when it's time comes up.", "Ok");
                    }
                }
                Saving.SavePlayer(Player);
            }
        }
    } 
}
