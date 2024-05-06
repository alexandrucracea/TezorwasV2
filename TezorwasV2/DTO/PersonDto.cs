
using TezorwasV2.Model;

namespace TezorwasV2.DTO
{
    public class PersonDto
    {
        public string Id { get; set; } = string.Empty; //da
        public string FirstName { get; set; } = string.Empty; //da
        public string LastName { get; set; } = string.Empty; //da
        public int? Age { get; set; }
        public AddressModel? Address { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
