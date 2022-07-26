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
using System.Data;
using System.IO;
using System.Linq;

namespace BoosterPack
{
    public class UnpackScreen : Screen
    {
        private int openProgress = 0;
        private int slideProgress = 0;
        private bool initOpen = false;
        private mgTimer openTimer;
        private mgTimer slideTimer;
        private bool isMoving = false;
        private mgTimer cardMoveToBack;
        private bool isMovingToBack = false;
        private mgTimer cardMoveToFront;
        private bool isMovingToFront = false;
        private int direction = 0;
        private Random random;

        private int cardCount = 0;
        public List<Card> packOfCards;
        public Card activeCard;

        public bool screenView = false;
        private int cardHoverDirection = 1;
        private mgTimer cardHoverTimer;
        private float cardHover = 0.0f;
        private List<CardParticles> cardParticles;

        private Button BackButton;
        public UnpackScreen()
        {
            openTimer = new mgTimer(0.15);
            slideTimer = new mgTimer(0.15);
            cardMoveToBack = new mgTimer(0.15);
            cardMoveToFront = new mgTimer(0.15);
            random = new Random(DateTime.Now.Millisecond);
            packOfCards = new List<Card>();
            cardHoverTimer = new mgTimer(0.15);
            screenView = GameComponent.defaultScreenView;
            cardParticles = new List<CardParticles>();

            BackButton = new Button("Done", GameComponent._mg.windowWidth - 100, GameComponent._mg.windowHeight - 25, 95, 20, 10.0f, new Action(delegate
            {
                GameComponent.changeScreens("SelectionScreen", null, null);
            }));

            LoadCards(GameComponent.ChosenBooster.Cards);
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            _mg.DrawFilledRectangle(new Rectangle(0, 0, _mg.windowWidth, _mg.windowHeight), Color.Black * 0.75f);

            if (!screenView)
            {
                if (activeCard != null && isMovingToFront && !isMovingToBack)
                {
                    activeCard.Draw(gameTime, ref _mg);
                }

                for (int i = packOfCards.Count - 1; i >= 0; i--)
                {
                    packOfCards[i].Draw(gameTime, ref _mg);
                }

                if (activeCard != null && !isMovingToFront && isMovingToBack)
                {
                    activeCard.Draw(gameTime, ref _mg);
                }
            }
            else
            {
                int c = 0;
                if (packOfCards.Count <= 0) return;
                if (activeCard == null) activeCard = packOfCards[0];
                int widthOfAllCards = (int)((packOfCards[0].card.Width * 0.5f) + 5.0f) * packOfCards.Count;
                foreach (Card card in packOfCards)
                {
                    Vector2 pos = new Vector2((((card.sourceRect.Width * 0.5f) * c) + (5 * c)) - (widthOfAllCards - _mg.windowWidth) / 2, 25);
                    Rectangle cardPosRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(card.card.Width * 0.5f), (int)(card.card.Height * 0.5f));

                    if (slideProgress < 200)
                    {
                        _mg.Draw(card.card, pos, card.sourceRect, Color.White * (0.0f + (slideProgress / 150.0f)), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                        if (card.cardAttributes.rarity != Attributes.Rarity.COMMON && CardAttributes.overlay != null)
                        {
                            _mg.Draw(CardAttributes.overlay, new Vector2(pos.X + (18.0f * 0.5f), pos.Y + (44.0f * 0.5f)),
                                new Rectangle(18, 44, 141, 138), (Color.Ivory * 0.55f) * (0.0f + (slideProgress / 150.0f)), 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                        }

                        foreach (CardParticles cp in cardParticles)
                            cp.Draw(gameTime, ref _mg);
                    }
                    else
                    {
                        if (cardPosRect.Contains(winmain._mouse.mousePosition))
                        {
                            activeCard = card;
                            if (cardHoverTimer.hasTicked(gameTime))
                            {
                                if (cardHoverDirection == 1)
                                {
                                    cardHover += 0.005f;
                                    if (1.0f - cardHover <= 0.6f) cardHoverDirection = 0;
                                }
                                else
                                {
                                    cardHover -= 0.005f;
                                    if (1.0f - cardHover >= 1.0f) cardHoverDirection = 1;
                                }
                            }
                            _mg.Draw(card.card, pos, card.sourceRect, Color.White * (1.0f - cardHover), 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                            if (card.cardAttributes.rarity != Attributes.Rarity.COMMON && CardAttributes.overlay != null)
                            {
                                _mg.Draw(CardAttributes.overlay, new Vector2(pos.X + (18.0f * 0.5f), pos.Y + (44.0f * 0.5f)),
                                    new Rectangle(18, 44, 141, 138), (Color.Ivory * 0.55f) * (0.0f + (slideProgress / 150.0f)), 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                            }

                            foreach (CardParticles cp in cardParticles)
                                cp.Draw(gameTime, ref _mg);
                        }
                        else 
                        {
                            _mg.Draw(card.card, pos, card.sourceRect, Color.White * 0.75f, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                            if (card.cardAttributes.rarity != Attributes.Rarity.COMMON && CardAttributes.overlay != null)
                            {
                                _mg.Draw(CardAttributes.overlay, new Vector2(pos.X + (18.0f * 0.5f), pos.Y + (44.0f * 0.5f)),
                                    new Rectangle(18, 44, 141, 138), (Color.Ivory * 0.55f) * (0.0f + (slideProgress / 150.0f)), 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
                            }

                            foreach (CardParticles cp in cardParticles)
                                cp.Draw(gameTime, ref _mg);
                        }
                    }
                    c++;
                }

                activeCard.Draw(gameTime, ref _mg);
            }

            Rectangle rect = new Rectangle(GameComponent.ChosenBooster.previewSourceRect.X, GameComponent.ChosenBooster.previewSourceRect.Y, GameComponent.ChosenBooster.previewSourceRect.Width, GameComponent.ChosenBooster.previewSourceRect.Height);
            if (slideProgress >= 200)
            {
                if (packOfCards.Count <= 0)
                {
                    GameComponent.changeScreens("SelectionScreen", null, new SelectionScreen());
                    return;
                }
                if (!screenView)
                {
                    string text = "(" + (cardCount + 1) + "/" + GameComponent.ChosenBooster.Cards + ")";
                    float sw = _mg.MeasureText(text, 12.0f, false).X;
                    Vector2 pos = new Vector2((_mg.windowWidth - rect.Width) / 2,
                        ((_mg.windowHeight - rect.Height) / 2));
                    _mg.DrawString(text, pos.X + ((rect.Width - sw) / 2), pos.Y + (rect.Height - 50.0f), Color.White, 12.0f);

                    text = packOfCards[0].cardAttributes.description;
                    float maxWidth = 600.0f;
                    text = MonoGraphics.WrapText(ref _mg, text, maxWidth, 12.0f);
                    _mg.DrawString(text, pos.X + ((rect.Width - maxWidth) / 2) + 25.0f, pos.Y + (rect.Height) - 25.0f, Color.White, 12.0f);
                }
                else
                {
                    Vector2 pos = new Vector2((_mg.windowWidth - rect.Width) / 2, ((_mg.windowHeight - rect.Height) / 2));
                    string text = activeCard.cardAttributes.description;
                    float maxWidth = 600.0f;
                    text = MonoGraphics.WrapText(ref _mg, text, maxWidth, 12.0f);
                    _mg.DrawString(text, pos.X + ((rect.Width - maxWidth) / 2) + 25.0f, pos.Y + (rect.Height) - 25.0f, Color.White, 12.0f);
                }
            }

            if (slideProgress <= 200)
            {
                _mg.Draw(GameComponent.ChosenBooster.packPreview, new Vector2((_mg.windowWidth - rect.Width) / 2,
                    ((_mg.windowHeight - rect.Height) / 2) + (3.15f * slideProgress)), new Rectangle(0, 25, rect.Width, rect.Height - 25), Color.White);
            }

            BackButton.Draw(gameTime, ref _mg);

            if (openProgress <= 100)
            {
                int changeWidth = (rect.Width / 100) * openProgress;
                _mg.Draw(GameComponent.ChosenBooster.packPreview, new Rectangle(((_mg.windowWidth - rect.Width) / 2) + changeWidth,
                    ((_mg.windowHeight - rect.Height) / 2) - 25, rect.Width - changeWidth, 25), new Rectangle(0, 0, rect.Width, 25), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (winmain._mouse.MiddleClick() && isMoving == false && slideProgress >= 200)
            {
                activeCard = null;
                cardCount = 0;
                screenView = !screenView;
                cardParticles = new List<CardParticles>();
                GameComponent.defaultScreenView = screenView;
            }

            BackButton.Update(gameTime);

            if (!initOpen)
            {
                Rectangle rect = new Rectangle(GameComponent.ChosenBooster.previewSourceRect.X, GameComponent.ChosenBooster.previewSourceRect.Y, GameComponent.ChosenBooster.previewSourceRect.Width, GameComponent.ChosenBooster.previewSourceRect.Height);
                rect = new Rectangle((GameComponent._mg.windowWidth - rect.Width) / 2, (GameComponent._mg.windowHeight - rect.Height) / 2, rect.Width, rect.Height);
                if (rect.Contains(winmain._mouse.mousePosition))
                {
                    if (winmain._mouse.Click())
                    {
                        initOpen = true;
                        GameComponent.seAttack.Play();
                    }
                }
            }

            if (initOpen)
            {
                if (openTimer.hasTicked(gameTime) && openProgress <= 100)
                {
                    openProgress++;
                }
                else
                {
                    if (slideTimer.hasTicked(gameTime) && slideProgress <= 200)
                    {
                        slideProgress++;
                    }
                }
            }

            if (packOfCards.Count > 0 && initOpen && slideProgress >= 200)
            {
                if (packOfCards[0].bounds.Contains(winmain._mouse.mousePosition) && activeCard == null)
                {
                    if (winmain._mouse.Click())
                    {
                        GameComponent.seDraw.Play();
                        direction = 1;
                        isMoving = true;
                        isMovingToBack = true;
                        activeCard = (Card)packOfCards[0].Clone();
                        packOfCards[0].Dispose();
                        packOfCards.RemoveAt(0);
                        cardCount++;
                        if (cardCount > GameComponent.ChosenBooster.Cards - 1)
                        {
                            cardCount = 0;
                        }
                    }
                    else if (winmain._mouse.RClick())
                    {
                        GameComponent.seDraw.Play();
                        direction = -1;
                        isMoving = true;
                        isMovingToFront = true;
                        activeCard = (Card)packOfCards[packOfCards.Count - 1].Clone();
                        packOfCards[packOfCards.Count - 1].Dispose();
                        packOfCards.RemoveAt(packOfCards.Count - 1);
                        cardCount--;
                        if (cardCount < 0) cardCount = GameComponent.ChosenBooster.Cards - 1;
                    }
                }

                foreach (Card crd in packOfCards)
                {
                    crd.showParticles = true;
                    crd.Update(gameTime);
                }

                if (activeCard != null)
                {
                    if (isMoving)
                    {
                        if (isMovingToBack)
                        {
                            if (cardMoveToBack.hasTicked(gameTime))
                            {
                                if (direction == 1)
                                {
                                    activeCard.location.X += 1.5f;
                                    if (activeCard.location.X >= activeCard.bounds.X + activeCard.bounds.Width + 10)
                                    {
                                        isMoving = true;
                                        isMovingToFront = true;
                                        isMovingToBack = false;
                                    }
                                }
                                else if (direction == -1)
                                {
                                    activeCard.location.X -= 1.5f;
                                    if (activeCard.location.X <= activeCard.bounds.X)
                                    {
                                        activeCard.location = activeCard.bounds.Location.ToVector2();
                                        isMoving = false;
                                        isMovingToFront = false;
                                        isMovingToBack = false;
                                        packOfCards.Insert(0, activeCard);
                                        activeCard = null;
                                        direction = 0;
                                    }
                                }
                            }
                        }
                        else if (isMovingToFront)
                        {
                            if (cardMoveToFront.hasTicked(gameTime))
                            {
                                if (direction == 1)
                                {
                                    activeCard.location.X -= 1.5f;
                                    if (activeCard.location.X <= activeCard.bounds.X)
                                    {
                                        activeCard.location = activeCard.bounds.Location.ToVector2();
                                        isMoving = false;
                                        isMovingToFront = false;
                                        isMovingToBack = false;
                                        packOfCards.Add(activeCard);
                                        activeCard = null;
                                        direction = 0;
                                    }
                                }
                                else if (direction == -1)
                                {
                                    activeCard.location.X += 1.5f;
                                    if (activeCard.location.X >= activeCard.bounds.X + activeCard.bounds.Width + 10)
                                    {
                                        isMoving = true;
                                        isMovingToFront = false;
                                        isMovingToBack = true;
                                    }
                                }
                            }
                        }
                    }

                    if (activeCard == null) return;
                    activeCard.Update(gameTime);
                }

                int c = 0;
                int widthOfAllCards = (int)((packOfCards[0].card.Width * 0.5f) + 5.0f) * packOfCards.Count;
                foreach (Card card in packOfCards)
                {
                    Vector2 pos = new Vector2((((card.sourceRect.Width * 0.5f) * c) + (5 * c)) - (widthOfAllCards - GameComponent._mg.windowWidth) / 2, 25);
                    Rectangle cardPosRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(card.card.Width * 0.5f), (int)(card.card.Height * 0.5f));

                    if (cardPosRect.Contains(winmain._mouse.mousePosition))
                    {
                        Mouse.SetCursor(MouseCursor.Hand);
                    }

                    if (cardParticles.Count < packOfCards.Count)
                    {
                        cardParticles.Add(new CardParticles(card.cardAttributes, pos, cardPosRect, new Range<float>(0.5f, 2.0f)));
                    }
                    c++;
                }
                foreach (CardParticles cp in cardParticles)
                    cp.Update(gameTime);
            }
        }

        public void LoadCards(int cards)
        {
            BoosterPack pack = GameComponent.ChosenBooster;

            for (int i = 0; i < cards; i++)
            {
                addRandomCard(pack.Cards, pack.Common, 100);
            }
            addRandomCard(pack.Cards, pack.Rare, pack.R, Attributes.Rarity.RARE);
            addRandomCard(pack.Cards, pack.SuperRare, pack.SR, Attributes.Rarity.SUPER_RARE);
            addRandomCard(pack.Cards, pack.UltraRare, pack.UR, Attributes.Rarity.ULTRA_RARE);

            SaveOpenedCards();
        }

        private void SaveOpenedCards()
        {
            string path = ".\\Data\\openedcards.packs";
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                foreach (Card c in packOfCards)
                {
                    sw.WriteLine(c.cardAttributes.id);
                    if (DeckScreen.AvailableCards.ContainsKey(c.cardAttributes.id))
                    {
                        DeckScreen.AvailableCards[c.cardAttributes.id].quantity++;
                    }
                    else
                    {
                        DeckScreen.AvailableCards.Add(c.cardAttributes.id, DeckScreen.getCard(c.cardAttributes.id));
                    }
                }
            }
        }

        private void addRandomCard(int numCards, List<string> availableCards, int chance, Attributes.Rarity rarity = Attributes.Rarity.COMMON)
        {
            if (random.Next(100) <= chance)
            {
                var chosen = getRandomCard(availableCards, rarity);
                if (chosen == null) return;
                while (packOfCards.Find(x => x.cardAttributes.id == chosen.cardAttributes.id) != null)
                {
                    chosen = getRandomCard(availableCards, rarity);
                    if (chosen == null) return;
                }
                if (packOfCards.Count >= numCards)
                {
                    int index = random.Next(0, packOfCards.Count - 1);
                    packOfCards.RemoveAt(index);
                    packOfCards.Insert(index, chosen);
                }
                else
                {
                    packOfCards.Add(chosen);
                }
            }
        }


        public Card getRandomCard(List<string> availableCards, Attributes.Rarity rarity, float scale = 1.0f)
        {
            Dictionary<string, Texture2D> cards = new Dictionary<string, Texture2D>();
            KeyValuePair<string, Texture2D>? kvp = getCardDataByID(availableCards.ElementAt(random.Next(0, availableCards.Count - 1)), scale);
            if (!kvp.HasValue) return null;

            CardAttributes cAttrib = new CardAttributes();
            cAttrib.rarity = rarity;
            using (DataTable dt = GameComponent.cardsdb.selectQuery("SELECT * FROM datas WHERE id=" + kvp.Value.Key))
            {
                cAttrib.type = Attributes.getTypeByFlag(Convert.ToUInt32(dt.Rows[0].ItemArray[4].ToString()));
                cAttrib.attack = Convert.ToInt32(dt.Rows[0].ItemArray[5].ToString());
                cAttrib.defense = Convert.ToInt32(dt.Rows[0].ItemArray[6].ToString());
                cAttrib.level = Convert.ToInt32(dt.Rows[0].ItemArray[7].ToString());
                cAttrib.race = Attributes.getRaceByFlag(Convert.ToUInt32(dt.Rows[0].ItemArray[8].ToString()));
                cAttrib.element = Attributes.getElementByFlag(Convert.ToUInt32(dt.Rows[0].ItemArray[9].ToString()));
            }
            using (DataTable dt = GameComponent.cardsdb.selectQuery("SELECT name,desc FROM texts WHERE id=" + kvp.Value.Key))
            {
                if (dt.Rows.Count > 0)
                {
                    cAttrib.name = dt.Rows[0].ItemArray[0].ToString();
                    cAttrib.description = dt.Rows[0].ItemArray[1].ToString();
                }
            }
            cAttrib.id = kvp.Value.Key;
            Card c = new Card(cAttrib, kvp.Value.Value);
            c.showTag = true;
            return c;
        }

        public KeyValuePair<string, Texture2D>? getCardDataByID(string selectedID, float scale = 1.0f)
        {
            string cpath = "pics\\" + selectedID + ".jpg";
            if (!File.Exists(cpath)) return null;
            return new KeyValuePair<string, Texture2D>(selectedID,
                Sprite.GetTexture(cpath));
        }

        public override void Dispose()
        {

        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }
    }
}
