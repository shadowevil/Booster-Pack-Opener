using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BoosterPack
{
    public class DeckScreen : Screen
    {
        private static ScrollableBox StashViewBox;
        private static ScrollableBox DescViewBox;
        public Rectangle DescriptionRect;
        public static Dictionary<string, StashViewCard> AvailableCards;
        private Random random;

        KeyValuePair<string, Texture2D>? placeholderCard;
        private Button BackButton;
        private DropDownMenu filterCardsDropDown;

        public DeckViewArea _MainDeck;
        public DeckViewArea _ExtraDeck;
        public DeckViewArea _SideDeck;
        public static StashViewCard StashSelectedCard;
        public static StashViewCard PreviewCard;

        private Rectangle FilterBounds;

        public DeckScreen()
        {
            // Ensure we have available cards to use if not exit
            if (AvailableCards == null) return;
            // Seed the random
            random = new Random(DateTime.Now.Millisecond);

            // Initialize StashViewBox (Chest full of cards)
            StashViewBox = new ScrollableBox(new Rectangle(GameComponent._mg.windowWidth - 240, GameComponent._mg.windowHeight - (GameComponent._mg.windowHeight / 2) - 140,
                238, GameComponent._mg.windowHeight - (GameComponent._mg.windowHeight - (GameComponent._mg.windowHeight / 2) - 135)), 0,
                (AvailableCards.Where(x => x.Value != null).ElementAt(0).Value.card.sourceRect.Height * 0.35f) + 4, 7);

            // Placeholder card so no empty space
            placeholderCard = getCardDataByID("301");

            // Description area and ScrollableBox area
            DescriptionRect = new Rectangle(0, (int)(placeholderCard.Value.Value.Height * 1.35f) + 5, (int)(placeholderCard.Value.Value.Width * 1.35f), GameComponent._mg.windowHeight - (int)(placeholderCard.Value.Value.Height * 1.35f) - 10);
            DescViewBox = new ScrollableBox(new Rectangle(DescriptionRect.X, DescriptionRect.Y + 80, DescriptionRect.Width, DescriptionRect.Height - 80), 0, 15, 15, 15.0f);

            // Decks to add cards to
            _SideDeck = new DeckViewArea(new Rectangle(DescriptionRect.Width + 8, DescriptionRect.Y + 544, StashViewBox.scrollViewport.X - DescriptionRect.Width - 15, 184), "Side Deck", 15, null);
            _ExtraDeck = new DeckViewArea(new Rectangle(DescriptionRect.Width + 8, DescriptionRect.Y + 360, StashViewBox.scrollViewport.X - DescriptionRect.Width - 15, 184), "Extra Deck", 15, new List<Attributes.Types>{ Attributes.Types.FUSION, Attributes.Types.SYNCHRO, Attributes.Types.XYZ, Attributes.Types.LINK });
            _MainDeck = new DeckViewArea(new Rectangle(DescriptionRect.Width + 8, DescriptionRect.Y, StashViewBox.scrollViewport.X - DescriptionRect.Width - 15, 360), "Main Deck", 60, null);
            
            // Add AvailableCards to the StashViewBox so the user can see the cards
            foreach (var card in AvailableCards)
            {
                card.Value.isScrollable = true;
                StashViewBox._components.Add(card.Key, card.Value);
            }
            // Set the lineheight and scrollspeed of the StashViewBox
            StashViewBox.scrollSpeed = (AvailableCards.Where(x => x.Value != null).ElementAt(0).Value.card.sourceRect.Height * 0.35f) + 4;

            // UI Button to go back to the selection screen
            BackButton = new Button("Exit", (int)(StashViewBox.scrollViewport.X + 152), (int)(StashViewBox.scrollViewport.Y - 22), 85, 20, 0.1f, 10.0f, new Action(delegate
            {
                if (GameComponent.transScreen != null) return;
                GameComponent.transScreen = new TransitionScreen("SelectionScreen", new SelectionScreen());
            }));

            FilterBounds = new Rectangle(DescriptionRect.Width + 8, 5, GameComponent._mg.windowWidth - (DescriptionRect.Width + 11), GameComponent._mg.windowHeight - (DescriptionRect.Height + 15));

            filterCardsDropDown = new DropDownMenu(500, 250, 200, 25, 25.0f, 5, 25.0f, (int)(25.0f * 5));
            filterCardsDropDown.Add("All", "All Cards", new Action(delegate
            {
                var _tmpList = AvailableCards;
                StashViewBox._components.Clear();
                foreach (var card in _tmpList)
                {
                    card.Value.isScrollable = true;
                    StashViewBox._components.Add(card.Key, card.Value);
                }
            }));
            filterCardsDropDown.Add("Normal", "Normal Monster Cards", new Action(delegate
            {
                var _tmpList = LoadingScreen.SortDictionary(AvailableCards, new Attributes.Types[] { Attributes.Types.MONSTER, Attributes.Types.NORMAL });
                StashViewBox._components.Clear();
                foreach (var card in _tmpList)
                {
                    card.Value.isScrollable = true;
                    StashViewBox._components.Add(card.Key, card.Value);
                }
            }));
            filterCardsDropDown.Add("Effect", "Effect Monster Cards", new Action(delegate
            {
                var _tmpList = LoadingScreen.SortDictionary(AvailableCards, new Attributes.Types[] { Attributes.Types.EFFECT });
                StashViewBox._components.Clear();
                foreach (var card in _tmpList)
                {
                    card.Value.isScrollable = true;
                    StashViewBox._components.Add(card.Key, card.Value);
                }
            }));
            filterCardsDropDown.Add("Fusion", "Fusion Monster Cards", new Action(delegate
            {
                var _tmpList = LoadingScreen.SortDictionary(AvailableCards, new Attributes.Types[] { Attributes.Types.FUSION });
                StashViewBox._components.Clear();
                foreach (var card in _tmpList)
                {
                    card.Value.isScrollable = true;
                    StashViewBox._components.Add(card.Key, card.Value);
                }
            }));
            filterCardsDropDown.Add("Ritual", "Ritual Monster Cards", new Action(delegate
            {
                var _tmpList = LoadingScreen.SortDictionary(AvailableCards, new Attributes.Types[] { Attributes.Types.RITUAL });
                StashViewBox._components.Clear();
                foreach (var card in _tmpList)
                {
                    card.Value.isScrollable = true;
                    StashViewBox._components.Add(card.Key, card.Value);
                }
            }));
            filterCardsDropDown.Add("Spell", "Spell Cards", new Action(delegate
                {
                    var _tmpList = LoadingScreen.SortDictionary(AvailableCards, new Attributes.Types[] { Attributes.Types.SPELL });
                    StashViewBox._components.Clear();
                    foreach (var card in _tmpList)
                    {
                        card.Value.isScrollable = true;
                        StashViewBox._components.Add(card.Key, card.Value);
                    }
                }));
            filterCardsDropDown.Add("Trap", "Trap Cards", new Action(delegate
            {
                var _tmpList = LoadingScreen.SortDictionary(AvailableCards, new Attributes.Types[] { Attributes.Types.TRAP });
                StashViewBox._components.Clear();
                foreach (var card in _tmpList)
                {
                    card.Value.isScrollable = true;
                    StashViewBox._components.Add(card.Key, card.Value);
                }
            }));
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            // Don't draw if AvailableCards is null
            if (AvailableCards == null) return;
            // Draw the StashViewBox area
            _mg.DrawFilledRectangle(StashViewBox.scrollViewport.Bounds, Color.Black * 0.90f);
            StashViewBox.Draw(gameTime, ref _mg);
            _mg.DrawRectangle(StashViewBox.scrollViewport.Bounds, Color.IndianRed, 1);
            
            // Drawing the total cards loaded
            Rectangle resultBox = new Rectangle(StashViewBox.scrollViewport.X, StashViewBox.scrollViewport.Y - 20, 150, 20);
            _mg.DrawRectangle(resultBox, Color.IndianRed, 1);
            uint totalCards = 0;
            foreach (var c in StashViewBox._components)
            {
                if ((c.Value as StashViewCard).quantity > 1)
                {
                    totalCards += (c.Value as StashViewCard).quantity;
                }
                else totalCards++;
            }
            _mg.DrawStringWithLabel("Results:", "" + totalCards, resultBox.X + 2, resultBox.Y, Color.White, Color.LightGray, 12.0f);

            _mg.DrawRectangle(FilterBounds, Color.IndianRed, 1);

            // Draw all the deck area's
            _MainDeck.Draw(gameTime, ref _mg);
            _ExtraDeck.Draw(gameTime, ref _mg);
            _SideDeck.Draw(gameTime, ref _mg);

            // UI Button draw
            BackButton.Draw(gameTime, ref _mg);
            Vector2 vec = _mg.MeasureText("Filter by type: ", 12.0f, true);
            _mg.DrawString("Filter by type: ", filterCardsDropDown.location.X - vec.X, filterCardsDropDown.location.Y + ((filterCardsDropDown.DropRect.Height - vec.Y) / 2), Color.White, 12.0f, true);
            filterCardsDropDown.Draw(gameTime, ref _mg);

            // Draw the Description area
            _mg.DrawFilledRectangle(DescriptionRect, Color.Black * 0.8f);
            _mg.DrawRectangle(DescriptionRect, Color.IndianRed, 1);
            DescViewBox.Draw(gameTime, ref _mg);

            // Draw placeholder card
            if (placeholderCard.HasValue)
            {
                _mg.Draw(placeholderCard.Value.Value, new Vector2(0, 0), new Rectangle(0, 0, placeholderCard.Value.Value.Width, placeholderCard.Value.Value.Height),
                    Color.White, 0.0f, Vector2.Zero, 1.35f, SpriteEffects.None, 0.0f);
            }

            // If a card is being hovered over, draw the card for user view
            if (PreviewCard != null)
            {
                PreviewCard.card.location = new Vector2(0, 0);
                PreviewCard.card.showParticles = true;
                PreviewCard.card.Draw(gameTime, ref _mg);

                string typeStr = "";
                {
                    int i = 0;
                    // Loop through the cards types to display the card types
                    foreach (var t in PreviewCard.card.cardAttributes.type)
                    {
                        string tk = t.Key; Attributes.Types tv = t.Value;
                        if (tv == Attributes.Types.MONSTER) continue;
                        if (tv == Attributes.Types.SPSUMMON) tk = "Special Summon";
                        if (i == 2 || i == 1) typeStr += "\\" + tk.Substring(0, 1) + tk.Substring(1, tk.Length - 1).ToLower();
                        else typeStr += tk.Substring(0, 1) + tk.Substring(1, tk.Length - 1).ToLower();
                        i++;
                    }
                }
                // If the card is a spell card then output this
                if (PreviewCard.card.cardAttributes.type.Find(x => x.Value == Attributes.Types.SPELL || x.Value == Attributes.Types.TRAP).Key != null)
                {
                    DescViewBox.scrollViewport.Bounds = new Rectangle(DescriptionRect.X, DescriptionRect.Y + 30, DescriptionRect.Width, DescriptionRect.Height - 30);
                    _mg.DrawString("[" + typeStr + "]", DescriptionRect.X + 2.0f, DescriptionRect.Y + 2.0f, Color.White, 10.0f);
                }
                else
                {   // If the card is a monster type
                    if (PreviewCard.card.cardAttributes.name.Length > 24)
                         _mg.DrawString(PreviewCard.card.cardAttributes.name, DescriptionRect.X + 6.0f, DescriptionRect.Y + 5.0f, Color.White, 10.0f);
                    else _mg.DrawString(PreviewCard.card.cardAttributes.name, DescriptionRect.X + 6.0f, DescriptionRect.Y + 2.0f, Color.White, 12.0f);

                    for (int i = 0; i < PreviewCard.card.cardAttributes.level; i++)
                        _mg.Draw(CardAttributes.levelStar, new Vector2(DescriptionRect.X + 6.0f + (i * 12.0f), DescriptionRect.Y + 25.0f), CardAttributes.levelStar.Bounds, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);


                    string wrapped = _mg.WrapText("[" + PreviewCard.card.cardAttributes.element_string + "] [" + PreviewCard.card.cardAttributes.race_string + "] [" + typeStr + "]", DescriptionRect.Width - 6.0f, 10.0f);
                    Vector2 typeVect = new Vector2(DescriptionRect.X + 2.0f, DescriptionRect.Y + 65.0f);

                    Vector2 attackVect = _mg.MeasureText("ATK: ", 10.0f, true) + _mg.MeasureText(PreviewCard.card.cardAttributes.attack.ToString(), 10.0f);
                    _mg.DrawStringWithLabel("ATK: ", PreviewCard.card.cardAttributes.attack.ToString(), typeVect.X, typeVect.Y - 22.0f, Color.White, Color.White, 10.0f);
                    _mg.DrawStringWithLabel("DEF: ", PreviewCard.card.cardAttributes.defense.ToString(), attackVect.X + 15.0f, typeVect.Y - 22.0f, Color.White, Color.White, 10.0f);

                    _mg.DrawString(wrapped, typeVect.X, typeVect.Y, Color.White, 10.0f);
                    DescViewBox.scrollViewport.Bounds = new Rectangle(DescriptionRect.X, DescriptionRect.Y + 85 + (int)(15.0f * wrapped.ToCharArray().Count(x => x == '\n')), DescriptionRect.Width, DescriptionRect.Height - 85 - (int)(15.0f * wrapped.ToCharArray().Count(x => x == '\n')));
                }
                // Draw the description based on the hovered card
                string desc = PreviewCard.card.cardAttributes.description;
                desc = _mg.WrapText(desc, DescriptionRect.Width - 18.0f, 10.0f);

                // Here we are invoking the Description Viewbox to draw
                DescViewBox.DrawString(ref _mg, desc, 6.0f, 2.0f - DescViewBox.scrollValue, Color.LightGray, 10.0f);
            }

            // Checks if we are dragging a card, if we release, it catches if we released the mouse or not as well
            if (StashSelectedCard != null || _MainDeck.DeckSelectedCard != null || _SideDeck.DeckSelectedCard != null || _ExtraDeck.DeckSelectedCard != null)
            {
                StashViewCard selectedCard = null;
                if (StashSelectedCard != null) selectedCard = StashSelectedCard;
                else if (_MainDeck.DeckSelectedCard != null) selectedCard = _MainDeck.DeckSelectedCard;
                else if (_ExtraDeck.DeckSelectedCard != null) selectedCard = _ExtraDeck.DeckSelectedCard;
                else if (_SideDeck.DeckSelectedCard != null) selectedCard = _SideDeck.DeckSelectedCard;
                if (winmain._mouse.state.LeftButton == ButtonState.Released)
                {
                    StashSelectedCard = null;
                    _MainDeck.DeckSelectedCard = null;
                    _ExtraDeck.DeckSelectedCard = null;
                    _SideDeck.DeckSelectedCard = null;
                    selectedCard = null;
                    return;
                }
                if (selectedCard == null) return;
                // Drawing a ghost card to visually show we are dragging a card
                _mg.Draw(selectedCard.card.card, new Vector2(winmain._mouse.mousePosition.X - (selectedCard.card.sourceRect.Width * 0.35f) / 2, winmain._mouse.mousePosition.Y - (selectedCard.card.sourceRect.Height * 0.35f) / 2),
                    selectedCard.card.sourceRect, Color.White * 0.75f, 0.0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0.0f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Only update if AvailableCards aren't null
            if (AvailableCards == null) return;

            // Handling what's actually visible to the user and checking if there is a card picked up or not.
            if (StashViewBox.ExtVisible.Contains(winmain._mouse.mousePosition))
            {
                foreach (var stashviewcard in StashViewBox._components)
                {
                    if (!(stashviewcard.Value is StashViewCard)) continue;
                    if ((stashviewcard.Value as StashViewCard).PickedUp == true && StashSelectedCard == null)
                    {
                        PreviewCard = (StashViewCard)(stashviewcard.Value as StashViewCard).Clone(1.35f);
                        StashSelectedCard = (StashViewCard)(stashviewcard.Value as StashViewCard).Clone();
                        (stashviewcard.Value as StashViewCard).PickedUp = false;
                        break;
                    }
                    if (PreviewCard == null) PreviewCard = (StashViewCard)(stashviewcard.Value as StashViewCard).Clone(1.35f);
                    if ((stashviewcard.Value as StashViewCard).rect.Contains(winmain._mouse.mousePosition) && PreviewCard.card.cardAttributes.id != (stashviewcard.Value as StashViewCard).card.cardAttributes.id)
                        PreviewCard = (StashViewCard)(stashviewcard.Value as StashViewCard).Clone(1.35f);
                }
            }
            
            // Update all of our UI features
            _ExtraDeck.Update(gameTime);
            _MainDeck.Update(gameTime);
            _SideDeck.Update(gameTime);

            BackButton.Update(gameTime);
            filterCardsDropDown.Update(gameTime);

            StashViewBox.Update(gameTime);
            DescViewBox.Update(gameTime);

            // Only update the PreviewCard as necessary
            if (PreviewCard == null) return;
            PreviewCard.Update(gameTime);
        }

        public override void Dispose()
        {

        }

        public static KeyValuePair<string, Texture2D>? getCardDataByID(string selectedID)
        {
            string cpath = "pics\\" + selectedID + ".jpg";
            if (!File.Exists(cpath)) return null;
            return new KeyValuePair<string, Texture2D>(selectedID,
                Sprite.GetTexture(cpath));
        }

        public static StashViewCard getCard(string selectedID)
        {
            Dictionary<string, Texture2D> cards = new Dictionary<string, Texture2D>();
            KeyValuePair<string, Texture2D>? kvp = getCardDataByID(selectedID);
            if (!kvp.HasValue) return null;

            CardAttributes cAttrib = new CardAttributes();
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
            using(DataTable dt = GameComponent.cardRarityDB.selectQuery("SELECT * FROM cards WHERE ID=\"" + cAttrib.id + "\";"))
            {
                cAttrib.rarity = (Attributes.Rarity)Convert.ToInt32(dt.Rows[0].ItemArray[1]);
            }
            return new StashViewCard(cAttrib, kvp.Value.Value, 0.33f);
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {
            if (AvailableCards == null) return;
            // Allows the StashViewBox to scroll fast using arrow keys up/down
            StashViewBox.UpdateKeyInput(gameTime, key, keyValue);
        }

        public class StashViewCard : Component, ICloneable
        {
            public uint quantity = 1;
            public Card card;
            public Rectangle rect;
            public bool PickedUp = false;
            public bool isScrollable = false;
            private Random random;

            public StashViewCard(CardAttributes cAttrib, Texture2D CardTexture, float cardScale)
            {
                random = new Random(DateTime.Now.Millisecond);
                this.location = new Vector2(0, 0);
                card = new Card(cAttrib, CardTexture, true, location.X + 2, location.Y + 2, cardScale);
            }

            public object Clone()
            {
                StashViewCard c = new StashViewCard(card.cardAttributes, card.card, card.cardScale);
                c.quantity = 1;
                return c;
            }

            public object Clone(float cardScale)
            {
                StashViewCard c = new StashViewCard(card.cardAttributes, card.card, cardScale);
                c.quantity = 1;
                return c;
            }

            public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
            {
                if (StashViewBox.IntVisible.Contains(location))
                {
                    if (rect.Contains(winmain._mouse.mousePosition))
                    {
                        _mg.DrawFilledRectangle(new Rectangle(0, (int)((((card.sourceRect.Height * 0.35f) * arrayPosition) + (4 * arrayPosition)) - StashViewBox.scrollValue),
                            rect.Width, rect.Height + 2), Color.White * 0.15f);
                    }

                    card.Draw(gameTime, ref _mg);

                    // Card Attributes
                    _mg.DrawString(card.cardAttributes.name, location.X + (card.sourceRect.Width * 0.35f) + 4, location.Y + 2, Color.White, 10.0f, true);
                    string quantityText = "Quantity: " + quantity;
                    Vector2 wh = _mg.MeasureText(quantityText, 10.0f, true);
                    _mg.DrawString(quantityText, rect.Width - wh.X - 15, (int)((((card.sourceRect.Height * 0.35f) * arrayPosition) + (4 * arrayPosition) + rect.Height - wh.Y) - StashViewBox.scrollValue), Color.White, 10.0f, true);
                    // Checking if Monster card or Spell\Trap card
                    if (card.cardAttributes.element.HasValue)
                    {   // Monster Card
                        string ElementRace = card.cardAttributes.element.Value.Key;
                        if(card.cardAttributes.race.HasValue) ElementRace += " | " + card.cardAttributes.race.Value.Key;
                        _mg.DrawString(ElementRace, location.X + (card.sourceRect.Width * 0.35f) + 4, location.Y + 2 + (15.0f * 1.0f), Color.Gray, 10.0f);
                        string AttackDefense = "ATK:" + card.cardAttributes.attack + "  DEF:" + card.cardAttributes.defense;
                        _mg.DrawString(AttackDefense, location.X + (card.sourceRect.Width * 0.35f) + 4, location.Y + 2 + (15.0f * 2.0f), Color.LightGray, 10.0f);
                        for(int i=0;i<card.cardAttributes.level;i++)
                            _mg.Draw(CardAttributes.levelStar, new Vector2(location.X + (card.sourceRect.Width * 0.35f) + 4 + (i * 12.0f), location.Y + 2 + (15.0f * 3.5f)), CardAttributes.levelStar.Bounds, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                    }
                    else
                    {   // Spell\Trap card
                        string typeString = "";
                        int i = 0;
                        foreach (var type in card.cardAttributes.type)
                        {
                            if (i == 1) typeString += " | ";
                            typeString += type.Key;
                            i++;
                        }
                        _mg.DrawString(typeString, location.X + (card.sourceRect.Width * 0.35f) + 4, location.Y + 2 + (15.0f * 1.0f), Color.Gray, 10.0f);
                    }
                }
            }

            public override void Update(GameTime gameTime)
            {
                if (isScrollable)
                {
                    this.location = new Vector2(0, (((card.sourceRect.Height * 0.35f) * arrayPosition) + (4 * arrayPosition)) - StashViewBox.scrollValue);
                    card.location = new Vector2(location.X + 2, location.Y + 2);
                    rect = new Rectangle(new Point(StashViewBox.scrollViewport.Bounds.X + (int)location.X, StashViewBox.scrollViewport.Bounds.Y + (int)location.Y), new Point(StashViewBox.scrollViewport.Width, (int)(card.sourceRect.Height * 0.35f)));
                }
                card.Update(gameTime);

                if (StashViewBox.IntVisible.Contains(location))
                {
                    if (rect.Contains(winmain._mouse.mousePosition))
                    {
                        //selectedCard = new KeyValuePair<int, StashViewCard>(arrayPosition, (StashViewCard)Clone());
                        DescViewBox.scrollValue = 0;

                        if (winmain._mouse.state.LeftButton == ButtonState.Pressed)
                        {
                            PickedUp = true;
                        } else if(winmain._mouse.state.RightButton == ButtonState.Pressed)
                        {
                            PickedUp = true;
                        }
                    }
                }
            }

            public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
            {

            }
        }

        public class DeckViewArea : Component
        {
            public Rectangle DeckRect;
            public List<StashViewCard> CardList;
            private string DeckViewTitle;
            public float cardScale = 0.3825f;
            private int DeckMaxSize = 0;
            private int MovingCardsFromDeckIndex = -1;
            public bool MovingCardFromStash = false;
            public bool MovingCardFromDeck = false;
            public StashViewCard DeckSelectedCard;
            public List<Attributes.Types> acceptedTypes;
            public List<Attributes.Types> disallowedTypes;

            public DeckViewArea(Rectangle bounds, string areaname, int maxSizeAllowed, List<Attributes.Types> types)
            {
                CardList = new List<StashViewCard>();
                DeckRect = bounds;
                DeckViewTitle = areaname;
                DeckMaxSize = maxSizeAllowed;
                acceptedTypes = types;
                if(acceptedTypes == null)
                {
                    disallowedTypes = new List<Attributes.Types> { Attributes.Types.FUSION, Attributes.Types.SYNCHRO, Attributes.Types.XYZ, Attributes.Types.LINK };
                }
            }

            public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
            {
                Rectangle DeckHeader = new Rectangle(DeckRect.X, DeckRect.Y, DeckRect.Width, 25);
                _mg.DrawFilledRectangle(DeckRect, Color.Black * 0.75f);
                _mg.DrawFilledRectangle(DeckHeader, Color.OrangeRed);
                _mg.DrawString(DeckViewTitle, DeckHeader.X + 15.0f, DeckHeader.Y + 2.5f, Color.White, 12.0f, true);
                _mg.DrawRectangle(DeckRect, Color.IndianRed, 1);

                int x = 0;
                int y = 0;
                foreach (StashViewCard c in CardList)
                {
                    int numPerRow = (int)((DeckRect.Width) / (c.card.sourceRect.Width * cardScale));
                    if (x >= 1)
                    {
                        if (x % numPerRow == 0)
                        {
                            y++;
                            x = 0;
                        }
                    }
                    c.location = new Vector2(DeckRect.X + (x * (c.card.sourceRect.Width * cardScale)) + 2.0f,
                        DeckRect.Y + 30 + (y * (c.card.sourceRect.Height * cardScale)));
                    _mg.Draw(c.card.card, c.location, c.card.sourceRect, Color.White,
                        0.0f, Vector2.Zero, cardScale, SpriteEffects.None, 0.0f);
                    x++;
                }

                x = 0;
                y = 0;
                foreach (StashViewCard c in CardList)
                {
                    int numPerRow = (int)((DeckRect.Width) / (c.card.sourceRect.Width * cardScale));
                    if (x >= 1)
                    {
                        if (x % numPerRow == 0)
                        {
                            y++;
                            x = 0;
                        }
                    }
                    Rectangle rect = new Rectangle((int)c.location.X, (int)c.location.Y, (int)(c.card.sourceRect.Width * cardScale + 0.1f), (int)(c.card.sourceRect.Height * cardScale + 0.1f));
                    if (rect.Contains(winmain._mouse.mousePosition))
                    {
                        _mg.Draw(c.card.card, new Vector2((c.location.X + rect.Width / 2) - rect.Width * 0.65f, (c.location.Y + rect.Height / 2) - rect.Width * 0.9f), c.card.sourceRect, Color.White,
                        0.0f, Vector2.Zero, cardScale + 0.1f, SpriteEffects.None, 0.0f);
                    }
                    x++;
                }
            }

            public override void Update(GameTime gameTime)
            {
                if (DeckScreen.StashSelectedCard != null)
                {
                    bool cont = false;
                    if (acceptedTypes != null)
                    {
                        for(int j=0;j<acceptedTypes.Count;j++)
                        {
                            for(int i=0;i<DeckScreen.StashSelectedCard.card.cardAttributes.type.Count;i++)
                            {
                                if (DeckScreen.StashSelectedCard.card.cardAttributes.type[i].Value == acceptedTypes[j])
                                {
                                    cont = true;
                                    break;
                                }
                            }
                        }
                    } else
                    {
                        cont = true;
                        for (int j = 0; j < disallowedTypes.Count; j++)
                        {
                            for (int i = 0; i < DeckScreen.StashSelectedCard.card.cardAttributes.type.Count; i++)
                            {
                                if (DeckScreen.StashSelectedCard.card.cardAttributes.type[i].Value == disallowedTypes[j])
                                {
                                    cont = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (cont)
                    {
                        if(DeckRect.Contains(winmain._mouse.mousePosition) && winmain._mouse.state.LeftButton == ButtonState.Released && winmain._mouse.lastState.LeftButton == ButtonState.Pressed) canPutInDeck();
                        if (winmain._mouse.state.RightButton == ButtonState.Released && winmain._mouse.lastState.RightButton == ButtonState.Pressed) canPutInDeck();
                    }
                }

                for (int i = 0; i < CardList.Count; i++)
                {
                    StashViewCard card = CardList[i];
                    float cardScale = 0.3825f;
                    Rectangle rect = new Rectangle((int)card.location.X, (int)card.location.Y, (int)(card.card.sourceRect.Width * cardScale), (int)(card.card.sourceRect.Height * cardScale));
                    if (rect.Contains(winmain._mouse.mousePosition) && PreviewCard.card.cardAttributes.id != card.card.cardAttributes.id)
                    {
                        PreviewCard = (StashViewCard)card.Clone(1.35f);
                    }
                    if (rect.Contains(winmain._mouse.mousePosition))
                    {
                        DeckSelectedCard = (StashViewCard)card.Clone();
                        if ((winmain._mouse.state.LeftButton == ButtonState.Pressed
                            || winmain._mouse.lastState.RightButton == ButtonState.Pressed && winmain._mouse.state.RightButton == ButtonState.Released)
                            && !MovingCardFromDeck && !MovingCardFromStash)
                        {
                            MovingCardsFromDeckIndex = i;
                            MovingCardFromDeck = true;
                            winmain._mouse.lastClickPosition = winmain._mouse.mousePosition;
                        }
                    }
                }

                if (!DeckRect.Contains(winmain._mouse.lastClickPosition)) return;
                if (DeckSelectedCard == null) return;
                if ((winmain._mouse.lastState.LeftButton == ButtonState.Pressed && winmain._mouse.state.LeftButton == ButtonState.Released)
                || (winmain._mouse.lastState.RightButton == ButtonState.Pressed && winmain._mouse.state.RightButton == ButtonState.Released)
                && MovingCardFromDeck && MovingCardsFromDeckIndex != -1)
                {
                    MovingCardFromDeck = false;
                    if (StashViewBox.scrollViewport.Bounds.Contains(winmain._mouse.mousePosition) || (winmain._mouse.lastState.RightButton == ButtonState.Pressed && winmain._mouse.state.RightButton == ButtonState.Released))
                    {
                        if (DeckSelectedCard != null)
                        {
                            if (AvailableCards.ContainsKey(DeckSelectedCard.card.cardAttributes.id))
                            {
                                if (MovingCardsFromDeckIndex == -1) return;
                                AvailableCards[DeckSelectedCard.card.cardAttributes.id].quantity++;
                                CardList.RemoveAt(MovingCardsFromDeckIndex);
                                MovingCardsFromDeckIndex = -1;
                                DeckSelectedCard = null;
                            }
                            else
                            {
                                if (MovingCardsFromDeckIndex == -1) return;
                                AvailableCards.Add(DeckSelectedCard.card.cardAttributes.id, (StashViewCard)DeckSelectedCard.Clone());
                                StashViewBox._components.Clear();
                                foreach (var card in AvailableCards)
                                {
                                    card.Value.isScrollable = true;
                                    StashViewBox._components.Add(card.Key, card.Value);
                                }
                                CardList.RemoveAt(MovingCardsFromDeckIndex);
                                MovingCardsFromDeckIndex = -1;
                                DeckSelectedCard = null;
                            }
                        }
                    }
                }
            }

            public void canPutInDeck()
            {
                bool canPutInDeck = CardList.Count <= DeckMaxSize;

                if (canPutInDeck)
                {
                    if (CardList.FindAll(x => x.card.cardAttributes.id == DeckScreen.StashSelectedCard.card.cardAttributes.id).Count >= 3) return;
                    CardList.Add((StashViewCard)DeckScreen.StashSelectedCard.Clone());

                    if (AvailableCards[DeckScreen.StashSelectedCard.card.cardAttributes.id].quantity <= 1)
                    {
                        bool found = false;
                        foreach (var card in StashViewBox._components)
                        {
                            if (!(card.Value is StashViewCard)) continue;
                            if ((card.Value as StashViewCard).arrayPosition == DeckScreen.StashSelectedCard.arrayPosition + 1) found = true;
                            if (!found) continue;
                            (card.Value as StashViewCard).arrayPosition--;
                        }
                        AvailableCards.Remove(DeckScreen.StashSelectedCard.card.cardAttributes.id);
                        StashViewBox._components.Remove(DeckScreen.StashSelectedCard.card.cardAttributes.id);
                        DeckScreen.StashSelectedCard = null;
                    }
                    else if (AvailableCards[DeckScreen.StashSelectedCard.card.cardAttributes.id].quantity > 1)
                    {
                        AvailableCards[DeckScreen.StashSelectedCard.card.cardAttributes.id].quantity--;
                        //(StashViewBox._components[DeckScreen.StashSelectedCard.card.cardAttributes.id] as StashViewCard).quantity--;
                        DeckScreen.StashSelectedCard = null;
                    }
                }
            }

            public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
            {

            }
        }
    }
}
