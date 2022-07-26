using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;

namespace BoosterPack
{
    public class Card : Component, ICloneable, IDisposable
    {
        public CardAttributes cardAttributes;
        public Texture2D card;
        public Rectangle sourceRect;
        public Rectangle bounds;
        public bool showTag = false;
        public bool showParticles = true;
        public float opacity = 1.0f;
        public float cardScale = 1.0f;
        public CardParticles particles;

        public Card(CardAttributes cardAttrib, Texture2D cardTex, bool showPart = false, float x = -1.0f, float y = -1.0f, float scale = 1.0f)
        {
            this.cardAttributes = cardAttrib;
            this.card = cardTex;

            this.sourceRect = new Rectangle(0, 0, card.Width, card.Height);
            cardScale = scale;

            if (x != -1 && y != -1)
            {
                this.bounds = new Rectangle((int)x, (int)y, (int)(card.Width * cardScale), (int)(card.Height * cardScale));
                this.location = new Vector2((int)x, (int)y);
            }
            else
            {
                this.bounds = new Rectangle((GameComponent._mg.windowWidth - card.Width) / 2, (GameComponent._mg.windowHeight - card.Height) / 2, card.Width, card.Height);
                this.location = new Vector2((GameComponent._mg.windowWidth - card.Width) / 2, (GameComponent._mg.windowHeight - card.Height) / 2);
            }

            showParticles = showPart;
            particles = new CardParticles(cardAttrib, this.location, new Rectangle(this.sourceRect.X, this.sourceRect.Y, (int)(this.sourceRect.Width * cardScale), (int)(this.sourceRect.Height * cardScale)), new Range<float>(1.0f, 3.0f));
        }

        public void Dispose()
        {

        }

        public object Clone()
        {
            Card c = new Card(this.cardAttributes, card, showParticles, scale: cardScale);
            return c;
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            _mg.Draw(card, location, sourceRect, Color.White * opacity, 0.0f, Vector2.Zero, cardScale, SpriteEffects.None, 0.0f);
            if (cardAttributes.rarity != Attributes.Rarity.COMMON && CardAttributes.overlay != null)
            {
                _mg.Draw(CardAttributes.overlay, new Vector2(location.X + (18.0f * cardScale), location.Y + (44.0f * cardScale)),
                    new Rectangle(18, 44, 141, 138), (Color.Ivory * 0.55f) * opacity, 0.0f, Vector2.Zero, cardScale, SpriteEffects.None, 0.0f);
            }

            if (showTag)
            {
                Texture2D cardTag = null;
                switch (cardAttributes.rarity)
                {
                    case Attributes.Rarity.RARE:
                        cardTag = CardAttributes.rareTag;
                        break;
                    case Attributes.Rarity.SUPER_RARE:
                        cardTag = CardAttributes.superTag;
                        break;
                    case Attributes.Rarity.ULTRA_RARE:
                        cardTag = CardAttributes.ultraTag;
                        break;
                    default:
                    case Attributes.Rarity.COMMON:
                        cardTag = CardAttributes.commonTag;
                        break;
                }
                if (cardTag != null)
                {
                    _mg.Draw(cardTag, new Vector2(location.X + card.Width - 80, location.Y), new Rectangle(3, 0, 80, 21), Color.White);
                }
            }

            if (!showParticles) return;
            particles.Draw(gameTime, ref _mg);
        }

        public override void Update(GameTime gameTime)
        {
            if (!showParticles) return;
            particles.Position = location;
            particles.Update(gameTime);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }

