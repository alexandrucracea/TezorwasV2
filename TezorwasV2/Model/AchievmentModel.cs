
namespace TezorwasV2.Model
{
    public class AchievmentModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int LevelRequired { get; set; }
        public int XpEarned { get; set; }
    }
}
