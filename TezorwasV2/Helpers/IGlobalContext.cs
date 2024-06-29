
namespace TezorwasV2.Helpers
{
    public interface IGlobalContext
    {
        string UserToken { get; set; }
        string UserFirstName { get; set; }
        string Email { get; set; }
        string UserLastName { get; set; }
        string PersonId { get; set; }
        string ProfileId { get; set; }

        void ClearUserData();
    }
}
