using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoosterPack
{
    public static class FramesPerSecond
    {
        public static double fps;
        private const double delay = 1000.0;
        private static int frameCounter = 0;
        public static double frameDelay = 0.0;
        public static float targetFrames = 244.0f;
        private static mgTimer frameTimer;
        private static bool isEnabled = false;

        public static void ToggleFrames()
        {
            isEnabled = !isEnabled;
        }

        public static void increaseFrame()
        {
            if (!isEnabled) return;
            frameCounter++;
        }

        public static void checkFrames(GameTime gameTime)
        {
            if (!isEnabled) return;
            if (frameTimer == null) frameTimer = new mgTimer(1000.0);
            if(frameTimer.hasTicked(gameTime))
            {
                fps = frameCounter;
                frameDelay = gameTime.ElapsedGameTime.TotalMilliseconds;
                frameCounter = 0;
            }
        }

        public static void DrawFrames(ref MonoGraphics _mg)
        {
            if (isEnabled)
            {
                Vector2 textVec = _mg.MeasureText("Frames: " + FramesPerSecond.fps + ":" + FramesPerSecond.frameDelay + "ms", 10.0f);
                _mg.DrawFilledRectangle(new Rectangle(0, 0, (int)textVec.X + 2, (int)textVec.Y + 2), Color.Black * 0.75f);
                _mg.DrawString("Frames: " + FramesPerSecond.fps + ":" + FramesPerSecond.frameDelay + "ms", 0, 0, Color.White, 10.0f);
            }
        }
    }
}
