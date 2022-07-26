using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace BoosterPack
{
    // Main window for selecting what pack to open
    public class SelectionScreen : Screen
    {
        // Selected/hovered booster pack should never be null
        public BoosterPack selectedPack;
        private int packPosDrawCount = 0;
        private int packPosUpdateCount = 0;

        private int pkHoverDirection = 1;
        private mgTimer pkHoverTimer;
        private float packHover = 0.0f;

        private Button DeckScreenButton;

        public SelectionScreen()
        {
            // Try to initalize the pack if it exists, if not could cause problems
            GameComponent.Boosters.TryGetValue("LOB", out selectedPack);
            pkHoverTimer = new mgTimer(0.15);
            DeckScreenButton = new Button("Deck screen", GameComponent._mg.windowWidth - 100, GameComponent._mg.windowHeight - 25,
                95, 20, 10.0f, new Action(delegate {
                    if (GameComponent.transScreen != null) return;
                    GameComponent.transScreen = new TransitionScreen(TransitionScreen.Direction.OUT, "DeckScreen", null, new DeckScreen());
                }));
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            // For counting each pack and how it should display on the screen
            packPosDrawCount = 0;

            foreach (var pack in GameComponent.Boosters)
            {
                // Ensure the packIcon is not null if it is, skip and move on
                if (pack.Value.packIcon == null) continue;

                Rectangle pos = new Rectangle(250 + (packPosDrawCount * ((int)(pack.Value.iconSourceRect.Width * pack.Value.packIconScale) + 10)), 15, (int)(pack.Value.iconSourceRect.Width * pack.Value.packIconScale), (int)(pack.Value.iconSourceRect.Height * pack.Value.packIconScale));
                if (pos.Contains(winmain._mouse.mousePosition))
                {
                    if (pkHoverTimer.hasTicked(gameTime))
                    {
                        if (selectedPack != null)
                        {
                            if (pkHoverDirection == 1)
                            {
                                packHover += 0.005f;
                                if (1.0f - packHover <= 0.6f) pkHoverDirection = 0;
                            }
                            else
                            {
                                packHover -= 0.005f;
                                if (1.0f - packHover >= 1.0f) pkHoverDirection = 1;
                            }
                        }
                    }
                    _mg.DrawRectangle(new Rectangle(pos.X - 2, pos.Y - 2, pos.Width + 4, pos.Height + 4), Color.White * (1.0f - packHover), 2);
                    _mg.Draw(pack.Value.packIcon, pos.Location.ToVector2(), pack.Value.iconSourceRect, Color.White * (1.0f - packHover), 0.0f, Vector2.Zero, pack.Value.packIconScale, SpriteEffects.None, 0.0f);
                }
                else _mg.Draw(pack.Value.packIcon, pos.Location.ToVector2(), pack.Value.iconSourceRect, Color.White * 0.75f, 0.0f, Vector2.Zero, pack.Value.packIconScale, SpriteEffects.None, 0.0f);

                packPosDrawCount++;
            }
            if (selectedPack == null) return;
            _mg.Draw(selectedPack.packPreview, new Vector2(5, 5), selectedPack.previewSourceRect, Color.White, 0.0f, Vector2.Zero, selectedPack.packPreviewScale, SpriteEffects.None, 0.0f);
            _mg.DrawString(selectedPack.name.Aggregate((a, b) => a + " " + b), 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f, Color.White, 10.0f, true);
            string wrappedDescription = _mg.WrapText(selectedPack.description, 200.0f, 9.0f, false);
            float descHeight = _mg.MeasureText(wrappedDescription, 9.0f, false).Y;
            _mg.DrawString(wrappedDescription, 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 1.0f), Color.White, 9.0f);
            _mg.DrawStringWithLabel("Release Date: ", selectedPack.release.ToString(), 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 1.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);
            _mg.DrawStringWithLabel("Cards per pack: ", selectedPack.Cards.ToString(), 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 2.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);
            _mg.DrawStringWithLabel("Common: ", selectedPack.Common.Count.ToString(), 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 3.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);
            _mg.DrawStringWithLabel("Rare: ", selectedPack.Rare.Count + "(%" + selectedPack.R + ")", 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 4.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);
            _mg.DrawStringWithLabel("Super Rare: ", selectedPack.SuperRare.Count + "(%" + selectedPack.SR + ")", 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 5.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);
            _mg.DrawStringWithLabel("Ultra Rare: ", selectedPack.UltraRare.Count + "(%" + selectedPack.UR + ")", 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 6.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);
            _mg.DrawStringWithLabel("Total: ", (selectedPack.Common.Count + selectedPack.Rare.Count + selectedPack.SuperRare.Count + selectedPack.UltraRare.Count).ToString(), 5.0f, (selectedPack.previewSourceRect.Height * selectedPack.packPreviewScale) + 5.0f + (17.0f * 7.0f) + descHeight, 125.0f, Color.White, Color.White, 10.0f);

            DeckScreenButton.Draw(gameTime, ref _mg);
        }

        public override void Update(GameTime gameTime)
        {
            packPosUpdateCount = 0;
            foreach (var pack in GameComponent.Boosters)
            {
                if (pack.Value.packIcon == null) continue;
                Rectangle pos = new Rectangle(250 + (packPosUpdateCount * ((int)(pack.Value.iconSourceRect.Width * pack.Value.packIconScale) + 10)), 15, (int)(pack.Value.iconSourceRect.Width * pack.Value.packIconScale), (int)(pack.Value.iconSourceRect.Height * pack.Value.packIconScale));
                if (pos.Contains(winmain._mouse.mousePosition))
                {
                    Mouse.SetCursor(MouseCursor.Hand);
                    selectedPack = pack.Value;
                }
                else if (selectedPack.name[0] == pack.Value.name[0]) Mouse.SetCursor(MouseCursor.Arrow);
                packPosUpdateCount++;
            }
            packPosUpdateCount = 0;
            foreach (var pack in GameComponent.Boosters)
            {
                if (pack.Value.packIcon == null) continue;
                Rectangle pos = new Rectangle(250 + (packPosUpdateCount * ((int)(pack.Value.iconSourceRect.Width * pack.Value.packIconScale) + 10)), 15, (int)(pack.Value.iconSourceRect.Width * pack.Value.packIconScale), (int)(pack.Value.iconSourceRect.Height * pack.Value.packIconScale));
                if (pos.Contains(winmain._mouse.mousePosition))
                {
                    if (winmain._mouse.Click())
                    {
                        GameComponent.ChosenBooster = selectedPack;
                        GameComponent.changeScreens("UnpackScreen", selectedPack, new UnpackScreen());
                    }
                }
                packPosUpdateCount++;
            }

            DeckScreenButton.Update(gameTime);
        }

        public override void Dispose()
        {

        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
