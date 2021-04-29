using System;

namespace MinecraftBorderExpansion {
    public class Item
    {
        public string Name { get; set; }
        public int EMC { get; set; }
        public float Expansion { get; set; }
        public int Amount { get; set; }

        public float Efficacy => Math.Max(MathF.Pow(0.98f, MathF.Floor(Expansion / 5f)), .01f);

        public Item(string name, int emc) {
            Name = name;
            EMC = emc;
        }
    }
}
