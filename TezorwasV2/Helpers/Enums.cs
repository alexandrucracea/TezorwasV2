
using System.ComponentModel;

namespace TezorwasV2.Helpers
{
    public static class Enums
    {
        public enum Paths
        {
            persons,
            profiles,
            completions,
            articles
        }

        public enum StatusCodes
        {
            Success = 200,
            InternalServerError = 500,
            NotFound = 404,
            BadRequest = 400

        }

        public enum WasteCategory
        {
            EcoHero = 1, //lowest level of waste
            GreenGuru = 3, //medium level of waste
            PlanetSaver = 5 //highest level of waste
        }

        public enum Level
        {
            [Description("Level 1")]
            Lvl1 = 99, //100
            [Description("Level 2")]
            Lvl2 = 299,
            [Description("Level 3")]
            Lvl3 = 599,
            [Description("Level 4")]
            Lvl4 = 999,
            [Description("Level 5")]
            Lvl5 = 1499,
            [Description("Level 6")]
            Lvl6 = 2099,
            [Description("Level 7")]
            Lvl7 = 2799,
            [Description("Level 8")]
            Lvl8 = 3599,
            [Description("Level 9")]
            Lvl9 = 4499,
            [Description("Level 10")]
            Lvl10 = 5499,
            [Description("Level 11")]
            Lvl11 = 6599,
            [Description("Level 12")]
            Lvl12 = 7799,
            [Description("Level 13")]
            Lvl13 = 9099,
            [Description("Level 14")]
            Lvl14 = 10499,
            [Description("Level 15")]
            Lvl15 = 11999,
            [Description("Level 16")]
            Lvl16 = 13599,
            [Description("Level 17")]
            Lvl17 = 15299,
            [Description("Level 18")]
            Lvl18 = 17099,
            [Description("Level 19")]
            Lvl19 = 18999,
            [Description("Level 20")]
            Lvl20 = 20999,
            [Description("Level 21")]
            Lvl21 = 23099,
            [Description("Level 22")]
            Lvl22 = 25299,
            [Description("Level 23")]
            Lvl23 = 27599,
            [Description("Level 24")]
            Lvl24 = 29999,
            [Description("Level 25")]
            Lvl25 = 32499,
            [Description("Level 26")]
            Lvl26 = 35099,
            [Description("Level 27")]
            Lvl27 = 37799,
            [Description("Level 28")]
            Lvl28 = 40599,
            [Description("Level 29")]
            Lvl29 = 43499,
            [Description("Level 30")]
            Lvl30 = 46499,
            [Description("Level 31")]
            Lvl31 = 49599,
            [Description("Level 32")]
            Lvl32 = 52799,
            [Description("Level 33")]
            Lvl33 = 56099,
            [Description("Level 34")]
            Lvl34 = 59499,
            [Description("Level 35")]
            Lvl35 = 62999,
            [Description("Level 36")]
            Lvl36 = 66599,
            [Description("Level 37")]
            Lvl37 = 70299,
            [Description("Level 38")]
            Lvl38 = 74099,
            [Description("Level 39")]
            Lvl39 = 77999,
            [Description("Level 40")]
            Lvl40 = 81999,
            [Description("Level 41")]
            Lvl41 = 86099,
            [Description("Level 42")]
            Lvl42 = 90299,
            [Description("Level 43")]
            Lvl43 = 94599,
            [Description("Level 44")]
            Lvl44 = 98999,
            [Description("Level 45")]
            Lvl45 = 103499,
            [Description("Level 46")]
            Lvl46 = 108099,
            [Description("Level 47")]
            Lvl47 = 112799,
            [Description("Level 48")]
            Lvl48 = 117599,
            [Description("Level 49")]
            Lvl49 = 122499,
            [Description("Level 50")]
            Lvl50 = 127499,
            [Description("Level 51")]
            Lvl51 = 132599,
            [Description("Level 52")]
            Lvl52 = 137799,
            [Description("Level 53")]
            Lvl53 = 143099,
            [Description("Level 54")]
            Lvl54 = 148499,
            [Description("Level 55")]
            Lvl55 = 153999,
            [Description("Level 56")]
            Lvl56 = 159599,
            [Description("Level 57")]
            Lvl57 = 165299,
            [Description("Level 58")]
            Lvl58 = 171099,
            [Description("Level 59")]
            Lvl59 = 176999,
            [Description("Level 60")]
            Lvl60 = 183000,
            [Description("Level 61")]
            Lvl61 = 189099,
            [Description("Level 62")]
            Lvl62 = 195299,
            [Description("Level 63")]
            Lvl63 = 201599,
            [Description("Level 64")]
            Lvl64 = 208000,
            [Description("Level 65")]
            Lvl65 = 214499,
            [Description("Level 66")]
            Lvl66 = 221099,
            [Description("Level 67")]
            Lvl67 = 227799,
            [Description("Level 68")]
            Lvl68 = 234599,
            [Description("Level 69")]
            Lvl69 = 241499,
            [Description("Level 70")]
            Lvl70 = 248499,
            [Description("Level 71")]
            Lvl71 = 255599,
            [Description("Level 72")]
            Lvl72 = 262799,
            [Description("Level 73")]
            Lvl73 = 270099,
            [Description("Level 74")]
            Lvl74 = 277499,
            [Description("Level 75")]
            Lvl75 = 284999,
            [Description("Level 76")]
            Lvl76 = 292599,
            [Description("Level 77")]
            Lvl77 = 300299,
            [Description("Level 78")]
            Lvl78 = 308099,
            [Description("Level 79")]
            Lvl79 = 315999,
            [Description("Level 80")]
            Lvl80 = 324999,
            [Description("Level 81")]
            Lvl81 = 332999,
            [Description("Level 82")]
            Lvl82 = 341099,
            [Description("Level 83")]
            Lvl83 = 349299,
            [Description("Level 84")]
            Lvl84 = 357599,
            [Description("Level 85")]
            Lvl85 = 365999,
            [Description("Level 86")]
            Lvl86 = 374499,
            [Description("Level 87")]
            Lvl87 = 383099,
            [Description("Level 88")]
            Lvl88 = 391799,
            [Description("Level 89")]
            Lvl89 = 400599,
            [Description("Level 90")]
            Lvl90 = 409499,
            [Description("Level 91")]
            Lvl91 = 418499,
            [Description("Level 92")]
            Lvl92 = 427599,
            [Description("Level 93")]
            Lvl93 = 436799,
            [Description("Level 94")]
            Lvl94 = 446099,
            [Description("Level 95")]
            Lvl95 = 455499,
            [Description("Level 96")]
            Lvl96 = 464999,
            [Description("Level 97")]
            Lvl97 = 474599,
            [Description("Level 98")]
            Lvl98 = 484299,
            [Description("Level 99")]
            Lvl99 = 494099,
            [Description("Level 100")]
            Lvl100 = 505000

        }
    }
}
