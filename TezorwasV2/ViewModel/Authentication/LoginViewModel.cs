using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Services;


namespace TezorwasV2.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IPersonService _personService;
        private readonly IGlobalContext _globalContext;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [ObservableProperty] public bool isBusy;

        public LoginViewModel(IAuthenticationService authenticationService, IPersonService personService, IGlobalContext globalContext)
        {
            _authenticationService = authenticationService;
            _personService = personService;
            _globalContext = globalContext;
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
                string bearerToken = callResponse.GetKeyValue("idToken");

                var allPersons = await _personService.GetAllPersons(bearerToken);
                var searchedPerson = allPersons.FirstOrDefault(person => person.Email.Equals(Username, StringComparison.OrdinalIgnoreCase));
                
                _globalContext.PersonId = searchedPerson.Id;
                _globalContext.UserToken = bearerToken;

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

        [RelayCommand]
        private void ShowActivityIndicator()
        {
            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                Thread.Sleep(5000);
                IsBusy = false;
            });
        }
    }
}