    public class CardAttributes
    {
        public string id;
        public string name;
        public string description;
        public List<KeyValuePair<string, Attributes.Types>> type;
        public int attack = -2;
        public int defense = -2;
        public int level = 1;
        public KeyValuePair<string, Attributes.Race>? race;
        public string race_string
        {
            get
            {
                if (race.HasValue) return race.Value.Key.Substring(0, 1) + race.Value.Key.ToLower().Substring(1, race.Value.Key.Length - 1);
                else return "";
            }
        }
        public int race_flag
        {
            get
            {
                if (race.HasValue) return (int)race.Value.Value;
                else return -1;
            }
        }
        public KeyValuePair<string, Attributes.Element>? element;
        public string element_string
        {
            get
            {
                if (race.HasValue) return element.Value.Key.Substring(0, 1) + element.Value.Key.ToLower().Substring(1, element.Value.Key.Length - 1);
                else return "";
            }
        }
        public int element_flag
        {
            get
            {
                if (element.HasValue) return (int)element.Value.Value;
                else return -1;
            }
        }
        public Attributes.Rarity rarity;

        public static Texture2D overlay;
        public static Texture2D commonTag;
        public static Texture2D rareTag;
        public static Texture2D superTag;
        public static Texture2D ultraTag;
        public static Texture2D levelStar;
    }

    public class CardParticles : Component
    {
        public Vector2 WidthHeight;
        public Vector2 Position;
        private ParticleEffect _particleEffect;
        private Texture2D _particleTexture;

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            if (_particleEffect == null) return;
            _mg._spriteBatch.Draw(_particleEffect);
        }

        public CardParticles(CardAttributes cardAttributes, Vector2 location, Rectangle sourceRect, Range<float> particleSize)
        {
            if (cardAttributes.rarity != Attributes.Rarity.SUPER_RARE &&
                cardAttributes.rarity != Attributes.Rarity.ULTRA_RARE)
                return;

            Position = location;
            WidthHeight = new Vector2(sourceRect.Width, sourceRect.Height);

            _particleTexture = new Texture2D(GameComponent._mg._graphics, 1, 1);
            _particleTexture.SetData(new[] { Color.White });

            Modifier modifier;
            if (cardAttributes.rarity == Attributes.Rarity.ULTRA_RARE)
            {
                modifier = new VelocityColorModifier
                {
                    StationaryColor = Color.Green.ToHsl(),
                    VelocityColor = Color.Blue.ToHsl(),
                    VelocityThreshold = 80f
                };
            }
            else
            {
                modifier = new AgeModifier
                {
                    Interpolators =
                    {
                        new ColorInterpolator
                        {
                            StartValue = Color.White.ToHsl(),
                            EndValue = Color.Black.ToHsl()
                        }
                    }
                };
            }

            TextureRegion2D textureRegion = new TextureRegion2D(_particleTexture);
            _particleEffect = new ParticleEffect(autoTrigger: false)
            {
                Position = new Vector2(location.X + sourceRect.Width / 2, location.Y + sourceRect.Height / 2),
                Emitters = new List<ParticleEmitter>
                    {
                        new ParticleEmitter(textureRegion, cardAttributes.rarity == Attributes.Rarity.ULTRA_RARE ? 100 : 50, TimeSpan.FromSeconds(0.25),
                        Profile.BoxFill(sourceRect.Width, sourceRect.Height))
                        {
                            Parameters = new ParticleReleaseParameters
                            {
                                Speed = new Range<float>(0.0f, 50.0f),
                                Quantity = 1,
                                Rotation = new Range<float>(-1f, 1f),
                                Scale = particleSize
                            },
                            Modifiers =
                            {
                                modifier,
                                new RotationModifier {RotationRate = -2.1f},
                                new RectangleContainerModifier {Width = sourceRect.Width + 15, Height = sourceRect.Height + 15 },
                                new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30.0f},
                                new OpacityFastFadeModifier(),
                            }
                        }
                    }
            };
        }

        public override void Update(GameTime gameTime)
        {
            if (_particleEffect == null) return;
            _particleEffect.Position = new Vector2(Position.X + WidthHeight.X / 2, Position.Y + WidthHeight.Y / 2);
            _particleEffect.Scale = new Vector2(WidthHeight.X, WidthHeight.Y);
            _particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
