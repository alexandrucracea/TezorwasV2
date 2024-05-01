
namespace TezorwasV2.Helpers
{
    public class GlobalContext : IGlobalContext
    {
        public string UserToken { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PersonId { get; set; } = string.Empty;
        //todo de adaugat profileId aici in contextul global
    }
}
