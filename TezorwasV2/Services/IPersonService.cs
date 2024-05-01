
using TezorwasV2.DTO;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public interface IPersonService
    {
        TezorwasApiHelper TezorwasApiHelper { get; set; }
        Task<HttpCallResponseData> CreatePerson(PersonDto personToCreate, string bearerToken);
        Task<HttpCallResponseData> UpdateAPerson(PersonDto personToUpdate, string bearerToken);
    }
}
