
namespace TezorwasV2.Helpers
{
    public static class Enums
    {
        public enum Paths
        {
            persons,
            profiles
        }

        public enum StatusCodes
        {
            Success = 200,
            InternalServerError = 500,
            NotFound = 404
        }

        public enum WasteCategory
        {
            EcoHero = 1, //lowest level of waste
            GreenGuru = 3, //medium level of waste
            PlanetSaver = 5 //highest level of waste
        }

        public enum Level
        {
            One = 1,
        }
    }
}
