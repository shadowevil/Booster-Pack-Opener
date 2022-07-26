using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitmap = System.Drawing.Bitmap;
using BitmapData = System.Drawing.Imaging.BitmapData;

namespace BoosterPack
{
    public class Sprite : Component
    {
        public Texture2D texture;
        public Rectangle Bounds;
        public Rectangle sourceRect;

        public Sprite(Texture2D texture, Rectangle rect)
        {
            this.texture = texture;
            Bounds = rect;
            sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            _mg.Draw(texture, Bounds, sourceRect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public static Texture2D GetTexture(string path)
        {
            Texture2D texture;
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                texture = Texture2D.FromStream(GameComponent._mg._graphics, fileStream);
            }
            return texture;
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }

    public class AnimatedSprite : Component
    {
        public List<Texture2D> textures;
        public mgTimer frameDelay;

        public AnimatedSprite(List<Texture2D> textures, mgTimer frameDelay)
        {
            this.textures = textures;
            this.frameDelay = frameDelay;
        }

        public static void LoadAnimatedSprite()
        {

        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
