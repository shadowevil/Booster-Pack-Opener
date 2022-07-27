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
    public class MainMenuScreen : Screen
    {
        private Button SelectScreenButton;
        private Button DeckScreenButton;
        private Button OptionScreenButton;
        private Button ExitGameButton;
        private Texture2D ButtonBackground;

        public MainMenuScreen()
        {
            SelectScreenButton = new Button("Open booster packs", (GameComponent._mg.windowWidth - 250) / 2,
                ((GameComponent._mg.windowHeight - 30) / 2) - 100 + (50 * 0), 250, 30, 0.2f, 12.0f,
                new Action(delegate {
                GameComponent.transScreen = new TransitionScreen("SelectionScreen", new SelectionScreen());
            }));
            DeckScreenButton = new Button("Deck Editor", (GameComponent._mg.windowWidth - 250) / 2,
                ((GameComponent._mg.windowHeight - 30) / 2) - 100 + (50 * 1), 250, 30, 0.2f, 12.0f,
                new Action(delegate {
                GameComponent.transScreen = new TransitionScreen("DeckScreen", new DeckScreen());
            }));
            OptionScreenButton = new Button("Options", (GameComponent._mg.windowWidth - 250) / 2,
                ((GameComponent._mg.windowHeight - 30) / 2) - 100 + (50 * 2), 250, 30, 0.2f, 12.0f,
                new Action(delegate {
                GameComponent.transScreen = new TransitionScreen("MainMenuScreen", new MainMenuScreen());
            }));
            ExitGameButton = new Button("Exit", (GameComponent._mg.windowWidth - 250) / 2,
                ((GameComponent._mg.windowHeight - 30) / 2) - 100 + (50 * 3), 250, 30, 0.2f, 12.0f,
                new Action(delegate {
                GameComponent.transScreen = new TransitionScreen("ExitScreen", new ExitScreen());
            }));
            ButtonBackground = MonoGraphics.GetRoundedRectangle(300, 100 + (50 * 3), 0.5f);
        }

        public override void Dispose()
        {

        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            int height = 125 + (50 * 4);
            _mg.Draw(ButtonBackground, new Vector2((GameComponent._mg.windowWidth - 325) / 2, (GameComponent._mg.windowHeight - height) / 2), Color.Black * 0.75f);
            SelectScreenButton.Draw(gameTime, ref _mg);
            DeckScreenButton.Draw(gameTime, ref _mg);
            OptionScreenButton.Draw(gameTime, ref _mg);
            ExitGameButton.Draw(gameTime, ref _mg);
        }

        public override void Update(GameTime gameTime)
        {
            SelectScreenButton.Update(gameTime);
            DeckScreenButton.Update(gameTime);
            OptionScreenButton.Update(gameTime);
            ExitGameButton.Update(gameTime);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }

    public class ExitScreen : Screen
    {
        private mgTimer exitTimer;

        public ExitScreen()
        {
            exitTimer = new mgTimer(500.0);
        }

        public override void Dispose()
        {

        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if(exitTimer.hasTicked(gameTime))
            {
                winmain._gameWindow.Exit();
            }
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
