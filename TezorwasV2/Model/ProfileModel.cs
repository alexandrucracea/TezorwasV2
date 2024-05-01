
namespace TezorwasV2.Model
{
    public class ProfileModel
    {
        public string Id { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        //public AchievmentsModel? Achievments { get; set; }
        public string PersonId { get; set; } = string.Empty;
        public int Level { get; set; }
        //public HabbitsModel? Habbits { get; set; }
        //TODO add friendlist model to this logic -> maybe to the api also
    }
}
