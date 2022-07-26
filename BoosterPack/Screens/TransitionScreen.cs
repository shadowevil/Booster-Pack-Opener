using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoosterPack
{
    public class TransitionScreen : Screen
    {
        public enum Direction : uint
        {
            IN = 0,
            OUT = 1
        };

        private Rectangle rect;
        private float opacity = 1.0f;
        private float rateOfChange = 0.01f;
        private mgTimer timer;
        private Direction dir;

        private string new_screen;
        private BoosterPack selectedPack;
        private Screen screen;

        public bool finished = false;

        public TransitionScreen(Direction direction, string new_screen, BoosterPack selectedPack, Screen screen)
        {
            this.dir = direction;
            if (dir == Direction.IN) opacity = 1.0f;
            if (dir == Direction.OUT) opacity = 0.0f;
            rect = new Rectangle(0, 0, GameComponent._mg.windowWidth, GameComponent._mg.windowHeight);
            timer = new mgTimer(0.15);
            this.new_screen = new_screen;
            this.selectedPack = selectedPack;
            this.screen = screen;
        }

        public override void Dispose()
        {

        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            _mg.DrawFilledRectangle(rect, Color.Black * opacity);
        }

        public override void Update(GameTime gameTime)
        {
            if (timer.hasTicked(gameTime))
            {
                if (dir == Direction.IN)
                {
                    opacity -= rateOfChange;
                    if(opacity < 0.0f)
                    {
                        finished = true;
                    }
                }
                if (dir == Direction.OUT)
                {
                    opacity += rateOfChange;

                    if (opacity >= 1.0f)
                    {
                        ChangeScreensNow();
                        dir = Direction.IN;
                    }
                }
            }
        }

        public void ChangeScreensNow()
        {
            GameComponent.changeScreens(new_screen, selectedPack, screen);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
