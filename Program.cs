using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinecraftBorderExpansion {
    public class Program {
        static FileModel data;
        const string DataFile = @".\Data.json";

        public static void Main(string[] args) {
            LoadData();
            Console.WriteLine("Loaded data");
            Console.WriteLine("Current worldborder: " + data.BorderSize);

            while (true) {
                string input = Console.ReadLine();
                string[] arg = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (arg.Length == 0) {
                    Console.WriteLine($"Current worldborder: {data.BorderSize}");
                }
                else if (arg.Length == 1) {
                    arg[0] = arg[0].ToLower();
                    if (data.Items.ContainsKey(arg[0])) {
                        var item = data.Items[arg[0]];
                        Console.WriteLine($"  Total contributed: {item.Amount}");
                        Console.WriteLine($"  Border expansion: {item.Expansion}");
                        Console.WriteLine($"  Current efficacy: {(item.Efficacy * 100).ToString("0.0")}%");
                    }
                    else {
                        Console.WriteLine("Unknown Item");
                    }
                    continue;
                }
                else if (arg.Length == 2) {
                    string itemName = arg[0].ToLower();
                    string itemAmountStr = arg[1];
                    if (int.TryParse(itemAmountStr, out int itemAmount)) {
                        if (!data.Items.ContainsKey(itemName)) {
                            Console.WriteLine("How much EMC is this item worth?");
                            string emcInput = Console.ReadLine();
                            if (int.TryParse(emcInput, out int emcAmount)) {
                                data.Items.Add(itemName, new Item(itemName, emcAmount));
                            }
                            else {
                                Console.WriteLine("Invalid integer provided. Cancelling item add");
                                continue;
                            }
                        }
                        AddItem(itemName, itemAmount);
                    }
                    else {
                        Console.WriteLine("Please provide an integer for number of items");
                        continue;
                    }
                }
                else {
                    Console.WriteLine("Unrecognized command. Please use one of the following:");
                    Console.WriteLine("  itemName");
                    Console.WriteLine("  itemName amount");
                }
            }
        }

        static void LoadData() {
            if (File.Exists(DataFile)) {
                using StreamReader file = File.OpenText(DataFile);
                using JsonTextReader reader = new JsonTextReader(file);
                JObject o2 = (JObject) JToken.ReadFrom(reader);
                data = o2.ToObject<FileModel>();
            }
            else {
                data = new FileModel();
            }
        }

        static void SaveData() {
            using StreamWriter file = File.CreateText(DataFile);
            using JsonTextWriter writer = new JsonTextWriter(file);
            JToken.FromObject(data).WriteTo(writer);
        }

        static void AddItem(string itemName, int amount) {
            float totalExpansion = 0;
            var item = data.Items[itemName];
            for (int i = 0; i < amount; i++) {
                if (item.EMC < 0) {
                    totalExpansion += item.EMC / 1000;
                }
                else {
                    float emc = item.EMC * item.Efficacy;
                    float expansionDelta = emc / 1000;
                    totalExpansion += expansionDelta;
                    item.Expansion += expansionDelta;
                }
            }
            item.Amount += amount;
            data.BorderSize += totalExpansion;
            Console.WriteLine($"New border size: {data.BorderSize} ({(totalExpansion >= 0 ? "+" : "")}{totalExpansion})");

            SaveData();
        }
    }
}
