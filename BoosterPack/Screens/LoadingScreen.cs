using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Bitmap = System.Drawing.Bitmap;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using System.Data;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework.Graphics;

namespace BoosterPack
{
    public class LoadingScreen : Screen
    {
        private int Progress = 0;
        private mgTimer timer;
        private Rectangle barOutline;
        private Rectangle ProgressBar;
        private bool finished = false;

        public LoadingScreen()
        {
            timer = new mgTimer(25);
            barOutline = new Rectangle(10, GameComponent._mg.windowHeight - 30, (GameComponent._mg.windowWidth - 20), 20);

            ProgressBar = new Rectangle(barOutline.X, barOutline.Y, barOutline.X + (int)(((GameComponent._mg.windowWidth - 30) / 100.0f) * Progress), barOutline.Height);
        }

        public override void Draw(GameTime gameTime, ref MonoGraphics _mg)
        {
            string loadingText = "Loading... %" + Progress;
            float textWidth = _mg.MeasureText(loadingText, 12.0f, true).X;
            _mg.DrawString(loadingText, (_mg.windowWidth - textWidth) / 2, _mg.windowHeight - 50.0f, Color.White, 12.0f, true);
            _mg.DrawFilledRectangle(new Rectangle(ProgressBar.X, ProgressBar.Y, barOutline.X + (int)(((GameComponent._mg.windowWidth - 30) / 100.0f) * Progress), ProgressBar.Height), Color.Orange);
            _mg.DrawRectangle(barOutline, Color.IndianRed, 1);
        }

        public override void Update(GameTime gameTime)
        {
            if (timer.hasTicked(gameTime))
            {
                switch (Progress)
                {
                    case 0:
                        {
                            // Loading our background into the list of controls this component has.
                            GameComponent._ControlsToAdd.Add("background", new Sprite(Sprite.GetTexture(Directory.GetCurrentDirectory() + "\\textures\\bg_deck.png"),
                                new Rectangle(0, 0, GameComponent._mg.windowWidth, GameComponent._mg.windowHeight)));
                            Progress++;
                            break;
                        }
                    case 5:
                        {
                            // Loading our sounds in
                            GameComponent.seAttack = SoundEffect.FromFile(Directory.GetCurrentDirectory() + "\\sound\\attack.wav");
                            GameComponent.seActivate = SoundEffect.FromFile(Directory.GetCurrentDirectory() + "\\sound\\activate.wav");
                            GameComponent.seSummon = SoundEffect.FromFile(Directory.GetCurrentDirectory() + "\\sound\\summon.wav");
                            GameComponent.seDraw = SoundEffect.FromFile(Directory.GetCurrentDirectory() + "\\sound\\draw.wav");

                            Progress++;
                            break;
                        }
                    case 30:
                        {
                            // Parsing the json file for the boosterpacks available to use
                            GameComponent.Boosters = JsonConvert.DeserializeObject<Dictionary<string, BoosterPack>>(File.ReadAllText(Directory.GetCurrentDirectory() + "\\BoosterPacks.json"));

                            // Prelaoding card overlays in the beginning
                            CardAttributes.overlay = Sprite.GetTexture(".\\Data\\booster\\overlay.png");
                            CardAttributes.commonTag = Sprite.GetTexture(".\\Data\\booster\\Common.png");
                            CardAttributes.rareTag = Sprite.GetTexture(".\\Data\\booster\\Rare.png");
                            CardAttributes.superTag = Sprite.GetTexture(".\\Data\\booster\\Super Rare.png");
                            CardAttributes.ultraTag = Sprite.GetTexture(".\\Data\\booster\\Ultra Rare.png");
                            CardAttributes.levelStar = Sprite.GetTexture(".\\Data\\booster\\levelstar.png");

                            DropDownMenu.arrowDown = Sprite.GetTexture(".\\Data\\booster\\arrowdown.png");

                            GUI.RoundedRectangleTexture = Sprite.GetTexture(".\\Data\\booster\\rounded_rect.png");
                            GUI.InitRoundedRectangle();

                            GameComponent.cardRarityDB = new DataClass(Directory.GetCurrentDirectory() + "\\Data\\cardrarity.cdb");

                            // Preload and initialize all booster packs into memory (images/data)
                            foreach (KeyValuePair<string, BoosterPack> kvp in GameComponent.Boosters)
                            {
                                kvp.Value.Preload(kvp.Key);
                                UpdateCardRarityDB(kvp.Value.Common, Attributes.Rarity.COMMON);
                                UpdateCardRarityDB(kvp.Value.Rare, Attributes.Rarity.RARE);
                                UpdateCardRarityDB(kvp.Value.SuperRare, Attributes.Rarity.SUPER_RARE);
                                UpdateCardRarityDB(kvp.Value.UltraRare, Attributes.Rarity.ULTRA_RARE);
                            }

                            // Initalize the access to the card database
                            GameComponent.cardsdb = new DataClass(Directory.GetCurrentDirectory() + "\\expansions\\cards.cdb", true);

                            LoadAvailableCards();

                            Progress++;
                            break;
                        }
                    case 100:
                        Finished();
                        break;
                    default:
                        Progress++;
                        break;
                }
            }
        }

        private void Finished()
        {
            if (finished) return;
            GameComponent.transScreen = new TransitionScreen("MainMenuScreen", new MainMenuScreen());
            finished = true;
        }

