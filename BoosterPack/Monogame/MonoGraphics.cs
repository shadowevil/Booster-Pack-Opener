using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using PrimitiveShape = MonoGame.Primitives2D;
using System.Reflection;

namespace BoosterPack
{
    public class MonoGraphics
    {
        private GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        public GraphicsDevice _graphics { get; private set; }
        public SpriteBatch _spriteBatch { get; private set; }
        public Dictionary<KeyValuePair<float, bool>, SpriteFont> font { get; set; }
        public int windowWidth;
        public int windowHeight;
        public const int MAX_FONT_SIZE = 22;
        public static bool hasSpriteBatchBegun { get; private set; }

        public MonoGraphics(string windowName, int _w, int _h, Game window, bool vsync = false, bool multisample = true, GraphicsProfile _gp = GraphicsProfile.HiDef, SurfaceFormat _sf = SurfaceFormat.Color, bool isFullscreen = false)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(window);
            GraphicsDeviceManager.GraphicsProfile = _gp;
            GraphicsDeviceManager.SynchronizeWithVerticalRetrace = vsync;
            GraphicsDeviceManager.PreferMultiSampling = multisample;
            windowHeight = _h;
            windowWidth = _w;
            GraphicsDeviceManager.ApplyChanges();

            _graphics = GraphicsDeviceManager.GraphicsDevice;
            _graphics.PresentationParameters.MultiSampleCount = 8;
            _graphics.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
            
            window.Window.Title = windowName;

            _spriteBatch = new SpriteBatch(_graphics);

            _fillTexture = new Texture2D(_graphics, 1, 1);
            _fillTexture.SetData(new Color[] { Color.White });
        }

