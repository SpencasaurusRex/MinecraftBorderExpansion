using System.Collections.Generic;

namespace MinecraftBorderExpansion {
    public class FileModel {
        public Dictionary<string, Item> Items { get; set; }
        public float BorderSize;

        public FileModel() {
            BorderSize = 100;
            Items = new Dictionary<string, Item>();
        }
    }
}
