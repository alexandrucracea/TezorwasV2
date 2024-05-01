using TezorwasV2.DTO;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public interface IRegisterService
    {
        AuthHelper? AuthHelper { get; set; }
        Task<HttpCallResponseData> RegisterUser(UserDto userToRegister);
    }
}
