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
            /*using (var filestream = File.Open(LegendaryHints, FileMode.Create, FileAccess.Write))
            {
                var serializer = new XmlSerializer(typeof(List<LegendaryItemHint>));
                serializer.Serialize(filestream, hints);
            }*/
        }
        public List<LegendaryItemHint> LoadLegendaryItemHints()
        {
            if (File.Exists(LegendaryHints))
            {
                using (var reader = new StreamReader(LegendaryHints))
                {
                    var serializer = new XmlSerializer(typeof(List<LegendaryItemHint>));
                    //return (List<LegendaryItemHint>)serializer.Deserialize(reader);
                }
            }
            return new List<LegendaryItemHint>();
        }



        public void SaveBanditHints(List<BanditHint> hints)
        {
            /*
            using (var filestream = File.Open(BanditHints, FileMode.Create, FileAccess.Write))
            {
                var serializer = new XmlSerializer(typeof(List<BanditHint>));
                serializer.Serialize(filestream, hints);
            }*/
        }
        public List<BanditHint> LoadBanditHints()
        {
            if (File.Exists(BanditHints))
            {
                using (var reader = new StreamReader(BanditHints))
                {
                    var serializer = new XmlSerializer(typeof(List<BanditHint>));
                    //return (List<BanditHint>)serializer.Deserialize(reader);
                }
            }
            return new List<BanditHint>();
        }
    }
}

