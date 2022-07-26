using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Bitmap = System.Drawing.Bitmap;
using Newtonsoft.Json;
using System.IO;

namespace BoosterPack
{
    public class GameComponent : Component, IDisposable
    {
        public static SoundEffect seDraw;
        public static SoundEffect seAttack;
        public static SoundEffect seActivate;
        public static SoundEffect seSummon;
        public static MonoGraphics _mg;
        private MouseHandle _mouse;
        public static Dictionary<string, Component> _controls;
        public static Dictionary<string, Component> _ControlsToAdd;
        public static List<string> _ControlsToRemove;
        public static TransitionScreen transScreen;

        public static string current_screen = "SelectionScreen";

        public static Dictionary<string, BoosterPack> Boosters;
        public static BoosterPack ChosenBooster;

        public static DataClass cardsdb;
        public static DataClass cardRarityDB;

        public static bool defaultScreenView = false;

        public GameComponent(ref MonoGraphics mg, ref MouseHandle _m)
        {
            _mg = mg;
            _mouse = _m;
            _controls = new Dictionary<string, Component>();
            Boosters = new Dictionary<string, BoosterPack>();
            _ControlsToAdd = new Dictionary<string, Component>();
            _ControlsToRemove = new List<string>();
        }

        public void Dispose()
        {
            // Close the connection to the database
            cardsdb.Close();
        }

        public void Loading()
        {
            // Added new loading screen to make it Visually more appealing
            changeScreens("LoadingScreen", null, new LoadingScreen());
        }

        public override void Update(GameTime gameTime)
        {
            if (transScreen != null)
            {
                if (!transScreen.finished)
                    transScreen.Update(gameTime);
                else transScreen = null;
            }

            // Update all controls that are the Screen
            foreach (KeyValuePair<string, Component> c in _controls)
            {
                if (c.Key.Contains("Screen")) continue;
                c.Value.Update(gameTime);
            }

            // Update all controls that are not the Screen (i.e buttons and images)
            foreach (KeyValuePair<string, Component> c in _controls)
            {
                if (!c.Key.Contains("Screen")) continue;
                if (c.Key == current_screen)
                    c.Value.Update(gameTime);
            }

            // Since you can't remove controls while in a loop, this is a hacky-fix on removing controls
            foreach(string s in _ControlsToRemove)
            {
                _controls.Remove(s);
            }
            // Same as above just adding controls, we always want to remove controls before we add as it could cause duplicates if we don't.
            foreach(KeyValuePair<string, Component> c in _ControlsToAdd)
            {
                _controls.Add(c.Key, c.Value);
            }
            // Clearing all of the remove and add controls so we don't stack
            _ControlsToRemove.Clear();
            _ControlsToAdd.Clear();
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            // Draw all controls that are the screen
            foreach (KeyValuePair<string, Component> c in _controls)
            {
                if (c.Key.Contains("Screen")) continue;
                c.Value.Draw(gameTime, ref _mg);
            }

            // Draw all controls that are not the screen
            foreach (KeyValuePair<string, Component> c in _controls)
            {
                if (!c.Key.Contains("Screen")) continue;
                if (c.Key == current_screen)
                    c.Value.Draw(gameTime, ref _mg);
            }

            if (transScreen == null) return;
            transScreen.Draw(gameTime, ref _mg);

            //_mg.DrawString("Mouse: " + _mouse.state.X + ":" + _mouse.state.Y + ":" + _mouse.state.LeftButton.ToString(), _mouse.state.X, _mouse.state.Y + 25, Color.White, 10.0f);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {
            foreach (KeyValuePair<string, Component> c in _controls)
            {
                c.Value.UpdateKeyInput(gameTime, key, keyValue);
            }
        }

        public static void changeScreens(string new_screen, BoosterPack selectedPack, Screen screen)
        {
            // Check if _control contains the screen we are wanting to switch to
            //   Prevents adding two of the same screen
            if(_controls.ContainsKey(new_screen))
            {
                // We dispose of the current screen as we are wanting to adjust to the new one
                (_controls[current_screen] as Screen).Dispose();
                // Add it to the queue to be added to _control
                _ControlsToRemove.Add(current_screen);
                // Set the current_screen text to the new_screen text;
                current_screen = new_screen;
            } else
            {
                // If the screen doesn't exist then we want to add it
                _ControlsToAdd.Add(new_screen, screen);
                current_screen = new_screen;
            }
        }
    }
}
