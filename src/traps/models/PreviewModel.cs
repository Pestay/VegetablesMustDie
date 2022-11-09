using System.Collections.Generic;

namespace Models{

    public class PreviewModel{

        public string PreviewName {get; set; }
        public PreviewProps Props {get; set;}

    }

    public class PreviewProps{
        public string Texture {get ; set;}
        public string Scene {get ; set; }
    }

}