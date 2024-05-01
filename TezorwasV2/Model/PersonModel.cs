
namespace TezorwasV2.Model
{
    public class PersonModel
    {
        public string Id { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public int? Age { get; set; }
        public AddressModel? Address { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
