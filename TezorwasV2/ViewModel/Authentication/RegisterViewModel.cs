using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Services;
using TezorwasV2.View;
using TezorwasV2.View.AppPages;


namespace TezorwasV2.ViewModel
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IRegisterService _registerService;
        private readonly IPersonService _personService;
        private readonly IGlobalContext _globalContext;

        public string DataValidationMessage { get; private set; } = string.Empty;

        [ObservableProperty] private string? lastName;
        [ObservableProperty] private string? firstName;
        [ObservableProperty] private string? email;
        [ObservableProperty] private string? password;
        [ObservableProperty] private string? confirmPassword;
        [ObservableProperty] private bool isBusy;

        public RegisterViewModel(IRegisterService registerService, IPersonService personService, IGlobalContext globalContext)
        {
            _registerService = registerService;
            _personService = personService;
            _globalContext = globalContext;
        }

        //todo de adaugat pe observable object validare cu textul care se schimba dedesupt
        public bool ValidateRegisterData()
        {
            if (LastName is null)
            {
                DataValidationMessage = "Last name field is mandatory!";
                return false;
            }
            if (FirstName is null)
            {
                DataValidationMessage = "First name field is mandatory!";
                return false;
            }
            if (Email is null)
            {
                DataValidationMessage = "Email is mandatory!";
                return false;
            }
            if (Password is null)
            {
                DataValidationMessage = "Password is mandatory!";
                return false;
            }
            //todo de gandit daca lasam asa sau punem cu trim
            if (Email.Contains(' '))
            {
                DataValidationMessage = "Email should not contain spaces!";
                return false;
            }

            string emailPatternToMatch = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(Email, emailPatternToMatch))
            {
                DataValidationMessage = "The email provided is not a correct email address!";
                return false;
            }

            string passwordPatternToMatch = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(Password, passwordPatternToMatch))
            {
                DataValidationMessage = "The password provided is not in the correct format!!";
                return false;
            }

            if (string.Compare(Password, ConfirmPassword, StringComparison.Ordinal) != 0)
            {
                DataValidationMessage = "The passwords provided don't match";
            }
            return true;
        }

        public async Task<Dictionary<string, dynamic>> RegisterUser()
        {
            UserDto userToRegister = new UserDto
            {
                Username = Email!,
                Password = Password!
            };

            PersonDto personDto = new PersonDto
            {
                Email = Email!,
                FirstName = FirstName!,
                LastName = LastName!
            };

            //todo de facut tranzactie pe astea doua
            HttpCallResponseData callResponse = await _registerService.RegisterUser(userToRegister);
            string bearerToken = callResponse.GetKeyValue("idToken");

            HttpCallResponseData createPersonResponse = await _personService.CreatePerson(personDto, bearerToken);


            if (createPersonResponse.StatusCode == (int)Enums.StatusCodes.Success &&
                callResponse.StatusCode == (int)Enums.StatusCodes.Success)
            {
                _globalContext.UserFirstName = FirstName!;
                _globalContext.Email = Email!;
                _globalContext.UserLastName = LastName!;
                _globalContext.UserToken = bearerToken;
                _globalContext.PersonId = createPersonResponse.GetDataDictionaryValue("id");


                return new Dictionary<string, dynamic>
                {
                    { "statusCode", createPersonResponse.StatusCode },
                    { "globalContext", _globalContext }
                };

            }
            //todo de adaugat validare ca utilizatorul nu exista deja in baza noastra de date
            return new Dictionary<string, dynamic>
            {
                { "statusCode", (int)Enums.StatusCodes.InternalServerError },
                { "message", "The registration process could not be completed" }
            };
        }


        [RelayCommand]
        public async Task Register()
        {
            if (!ValidateRegisterData())
            {
                await Shell.Current.DisplayAlert("Registration failed", DataValidationMessage, "OK");
                return;
            }
            IsBusy = true;
            var registerResponse = await RegisterUser();

            if (registerResponse.TryGetValue("statusCode", out var value))
            {
                if (value == (int)Enums.StatusCodes.Success)
                {
                    var globalContext = registerResponse["globalContext"];

                    var globalContextNavParam = new Dictionary<string, object>
                        {
                            { "globalContext", globalContext }
                        };


                    await Shell.Current.GoToAsync(nameof(QuestionsView), true, globalContextNavParam);


                }
            }
            else
            {
                await Shell.Current.DisplayAlert("CallResponse", "The registration could not be completed", "OK");

            }
            //todo de adaugat o proprietate ceva pe care o atribuim si o adaugam in alerta ori in ceva si de aici vedem cum afisam eroare (pentru viitor)
        }
    }
}
