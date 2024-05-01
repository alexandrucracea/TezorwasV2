
using TezorwasV2.DTO;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public interface IProfileService
    {
        TezorwasApiHelper TezorwasApiHelper { get; set; }
        Task<HttpCallResponseData> CreateProfile(ProfileDto profileToCreate, string bearerToken);
        Task<dynamic> GetProfileInfo(string profileId, string bearerToken);
    }
}
