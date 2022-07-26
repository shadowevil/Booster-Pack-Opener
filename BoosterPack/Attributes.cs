using System;
using System.Collections.Generic;

namespace BoosterPack
{
    /*
        #define TYPE_MONSTER     0x1
        #define TYPE_SPELL       0x2
        #define TYPE_TRAP        0x4
        #define TYPE_NORMAL      0x10
        #define TYPE_EFFECT      0x20
        #define TYPE_FUSION      0x40
        #define TYPE_RITUAL      0x80
        #define TYPE_TRAPMONSTER 0x100
        #define TYPE_SPIRIT      0x200
        #define TYPE_UNION       0x400
        #define TYPE_GEMINI      0x800
        #define TYPE_TUNER       0x1000
        #define TYPE_SYNCHRO     0x2000
        #define TYPE_TOKEN       0x4000
        #define TYPE_MAXIMUM     0x8000
        #define TYPE_QUICKPLAY   0x10000
        #define TYPE_CONTINUOUS  0x20000
        #define TYPE_EQUIP       0x40000
        #define TYPE_FIELD       0x80000
        #define TYPE_COUNTER     0x100000
        #define TYPE_FLIP        0x200000
        #define TYPE_TOON        0x400000
        #define TYPE_XYZ         0x800000
        #define TYPE_PENDULUM    0x1000000
        #define TYPE_SPSUMMON    0x2000000
        #define TYPE_LINK        0x4000000
 */
    public static class Attributes
    {
        public static List<KeyValuePair<string, Types>> getTypeByFlag(uint iType)
        {
            List<KeyValuePair<string, Types>> typeNames = new List<KeyValuePair<string, Types>>();
            foreach (uint flag in Enum.GetValues(typeof(Types)))
            {
                if ((iType & flag) != 0)
                {
                    typeNames.Add(new KeyValuePair<string, Types>(Enum.GetName(typeof(Attributes.Types), flag), (Types)flag));
                }
            }
            return typeNames;
        }

        public enum Types : uint
        {
            MONSTER = 0x1,
            SPELL = 0x2,
            TRAP = 0x4,
            NORMAL = 0x10,
            EFFECT = 0x20,
            FUSION = 0x40,
            RITUAL = 0x80,
            TRAPMONSTER = 0x100,
            SPIRIT = 0x200,
            UNION = 0x400,
            GEMINI = 0x800,
            TUNER = 0x1000,
            SYNCHRO = 0x2000,
            TOKEN = 0x4000,
            MAXIMUM = 0x8000,
            QUICKPLAY = 0x10000,
            CONTINUOUS = 0x20000,
            EQUIP = 0x40000,
            FIELD = 0x80000,
            COUNTER = 0x100000,
            FLIP = 0x200000,
            TOON = 0x400000,
            XYZ = 0x800000,
            PENDULUM = 0x1000000,
            SPSUMMON = 0x2000000,
            LINK = 0x4000000
        }

        public static KeyValuePair<string, Race>? getRaceByFlag(uint flag)
        {
            foreach(uint _flag in Enum.GetValues(typeof(Race)))
            {
                if((flag & _flag) != 0)
                {
                    return new KeyValuePair<string, Race>(Enum.GetName(typeof(Race), _flag), (Race)_flag);
                }
            }
            return null;
        }

        public enum Race : uint
        {
            WARRIOR = 0x1,
            SPELLCASTER = 0x2,
            FAIRY = 0x4,
            FIEND = 0x8,
            ZOMBIE = 0x10,
            MACHINE = 0x20,
            AQUA = 0x40,
            PYRO = 0x80,
            ROCK = 0x100,
            WINGEDBEAST = 0x200,
            PLANT = 0x400,
            INSECT = 0x800,
            THUNDER = 0x1000,
            DRAGON = 0x2000,
            BEAST = 0x4000,
            BEASTWARRIOR = 0x8000,
            DINOSAUR = 0x10000,
            FISH = 0x20000,
            SEASERPENT = 0x40000,
            REPTILE = 0x80000,
            PSYCHIC = 0x100000,
            DIVINE = 0x200000,
            CREATORGOD = 0x400000,
            WYRM = 0x800000,
            CYBERSE = 0x1000000,
            CYBORG = 0x2000000
        }

        public static KeyValuePair<string, Element>? getElementByFlag(uint flag)
        {
            foreach(uint _flag in Enum.GetValues(typeof(Element)))
            {
                if((flag & _flag) != 0)
                {
                    return new KeyValuePair<string, Element>(Enum.GetName(typeof(Element), _flag), (Element)_flag);
                }
            }
            return null;
        }

        public enum Element : uint
        {
            EARTH  = 0x01,
            WATER  = 0x02,
            FIRE   = 0x04,
            WIND   = 0x08,
            LIGHT  = 0x10,
            DARK   = 0x20,
            DIVINE = 0x40
        }

        public enum Rarity : uint
        {
            COMMON = 0,
            RARE = 1,
            SUPER_RARE = 2,
            ULTRA_RARE = 3
        }
    }
}