        public void setResolution(bool isFullscreen = false, int w = -1, int h = -1)
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = windowWidth;
            GraphicsDeviceManager.PreferredBackBufferHeight = windowHeight;
            GraphicsDeviceManager.IsFullScreen = isFullscreen;
            if (isFullscreen)
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = GraphicsDeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                GraphicsDeviceManager.PreferredBackBufferHeight = GraphicsDeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            }
            windowHeight = GraphicsDeviceManager.PreferredBackBufferHeight;
            windowWidth = GraphicsDeviceManager.PreferredBackBufferWidth;
            GraphicsDeviceManager.ApplyChanges();
        }

        public void LoadFonts()
        {
            string ContentPath = Directory.GetCurrentDirectory() + "\\Data\\";
            font = new Dictionary<KeyValuePair<float, bool>, SpriteFont>();
            for (int i = 8; i < MonoGraphics.MAX_FONT_SIZE + 1; i++)
            {
                if (File.Exists(ContentPath + "Font\\font" + i + ".xnb"))
                {
                    font.Add(new KeyValuePair<float, bool>(i, false), winmain._gameWindow.Content.Load<SpriteFont>(ContentPath + "Font\\font" + i));
                    if(File.Exists(ContentPath + "Font\\font" + i + "b.xnb"))
                    {
                        font.Add(new KeyValuePair<float, bool>(i, true), winmain._gameWindow.Content.Load<SpriteFont>(ContentPath + "Font\\font" + i + "b"));
                    }
                }
            }
        }

        public void BeginSpriteBatch()
        {
            _spriteBatch.Begin();
            hasSpriteBatchBegun = true;
        }
        
        public void EndSpriteBatch()
        {
            _spriteBatch.End();
            hasSpriteBatchBegun = false;
        }

        public void DrawString(string text, float x, float y, Color c, float fontSize, bool bold = false)
        {
            SpriteFont _f = null;
            bool found = font.TryGetValue(new KeyValuePair<float, bool>(fontSize, bold), out _f);
            if (!found) return;
            if (!hasSpriteBatchBegun) return;
            _spriteBatch.DrawString(_f, text, new Vector2(x, y), c);
        }

        public void DrawStringWithLabel(string Label, string content, float x, float y, Color c, Color contentColor, float fontSize)
        {
            DrawString(Label, x, y, c, fontSize, true);
            float width = MeasureText(Label, fontSize, true).X;
            DrawString(content, x + width + 2.0f, y, contentColor, fontSize);
        }

        public void DrawStringWithLabel(string Label, string content, float x, float y, float width, Color c, Color contentColor, float fontSize)
        {
            DrawString(Label, x, y, c, fontSize, true);
            DrawString(content, x + width, y, contentColor, fontSize);
        }

        public void Draw(Texture2D tx, Rectangle rc, Color c)
        {
            _spriteBatch.Draw(tx, rc, c);
        }

        public void Draw(Texture2D tx, Vector2 vc2, Color c)
        {
            _spriteBatch.Draw(tx, vc2, c);
        }

        public void Draw(Texture2D tx, Rectangle dr, Rectangle sr, Color c)
        {
            _spriteBatch.Draw(tx, dr, sr, c);
        }

        public void Draw(Texture2D tx, Vector2 vc2, Rectangle sr, Color c)
        {
            _spriteBatch.Draw(tx, vc2, sr, c);
        }

        public void Draw(Texture2D tx, Rectangle dr, Rectangle sr, Color c, float fr, Vector2 vc2o, SpriteEffects se, float fld)
        {
            _spriteBatch.Draw(tx, dr, sr, c, fr, vc2o, se, fld);
        }

        public void Draw(Texture2D tx, Vector2 vc2, Rectangle sr, Color c, float fr, Vector2 vc2o, float scale, SpriteEffects se, float fld)
        {
            _spriteBatch.Draw(tx, vc2, sr, c, fr, vc2o, scale, se, fld);
        }

        public void Draw(Texture2D tx, Vector2 vc2, Rectangle sr, Color c, float fr, Vector2 vc2o, Vector2 vc2s, SpriteEffects se, float fld)
        {
            _spriteBatch.Draw(tx, vc2, sr, c, fr, vc2o, vc2s, se, fld);
        }

        Texture2D _pointTexture;

        public void DrawRectangle(Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height), color);
            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, lineWidth), color);

            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width - lineWidth, rectangle.Y, lineWidth, rectangle.Height), color);
            _spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, lineWidth), color);
        }

        Texture2D _fillTexture;

        public void DrawFilledRectangle(Rectangle rectangle, Color color)
        {
            this.Draw(_fillTexture, rectangle, color);
        }

        public void DrawFilledRectangle(Rectangle rectangle, Vector2 location, Color color)
        {
            this.Draw(_fillTexture, location, rectangle, color);
        }

        public static string WrapText(ref MonoGraphics mf, string text, float MaxLineWidth, float fontSize, bool bold = false)
        {
            text = text.Replace("Ɐ", "A");
            text = text.Replace("●", "-");
            if (mf.font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(text).X < MaxLineWidth)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = mf.font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(" ").X;
            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 size = mf.font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(words[i]);
                if (linewidth + size.X < MaxLineWidth)
                {
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n");
                    linewidth = size.X + spaceWidth;
                }
                wrappedText.Append(words[i]);
                wrappedText.Append(" ");
            }

            return wrappedText.ToString();
        }

        public string WrapText(string text, float MaxLineWidth, float fontSize, bool bold = false)
        {
            text = text.Replace("Ɐ", "A");
            text = text.Replace("●", "-");
            if (font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(text).X < MaxLineWidth)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(" ").X;
            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 size = font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(words[i]);
                if (linewidth + size.X < MaxLineWidth)
                {
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n");
                    linewidth = size.X + spaceWidth;
                }
                wrappedText.Append(words[i]);
                wrappedText.Append(" ");
            }

            return wrappedText.ToString();
        }

        public Vector2 MeasureText(string text, float fontSize, bool bold = false)
        {
            return GameComponent._mg.font[new KeyValuePair<float, bool>(fontSize, bold)].MeasureString(text);
        }

        public static Texture2D GetRoundedRectangle(int width, int height, float radius)
        {
            Rectangle rect = new Rectangle(0, 0, (int)(53.0f * 1.5f) + (int)(width - 53.0f), (int)(53.0f * 1.5f) + (int)(height - 53.0f));
            RenderTarget2D renderTarget2D = new RenderTarget2D(GameComponent._mg._graphics, rect.Width, rect.Height);
            Texture2D returntexture = new Texture2D(GameComponent._mg._graphics, rect.Width, rect.Height);

            GameComponent._mg._graphics.SetRenderTarget(renderTarget2D);
            GameComponent._mg._graphics.Clear(Color.Fuchsia);
            GameComponent._mg.BeginSpriteBatch();
            GameComponent._mg.DrawFilledRectangle(new Rectangle(rect.X + (int)(53.0f * radius), rect.Y, rect.Width - (int)((53.0f * radius) * 2) + 2, rect.Height), Color.White);
            GameComponent._mg.DrawFilledRectangle(new Rectangle(rect.X, rect.Y + (int)(53.0f * radius), rect.Width, rect.Height - (int)((53.0f * radius) * 2) + 2), Color.White);
            GameComponent._mg.Draw(GUI.topleftCorner, new Vector2(rect.X, rect.Y), GUI.topleftCorner.Bounds, Color.White, 0.0f, Vector2.Zero, radius, SpriteEffects.None, 0.0f);
            GameComponent._mg.Draw(GUI.topRightCorner, new Vector2(rect.X + rect.Width - (int)(53.0f * radius), rect.Y), GUI.topRightCorner.Bounds, Color.White, 0.0f, Vector2.Zero, radius, SpriteEffects.None, 0.0f);
            GameComponent._mg.Draw(GUI.bottomRightCorner, new Vector2(rect.X + rect.Width - (int)(53.0f * radius), rect.Y + rect.Height - (int)(53.0f * radius)), GUI.bottomRightCorner.Bounds, Color.White, 0.0f, Vector2.Zero, radius, SpriteEffects.None, 0.0f);
            GameComponent._mg.Draw(GUI.bottomLeftCorner, new Vector2(rect.X, rect.Y + rect.Height - (int)(53.0f * radius)), GUI.bottomLeftCorner.Bounds, Color.White, 0.0f, Vector2.Zero, radius, SpriteEffects.None, 0.0f);
            GameComponent._mg.EndSpriteBatch();
            Color[] texdata = new Color[renderTarget2D.Width * renderTarget2D.Height];
            renderTarget2D.GetData<Color>(texdata);
            for (int i = 0; i < texdata.Length; i++)
                if (texdata[i] != null)
                    if (texdata[i] == Color.Fuchsia || texdata[i] != Color.White)
                        texdata[i] = Color.Transparent;
            returntexture.SetData(texdata);

            return returntexture;
        }
    }


    public class ScrollableBox : Component
    {
        private Viewport defaultViewport;
        public Viewport scrollViewport;
        public float scrollValue = 0;
        public float scrollSpeed = 15.0f;
        public float lineheight;
        public int lineVisible = 1;
        private float maxHeight = 570.0f;
        private Rectangle scrollBar;
        private Rectangle scrollBarButtonUp;
        private Rectangle scrollBarButtonDown;
        private Rectangle scrollBarBAR;
        private Vector2 scrollLocation;
        private float maxScrollHeight;

        private bool isArrowKeyDown = false;
        private bool isArrowKeyUp = false;

        public Dictionary<string, Component> _components;

        public Rectangle ExtVisible;
        public Rectangle IntVisible;

        public ScrollableBox(Rectangle rect, float _maxHeight, float _lineHeight, int _linesVisible, float _scrollSpeed = 15.0f)
        {
            _components = new Dictionary<string, Component>();
            scrollViewport = new Viewport(rect);
            scrollSpeed = _scrollSpeed;
            maxHeight = _maxHeight;
            lineVisible = _linesVisible;
            lineheight = _lineHeight;
            if(maxHeight < rect.Height) maxHeight = rect.Height;

            scrollBar = new Rectangle(scrollViewport.Width - 10, 0, 10, scrollViewport.Height);
            scrollBarButtonUp = new Rectangle(scrollBar.X, scrollBar.Y, 10, 10);
            scrollBarButtonDown = new Rectangle(scrollBar.X, scrollViewport.Height - 10, 10, 10);
            scrollBarBAR = new Rectangle(0, 0, 5, 40);
            scrollLocation = new Vector2(scrollBar.X + (scrollBar.Width - scrollBarBAR.Width) / 2, 0);
            maxScrollHeight = (scrollBar.Height - scrollBarButtonUp.Height - scrollBarButtonDown.Height);
        }

        public void EnterViewport(ref MonoGraphics _mg)
        {
            _mg.EndSpriteBatch();
            defaultViewport = _mg._graphics.Viewport;

            _mg._graphics.Viewport = scrollViewport;
            _mg.BeginSpriteBatch();
        }

        public void ExitViewport(ref MonoGraphics _mg)
        {
            _mg.EndSpriteBatch();
            _mg._graphics.Viewport = defaultViewport;
            _mg.BeginSpriteBatch();
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            EnterViewport(ref _mg);

            int i = 0;
            foreach(var kvp in _components)
            {
                kvp.Value.arrayPosition = i;
                kvp.Value.Draw(gameTime, ref _mg);
                i++;
            }

            if (maxHeight > 0)
            {
                _mg.DrawFilledRectangle(scrollBar, Color.Black * 0.75f);
                _mg.DrawFilledRectangle(scrollBarButtonUp, Color.OrangeRed);
                _mg.DrawRectangle(scrollBarButtonUp, Color.IndianRed, 1);
                _mg.DrawFilledRectangle(scrollBarButtonDown, Color.OrangeRed);
                _mg.DrawRectangle(scrollBarButtonDown, Color.IndianRed, 1);
                _mg.DrawFilledRectangle(scrollBarBAR, scrollLocation, Color.Orange * 0.75f);
            }
            ExitViewport(ref _mg);
        }

        public override void Update(GameTime gameTime)
        {
            maxHeight = lineheight * (_components.Count - lineVisible) + lineheight;
            if (maxHeight < 0) maxHeight = 0;

            ExtVisible = new Rectangle(scrollViewport.X, scrollViewport.Y - 25, scrollViewport.Width, scrollViewport.Height + 25);
            IntVisible = new Rectangle(0, -25, scrollViewport.Width, scrollViewport.Height + 25);

            if (scrollViewport.Bounds.Contains(winmain._mouse.mousePosition))
            {
                if (winmain._mouse.lastState.ScrollWheelValue > winmain._mouse.state.ScrollWheelValue ||
                    isArrowKeyDown) scrollValue += scrollSpeed;
                if (winmain._mouse.lastState.ScrollWheelValue < winmain._mouse.state.ScrollWheelValue ||
                    isArrowKeyUp) scrollValue -= scrollSpeed;
                scrollValue = MathHelper.Clamp(scrollValue, 0, maxHeight);
            }

            float percentage = (scrollValue / maxHeight) * 100;
            float scrollMaxHeight = (maxScrollHeight + scrollBarBAR.Height) - scrollBarButtonDown.Height - scrollBarButtonUp.Height;
            scrollLocation.Y = (percentage / 100.0f) * (scrollMaxHeight - (scrollBarBAR.Height + 10.0f));
            scrollLocation.Y = MathHelper.Clamp(scrollLocation.Y, scrollBarButtonUp.Height, scrollMaxHeight);
            if (scrollMaxHeight < 200.0f)
            {
                if (scrollLocation.Y + scrollBarBAR.Height + scrollBarButtonDown.Height >= scrollMaxHeight) scrollLocation.Y = scrollMaxHeight - scrollBarButtonDown.Height - scrollBarBAR.Height - 23;
            }

            foreach (var kvp in _components)
            {
                kvp.Value.Update(gameTime);
            }
            scrollBarButtonUp = new Rectangle(scrollBar.X, scrollBar.Y, 10, 10);
            scrollBarButtonDown = new Rectangle(scrollBar.X, scrollViewport.Height - 10, 10, 10);
        }

        public void DrawString(ref MonoGraphics _mg, string text, float x, float y, Color c, float fontSize, bool bold = false)
        {
            EnterViewport(ref _mg);
            _mg.DrawString(text, x, y, c, fontSize, bold);
            ExitViewport(ref _mg);
        }

        public void DrawTexture(ref MonoGraphics _mg, Texture2D texture, float x, float y, Rectangle sourceRect, Color c, float scale)
        {
            EnterViewport(ref _mg);
            _mg.Draw(texture, new Vector2(x, y), sourceRect, c, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            ExitViewport(ref _mg);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {
            if (key == Keys.Up) isArrowKeyUp = keyValue;
            if (key == Keys.Down) isArrowKeyDown = keyValue;
        }
    }
}
