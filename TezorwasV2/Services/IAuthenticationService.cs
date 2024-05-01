using TezorwasV2.DTO;
using TezorwasV2.Helpers;

namespace TezorwasV2.Services;
public interface IAuthenticationService
{
    AuthHelper? AuthHelper { get; set; }
    Task<HttpCallResponseData> AuthenticateUser(UserDto userToAuthenticate);
}

