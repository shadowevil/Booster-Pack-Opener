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
    public static class GUI
    {
        public static Texture2D RoundedRectangleTexture;
        public static Texture2D topleftCorner;
        public static Texture2D bottomLeftCorner;
        public static Texture2D topRightCorner;
        public static Texture2D bottomRightCorner;

        public static void InitRoundedRectangle()
        {
            Color[] cData = new Color[53 * 53];
            RoundedRectangleTexture.GetData<Color>(0, new Rectangle(0, 0, 53, 53), cData, 0, cData.Length);
            topleftCorner = new Texture2D(GameComponent._mg._graphics, 53, 53);
            topleftCorner.SetData(cData);

            cData = new Color[53 * 53];
            RoundedRectangleTexture.GetData<Color>(0, new Rectangle(203, 0, 53, 53), cData, 0, cData.Length);
            topRightCorner = new Texture2D(GameComponent._mg._graphics, 53, 53);
            topRightCorner.SetData(cData);

            cData = new Color[53 * 53];
            RoundedRectangleTexture.GetData<Color>(0, new Rectangle(0, 203, 53, 53), cData, 0, cData.Length);
            bottomLeftCorner = new Texture2D(GameComponent._mg._graphics, 53, 53);
            bottomLeftCorner.SetData(cData);

            cData = new Color[53 * 53];
            RoundedRectangleTexture.GetData<Color>(0, new Rectangle(203, 203, 53, 53), cData, 0, cData.Length);
            bottomRightCorner = new Texture2D(GameComponent._mg._graphics, 53, 53);
            bottomRightCorner.SetData(cData);
        }
    }

    public class Button : Component
    {
        private string text;
        private float fontSize;
        private Texture2D btn;
        private Rectangle rect;
        private Action action;
        private bool hasClicked = false;

        public Button(string text, int x, int y, int width, int height, float radious, float fontSize, Action action)
        {
            this.text = text;
            this.fontSize = fontSize;
            btn = MonoGraphics.GetRoundedRectangle(width, height, radious);
            rect = new Rectangle(x, y, width, height);
            this.action = action;
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            _mg.Draw(btn, new Rectangle(rect.X-2, rect.Y-2, rect.Width+4, rect.Height+4), Color.Black);
            if (rect.Contains(winmain._mouse.mousePosition))
            {
                _mg.Draw(btn, rect, Color.OrangeRed);
            }
            else
            {
                _mg.Draw(btn, rect, Color.IndianRed);
            }
            float stringWidth = _mg.MeasureText(text, fontSize, true).X;
            float stringHeight = _mg.MeasureText(text, fontSize, true).Y;
            _mg.DrawString(text, rect.X + ((btn.Width - 27) - stringWidth) / 2, rect.Y + ((btn.Height - 27) - stringHeight) / 2, Color.Black, fontSize, true);
        }

        public override void Update(GameTime gameTime)
        {
            if (hasClicked) return;
            if (rect.Contains(winmain._mouse.mousePosition))
            {
                if (winmain._mouse.Click())
                {
                    action();
                    hasClicked = true;
                    return;
                }
            }
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }

    public class DropDownMenu : Component
    {
        public static Texture2D arrowDown;
        public Rectangle DropRect;
        public ScrollableBox dropdown;
        public bool isUsingDropDown = false;
        private int dropDownHeight = 120;
        private mgTimer pullDownTimer;
        private int dropDownValue = 0;
        private int direction = 0;
        public DropDownMenuItem selectedItem;

        public DropDownMenu(int x, int y, int width, int height, float lineHeight, int linesVisible, float scrollSpeed, int _dropDownHeight)
        {
            location = new Vector2(x, y);
            DropRect = new Rectangle(x, y, width, height);
            dropDownHeight = _dropDownHeight;
            dropdown = new ScrollableBox(new Rectangle(DropRect.X, DropRect.Y + DropRect.Height, DropRect.Width, dropDownHeight), 0, _lineHeight: lineHeight, _linesVisible: linesVisible, _scrollSpeed: scrollSpeed);

            pullDownTimer = new mgTimer(0.05);
        }

        public void Add(DropDownMenuItem menuItem)
        {
            dropdown._components.Add(menuItem.name, menuItem);
        }

        public void Add(string _name, string _text, object _value)
        {
            dropdown._components.Add(_name, new DropDownMenuItem(_name, _text, _value, this));
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            _mg.DrawFilledRectangle(DropRect, Color.Black * 0.75f);
            _mg.DrawFilledRectangle(new Rectangle(DropRect.X + DropRect.Width - DropRect.Height, DropRect.Y, DropRect.Height, DropRect.Height), Color.OrangeRed);
            _mg.DrawRectangle(new Rectangle(DropRect.X + DropRect.Width - DropRect.Height, DropRect.Y, DropRect.Height, DropRect.Height), Color.Black, 1);
            _mg.DrawRectangle(DropRect, Color.IndianRed, 1);

            if (selectedItem != null)
            {
                float height = _mg.MeasureText(selectedItem.text, 12.0f).Y;
                _mg.DrawString(selectedItem.text, DropRect.X + 4.0f, DropRect.Y + ((DropRect.Height - height) / 2), Color.White, 12.0f);
                if(selectedItem.value is Action)
                {
                    (selectedItem.value as Action)();
                }
            }

            if (isUsingDropDown)
            {
                if (DropRect.Contains(winmain._mouse.mousePosition))
                {
                    _mg.DrawRectangle(DropRect, Color.White, 1);
                    _mg.Draw(arrowDown, new Vector2(DropRect.X + DropRect.Width + DropRect.Height + ((DropRect.Height - arrowDown.Bounds.Width) / 2) - 3, DropRect.Y + DropRect.Height - 2), arrowDown.Bounds, Color.White * 0.75f, 3.15f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                } else
                {
                    _mg.Draw(arrowDown, new Vector2(DropRect.X + DropRect.Width + DropRect.Height + ((DropRect.Height - arrowDown.Bounds.Width) / 2) - 3, DropRect.Y + DropRect.Height - 2), arrowDown.Bounds, Color.Black * 0.75f, 3.15f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                }
            }
            else
            {
                if (DropRect.Contains(winmain._mouse.mousePosition))
                {
                    _mg.DrawRectangle(DropRect, Color.White, 1);
                    _mg.Draw(arrowDown, new Vector2(DropRect.X + DropRect.Width + ((DropRect.Height - arrowDown.Bounds.Width) / 2) + 3, DropRect.Y + 4), arrowDown.Bounds, Color.White * 0.75f, 0.0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    _mg.Draw(arrowDown, new Vector2(DropRect.X + DropRect.Width + ((DropRect.Height - arrowDown.Bounds.Width) / 2) + 3, DropRect.Y + 4), arrowDown.Bounds, Color.Black * 0.75f, 0.0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                }
            }

            if (dropDownValue >= 0)
            {
                Rectangle rect = new Rectangle(dropdown.scrollViewport.Bounds.X, dropdown.scrollViewport.Bounds.Y, dropdown.scrollViewport.Bounds.Width, (dropDownHeight / 100) * dropDownValue);
                _mg.DrawFilledRectangle(rect, Color.Black * 0.75f);
                _mg.DrawRectangle(rect, Color.IndianRed, 1);
                dropdown.Draw(gameTime, ref _mg);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (DropRect.Contains(winmain._mouse.mousePosition))
            {
                ShowMenu();
            }

            if (pullDownTimer.hasTicked(gameTime))
            {
                if(dropDownValue <= 100 && direction == 0 && isUsingDropDown) dropDownValue += 2;
                if(dropDownValue >= 0 && direction == 1 && !isUsingDropDown) dropDownValue -= 2;

                dropdown.scrollViewport.Height = (dropDownHeight / 100) * dropDownValue;
            }

            if(dropDownValue >= 0)
            {
                dropdown.Update(gameTime);
            }
        }

        public void ShowMenu()
        {
            if (winmain._mouse.Click() && (dropDownValue >= 100 || dropDownValue <= 0))
            {
                dropdown.scrollValue = 0.0f;
                isUsingDropDown = !isUsingDropDown;
                if (isUsingDropDown) direction = 0;
                else direction = 1;
            }
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }

    public class DropDownMenuItem : Component
    {
        public string name;
        public string text;
        public DropDownMenu parent;
        public Rectangle bounds;
        public object value;

        public DropDownMenuItem(string _name, string _text, object _value, DropDownMenu _parent)
        {
            name = _name;
            text = _text;
            parent = _parent;
            value = _value;
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            if (parent.dropdown.IntVisible.Contains(location))
            {
                if (bounds.Contains(winmain._mouse.mousePosition))
                {
                    _mg.DrawFilledRectangle(new Rectangle((int)location.X, (int)location.Y, bounds.Width, bounds.Height), Color.White * 0.5f);
                    if (winmain._mouse.Click())
                    {
                        parent.ShowMenu();
                        parent.selectedItem = this;
                    }
                }
                float textHeight = _mg.MeasureText(text, 10.0f).Y;
                _mg.DrawString(text, location.X + 4.0f, location.Y + ((bounds.Height - textHeight) / 2), Color.White, 10.0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            location = new Vector2(0, (parent.dropdown.lineheight * arrayPosition) - parent.dropdown.scrollValue);
            bounds = new Rectangle(parent.dropdown.scrollViewport.X + (int)location.X, parent.dropdown.scrollViewport.Y + (int)location.Y, parent.dropdown.scrollViewport.Width, (int)(parent.dropdown.lineheight));
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
