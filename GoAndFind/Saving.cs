 
    using GoAndFind.viewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Essentials;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Android.SE.Omapi;

namespace GoAndFind
{
    class Saving
    {
        private static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        //private static string Inventory = System.IO.Path.Combine(path, "Inventory");
        private static string Player = Path.Combine(path, "Player");
        private static string Items = Path.Combine(path, "Items");
        private static string LegendaryHints = Path.Combine(path, "LegendaryHints");
        private static string BanditHints = Path.Combine(path, "BanditHints");



        public void SavePlayer(Player player)
        {
            File.Delete(Player);
            {
                using (StreamWriter reader = new StreamWriter(Player))
                {
                    var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                    jsonSerializer.Serialize(reader, player);
                }
            }
        }
        public Player LoadPlayer()
        {
            if (File.Exists(Player))
            {
                using (var reader = new StreamReader(Player))
                {
                    var jsonString = File.ReadAllText(Player);
                    return JsonConvert.DeserializeObject<Player>(jsonString);
                }
            }
            return new Player(3,3);
        }



        public void SaveItems(List<Item> items)
        {
            File.Delete(Items);
            {
                using (StreamWriter reader = new StreamWriter(Items))
                {
                    var jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(reader, items);
                }
            }
        }
        public List<Item> LoadItems()
        {
            if (File.Exists(Items))
            {
                using (var reader = new StreamReader(Items))
                {
                    var jsonString = File.ReadAllText(Items);
                    return JsonConvert.DeserializeObject<List<Item>>(jsonString);
                }
            }
            return new List<Item>();
        }


        public void SaveLegendaryHints(List<LegendaryItemHint> hints)
        {
            //File.Delete(LegendaryHints);
            {
                /*using (StreamWriter reader = new StreamWriter(LegendaryHints))
                {
                    var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                    jsonSerializer.Serialize(reader, hints);
                }*/
            }
        }
        public List<LegendaryItemHint> LoadLegendaryItemHints()
        {
           if (File.Exists(LegendaryHints))
            {
                /*using (var reader = new StreamReader(LegendaryHints))
                {
                    var jsonString = File.ReadAllText(LegendaryHints);
                    return JsonConvert.DeserializeObject<List<LegendaryItemHint>>(jsonString);
                }*/
            }
            return new List<LegendaryItemHint>();
        }



        public void SaveBanditHints(List<BanditHint> hints)
        {
            /*File.Delete(BanditHints);
            {
                using (StreamWriter reader = new StreamWriter(BanditHints))
                {
                    var jsonSerializer = new Newtonsoft.Json.JsonSerializer();
                    jsonSerializer.Serialize(reader, hints);
                }
            }*/
        }
        public List<BanditHint> LoadBanditHints()
        {
            /*if (File.Exists(BanditHints))
            {
                using (var reader = new StreamReader(BanditHints))
                {
                    var jsonString = File.ReadAllText(BanditHints);
                    return JsonConvert.DeserializeObject<List<BanditHint>>(jsonString);
                }
            }*/
            return new List<BanditHint>();
        }
    }
}

