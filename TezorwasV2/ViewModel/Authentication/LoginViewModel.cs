using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.RegularExpressions;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Services;


namespace TezorwasV2.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authenticationService;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public bool ValidateEmail()
        {
            if (Username.Length == 0)
            {
                return false;
            }

            if (Username.Contains(' '))
            {
                return false;
            }

            string emailPatternToMatch = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(Username, emailPatternToMatch))
            {
                return false;
            }

            return true;
        }

        public async Task<HttpCallResponseData> AuthenticateUser()
        {
            HttpCallResponseData callResponse;
            if (ValidateEmail())
            {
                var userToAuthenticate = new UserDto
                {
                    Username = Username,
                    Password = Password
                };
                callResponse = await _authenticationService.AuthenticateUser(userToAuthenticate);
            }
            else
            {
                callResponse = new HttpCallResponseData
                {
                    Response = "Login process failed",
                    StatusCode = (int)Enums.StatusCodes.InternalServerError
                };
            }

            return callResponse;
        }
    }
}
