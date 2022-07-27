using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BoosterPack
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initalizing the MonoGame window and specifying the window name
            using (winmain s = new winmain("Booster Pack Opener"))
            {
                // Actually running the window
                s.Run();
            }
        }
    }

    public class winmain : Game
    {
        public static Game _gameWindow;

        private MonoGraphics _mg;
        public static GameComponent _gameComponent;
        private List<Component> _controls;
        public static MouseHandle _mouse;

        private GameTime _gameTime;

        public winmain(string windowTitle)
        {
            // Specifying the MonoGraphics library for ease of use
            _mg = new MonoGraphics(windowTitle, 1920, 1080, this);

            this.IsMouseVisible = true;
            
            // Setting our target frames
            this.TargetElapsedTime = TimeSpan.FromTicks((long)(TimeSpan.TicksPerSecond / FramesPerSecond.targetFrames));
            this.IsFixedTimeStep = true;
            
            // Getting a global variable for anything that
            //   may need access to the game window directly
            _gameWindow = this;

            this.Window.KeyDown += Window_KeyDown;
            this.Window.KeyUp += Window_KeyUp;
        }

        private void Window_KeyUp(object sender, InputKeyEventArgs e)
        {
            foreach (Component c in _controls)
                c.UpdateKeyInput(_gameTime, e.Key, false);
        }

        private void Window_KeyDown(object sender, InputKeyEventArgs e)
        {
            foreach (Component c in _controls)
                c.UpdateKeyInput(_gameTime, e.Key, true);
        }

        protected override void Initialize()
        {
            // Setting if the window is fullscreen and/or window size
            _mg.setResolution(false);
            
            // Getting the MouseHandle for some nicer framework
            _mouse = new MouseHandle();

            // Loading all the fonts we need
            _mg.LoadFonts();

            // Creating our game components for ease of drawing and updating
            _gameComponent = new GameComponent(ref _mg, ref _mouse);
            
            // Populating the control list with our gameComponent
            _controls = new List<Component>()
            {
                _gameComponent
            };

            // Toggling the frames on or off (using the function again will turn it off)
            FramesPerSecond.ToggleFrames();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Load our Game Components
            _gameComponent.Loading();
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // When window is unloading, unload our components
            _gameComponent.Dispose();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Delay the actual updating of the game until the window is fully displayed
            if (gameTime.TotalGameTime.TotalSeconds >= 3)
            {
                // Global game time
                _gameTime = gameTime;
                // Always update the mouse before doing anything
                _mouse.UpdateMouse();

                // Check and update our frames
                FramesPerSecond.checkFrames(gameTime);
                
                // Actually updating all of our controls/components we may want.
                foreach (Component c in _controls)
                    c.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Delay the actual drawing of the game until the window is fully displayed
            if (gameTime.TotalGameTime.TotalSeconds >= 3)
            {
                _mg._graphics.Clear(Color.Black);
                
                // Begin and initalize the drawing phase.
                _mg.BeginSpriteBatch();
                _mg._graphics.SetRenderTarget(null);
                // Draw all components (draw order is based on what position it is in the code)
                foreach (Component c in _controls)
                    c.Draw(gameTime, ref _mg);

                // Draw our frames if drawing frames are toggled
                FramesPerSecond.DrawFrames(ref _mg);

                // End the drawing phase and present it to the window
                _mg.EndSpriteBatch();
                
                // Increase the frame count as we have finished the frame
                FramesPerSecond.increaseFrame();
            }
            base.Draw(gameTime);
        }
    }
}