        private void UpdateCardRarityDB(List<string> cards, Attributes.Rarity rarity)
        {
            foreach (var vp in cards)
            {
                DataTable queryExist = GameComponent.cardRarityDB.selectQuery("SELECT * FROM cards WHERE ID='" + vp + "';");
                if (queryExist.Rows.Count > 0)
                {
                    if (Convert.ToInt32(queryExist.Rows[0].ItemArray[1]) != (int)rarity)
                    {
                        GameComponent.cardRarityDB.selectQuery("UPDATE cards SET Rarity=" + (int)rarity + " WHERE ID='" + vp + "';");
                    }
                    else continue;
                }
                else
                {
                    GameComponent.cardRarityDB.selectQuery("INSERT INTO cards(`ID`, `Rarity`) VALUES('" + vp + "', " + (int)rarity + ");");
                }
            }
        }

        private void LoadAvailableCards()
        {
            string path = ".\\Data\\openedcards.packs";
            if (!File.Exists(path)) return;
            foreach(string id in File.ReadAllLines(path))
            {
                if (DeckScreen.AvailableCards == null) DeckScreen.AvailableCards = new Dictionary<string, DeckScreen.StashViewCard>();
                if (DeckScreen.AvailableCards.ContainsKey(id))
                {
                    DeckScreen.AvailableCards[id].quantity++;
                }
                else
                {
                    DeckScreen.AvailableCards.Add(id, DeckScreen.getCard(id));
                }
            }
        }

        public static Dictionary<string, DeckScreen.StashViewCard> SortDictionary(Dictionary<string, DeckScreen.StashViewCard> cards, Attributes.Types[] sortOrder)
        {
            List<KeyValuePair<string, DeckScreen.StashViewCard>> _tmpList = cards.ToList();
            Dictionary<string, DeckScreen.StashViewCard> NormalMonsterList = new Dictionary<string, DeckScreen.StashViewCard>();
            Dictionary<string, DeckScreen.StashViewCard> EffectMonsterList = new Dictionary<string, DeckScreen.StashViewCard>();
            Dictionary<string, DeckScreen.StashViewCard> RitualMonsterList = new Dictionary<string, DeckScreen.StashViewCard>();
            Dictionary<string, DeckScreen.StashViewCard> FusionMonsterList = new Dictionary<string, DeckScreen.StashViewCard>();
            Dictionary<string, DeckScreen.StashViewCard> spellList = new Dictionary<string, DeckScreen.StashViewCard>();
            Dictionary<string, DeckScreen.StashViewCard> trapList = new Dictionary<string, DeckScreen.StashViewCard>();

            while (_tmpList.Count > 0)
            {
                for (int i = 0; i < _tmpList.Count; i++)
                {
                    if (CheckSort(_tmpList[i], Attributes.Types.MONSTER))
                    {
                        if (CheckSort(_tmpList[i], Attributes.Types.FUSION))
                        {
                            FusionMonsterList.Add(_tmpList[i].Key, _tmpList[i].Value);
                            _tmpList.RemoveAt(i);
                            break;
                        } else if (CheckSort(_tmpList[i], Attributes.Types.RITUAL))
                        {
                            RitualMonsterList.Add(_tmpList[i].Key, _tmpList[i].Value);
                            _tmpList.RemoveAt(i);
                            break;
                        } else if (CheckSort(_tmpList[i], Attributes.Types.EFFECT))
                        {
                            EffectMonsterList.Add(_tmpList[i].Key, _tmpList[i].Value);
                            _tmpList.RemoveAt(i);
                            break;
                        } else
                        {
                            NormalMonsterList.Add(_tmpList[i].Key, _tmpList[i].Value);
                            _tmpList.RemoveAt(i);
                            break;
                        }
                    } else
                    {
                        if (CheckSort(_tmpList[i], Attributes.Types.SPELL))
                        {
                            spellList.Add(_tmpList[i].Key, _tmpList[i].Value);
                            _tmpList.RemoveAt(i);
                            break;
                        } else if (CheckSort(_tmpList[i], Attributes.Types.TRAP))
                        {
                            trapList.Add(_tmpList[i].Key, _tmpList[i].Value);
                            _tmpList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            Dictionary<Attributes.Types, Dictionary<string, DeckScreen.StashViewCard>> sortOrderList = new Dictionary<Attributes.Types, Dictionary<string, DeckScreen.StashViewCard>>();
            sortOrderList.Add(Attributes.Types.MONSTER, NormalMonsterList);
            sortOrderList.Add(Attributes.Types.FUSION, FusionMonsterList);
            sortOrderList.Add(Attributes.Types.RITUAL, RitualMonsterList);
            sortOrderList.Add(Attributes.Types.EFFECT, EffectMonsterList);
            sortOrderList.Add(Attributes.Types.SPELL, spellList);
            sortOrderList.Add(Attributes.Types.TRAP, trapList);

            Dictionary<string, DeckScreen.StashViewCard> newList = new Dictionary<string, DeckScreen.StashViewCard>();
            foreach (Attributes.Types type in sortOrder)
            {
                foreach(var soli in sortOrderList)
                {
                    if(soli.Key == type)
                    {
                        newList = newList.Concat(soli.Value).ToDictionary(x => x.Key, x => x.Value);
                        break;
                    }
                }
            }
            return newList;
        }

        public static bool CheckSort(KeyValuePair<string, DeckScreen.StashViewCard> svc, Attributes.Types sortType)
        {
            if (svc.Value.card.cardAttributes.type.ToDictionary(x => x.Key, x => x.Value).ContainsValue(sortType))
            {
                return true;
            }
            return false;
        }

        public override void UpdateKeyInput(GameTime gameTime, Keys key, bool keyValue)
        {

        }

        public override void Dispose()
        {

        }
    }
}
