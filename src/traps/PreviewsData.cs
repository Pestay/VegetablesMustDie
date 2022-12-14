using System.Collections.Generic;

namespace PreviewsData{

    public class Data{

        public struct PreviewProps{
            public string TexturePreview {get;}
            public string Scene {get;}

            public PreviewProps(string tex, string scn){
                TexturePreview = tex;
                Scene = scn;
            }
        }

        public Dictionary<string, PreviewProps> data = new Dictionary<string, PreviewProps>{
            { "WoodenTable" , new PreviewProps("res://src/traps/1x1block/table.png","res://src/traps/1x1block/WoodenBlock1x1.tscn")},
            {"Detergent" , new PreviewProps("res://src/traps/detergent/detergent.png", "res://src/traps/detergent/DetergentFloor.tscn")},
            {"Turret" , new PreviewProps("res://src/traps/turret/turret_simple.png", "res://src/traps/turret/Turret.tscn")},
            {"Spikes", new PreviewProps("res://src/traps/spikes/spikes.png", "res://src/traps/spikes/Spikes.tscn")},
        };



    }

}