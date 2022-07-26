using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoosterPack
{
    public abstract class Component
    {
        public Vector2 location;
        public int arrayPosition;
        public abstract void Draw(GameTime gameTime, ref MonoGraphics _mg);
        public abstract void Update(GameTime gameTime);
        public abstract void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue);
    }
}
