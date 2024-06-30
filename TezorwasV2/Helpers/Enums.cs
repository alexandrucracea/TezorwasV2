
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
            articles,
            forgotPassword
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
            [Description("1")]
            Lvl1 = 99, //100
            [Description("2")]
            Lvl2 = 299,
            [Description("3")]
            Lvl3 = 599,
            [Description("4")]
            Lvl4 = 999,
            [Description("5")]
            Lvl5 = 1499,
            [Description("6")]
            Lvl6 = 2099,
            [Description("7")]
            Lvl7 = 2799,
            [Description("8")]
            Lvl8 = 3599,
            [Description("9")]
            Lvl9 = 4499,
            [Description("10")]
            Lvl10 = 5499,
            [Description("11")]
            Lvl11 = 6599,
            [Description("12")]
            Lvl12 = 7799,
            [Description("13")]
            Lvl13 = 9099,
            [Description("14")]
            Lvl14 = 10499,
            [Description("15")]
            Lvl15 = 11999,
            [Description("16")]
            Lvl16 = 13599,
            [Description("17")]
            Lvl17 = 15299,
            [Description("18")]
            Lvl18 = 17099,
            [Description("19")]
            Lvl19 = 18999,
            [Description("20")]
            Lvl20 = 20999,
            [Description("21")]
            Lvl21 = 23099,
            [Description("22")]
            Lvl22 = 25299,
            [Description("23")]
            Lvl23 = 27599,
            [Description("24")]
            Lvl24 = 29999,
            [Description("25")]
            Lvl25 = 32499,
            [Description("26")]
            Lvl26 = 35099,
            [Description("27")]
            Lvl27 = 37799,
            [Description("28")]
            Lvl28 = 40599,
            [Description("29")]
            Lvl29 = 43499,
            [Description("30")]
            Lvl30 = 46499,
            [Description("31")]
            Lvl31 = 49599,
            [Description("32")]
            Lvl32 = 52799,
            [Description("33")]
            Lvl33 = 56099,
            [Description("34")]
            Lvl34 = 59499,
            [Description("35")]
            Lvl35 = 62999,
            [Description("36")]
            Lvl36 = 66599,
            [Description("37")]
            Lvl37 = 70299,
            [Description("38")]
            Lvl38 = 74099,
            [Description("39")]
            Lvl39 = 77999,
            [Description("40")]
            Lvl40 = 81999,
            [Description("41")]
            Lvl41 = 86099,
            [Description("42")]
            Lvl42 = 90299,
            [Description("43")]
            Lvl43 = 94599,
            [Description("44")]
            Lvl44 = 98999,
            [Description("45")]
            Lvl45 = 103499,
            [Description("46")]
            Lvl46 = 108099,
            [Description("47")]
            Lvl47 = 112799,
            [Description("48")]
            Lvl48 = 117599,
            [Description("49")]
            Lvl49 = 122499,
            [Description("50")]
            Lvl50 = 127499,
            [Description("51")]
            Lvl51 = 132599,
            [Description("52")]
            Lvl52 = 137799,
            [Description("53")]
            Lvl53 = 143099,
            [Description("54")]
            Lvl54 = 148499,
            [Description("55")]
            Lvl55 = 153999,
            [Description("56")]
            Lvl56 = 159599,
            [Description("57")]
            Lvl57 = 165299,
            [Description("58")]
            Lvl58 = 171099,
            [Description("59")]
            Lvl59 = 176999,
            [Description("60")]
            Lvl60 = 183000,
            [Description("61")]
            Lvl61 = 189099,
            [Description("62")]
            Lvl62 = 195299,
            [Description("63")]
            Lvl63 = 201599,
            [Description("64")]
            Lvl64 = 208000,
            [Description("65")]
            Lvl65 = 214499,
            [Description("66")]
            Lvl66 = 221099,
            [Description("67")]
            Lvl67 = 227799,
            [Description("68")]
            Lvl68 = 234599,
            [Description("69")]
            Lvl69 = 241499,
            [Description("70")]
            Lvl70 = 248499,
            [Description("71")]
            Lvl71 = 255599,
            [Description("72")]
            Lvl72 = 262799,
            [Description("73")]
            Lvl73 = 270099,
            [Description("74")]
            Lvl74 = 277499,
            [Description("75")]
            Lvl75 = 284999,
            [Description("76")]
            Lvl76 = 292599,
            [Description("77")]
            Lvl77 = 300299,
            [Description("78")]
            Lvl78 = 308099,
            [Description("79")]
            Lvl79 = 315999,
            [Description("80")]
            Lvl80 = 324999,
            [Description("81")]
            Lvl81 = 332999,
            [Description("82")]
            Lvl82 = 341099,
            [Description("83")]
            Lvl83 = 349299,
            [Description("84")]
            Lvl84 = 357599,
            [Description("85")]
            Lvl85 = 365999,
            [Description("86")]
            Lvl86 = 374499,
            [Description("87")]
            Lvl87 = 383099,
            [Description("88")]
            Lvl88 = 391799,
            [Description("89")]
            Lvl89 = 400599,
            [Description("90")]
            Lvl90 = 409499,
            [Description("91")]
            Lvl91 = 418499,
            [Description("92")]
            Lvl92 = 427599,
            [Description("93")]
            Lvl93 = 436799,
            [Description("94")]
            Lvl94 = 446099,
            [Description("95")]
            Lvl95 = 455499,
            [Description("96")]
            Lvl96 = 464999,
            [Description("97")]
            Lvl97 = 474599,
            [Description("98")]
            Lvl98 = 484299,
            [Description("99")]
            Lvl99 = 494099,
            [Description("100")]
            Lvl100 = 505000

        }

        
    }
}
