using Microsoft.Xna.Framework;
using System;

namespace BoosterPack
{
    public abstract class Screen : Component, IDisposable
    {
        public abstract override void Draw(GameTime gameTime, ref MonoGraphics _mg);

        public abstract override void Update(GameTime gameTime);

        public abstract void Dispose();
    }
}
