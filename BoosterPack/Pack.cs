using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitmap = System.Drawing.Bitmap;
using Size = System.Drawing.Size;

namespace BoosterPack
{
    public class BoosterPack : Pack
    {
        public Texture2D packIcon;
        public float packIconScale = 0.35f;
        public Texture2D packPreview;
        public float packPreviewScale = 0.75f;
        public Rectangle previewSourceRect;
        public Rectangle iconSourceRect;

        public Dictionary<string, Texture2D> cards_Common;
        public Dictionary<string, Texture2D> cards_Rare;
        public Dictionary<string, Texture2D> cards_SuperRare;
        public Dictionary<string, Texture2D> cards_UltraRare;

        public void Preload(string name)
        {
            string path = "Data\\booster\\" + name + ".png";
            if (!File.Exists(path)) return;

            packPreview = Sprite.GetTexture(path);
            packIcon = Sprite.GetTexture(path);

            previewSourceRect = new Rectangle(0, 0, packPreview.Width, packPreview.Height);
            iconSourceRect = new Rectangle(0, 0, packIcon.Width, packIcon.Height);

            cards_Common = new Dictionary<string, Texture2D>();
            cards_Rare = new Dictionary<string, Texture2D>();
            cards_SuperRare = new Dictionary<string, Texture2D>();
            cards_UltraRare = new Dictionary<string, Texture2D>();
        }
    }

    public class Pack
    {
        public List<string> name { get; set; }
        public string description { get; set; }
        public string release { get; set; }

        [JsonProperty("#cards")]
        public int Cards { get; set; }
        public List<string> Common { get; set; }
        public List<string> Rare { get; set; }

        [JsonProperty("Super Rare")]
        public List<string> SuperRare { get; set; }

        [JsonProperty("Ultra Rare")]
        public List<string> UltraRare { get; set; }

        [JsonProperty("%R")]
        public int R { get; set; }

        [JsonProperty("%SR")]
        public int SR { get; set; }

        [JsonProperty("%UR")]
        public int UR { get; set; }
    }
}
