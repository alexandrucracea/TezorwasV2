using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;
using TezorwasV2.View.AppPages;


namespace TezorwasV2.ViewModel.MainPages
{
    public partial class QuestionsViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        private readonly IPersonService _personService;
        private readonly IProfileService _profileService;

        [ObservableProperty] private string? personName;
        [ObservableProperty] private string? street;
        [ObservableProperty] private string? county;
        [ObservableProperty] private string? city;
        [ObservableProperty] private int? age;
        [ObservableProperty] private string? habbitOneDescription;
        [ObservableProperty] private string? habbitTwoDescription;
        [ObservableProperty] private string? habbitThreeDescription;
        [ObservableProperty] private double? habbitLevelOfWaste;
        public string DataValidationMessage { get; private set; } = string.Empty;
        public bool IsOkToContinueRegistration;


        public QuestionsViewModel(IGlobalContext globalContext, IPersonService personService,
            IProfileService profileService)
        {
            _globalContext = globalContext;
            _personService = personService;
            _profileService = profileService;
            PersonName = _globalContext.UserFirstName;
        }

        public bool ValidateHabbitCompletion()
        {
            if (HabbitOneDescription is null
                || HabbitTwoDescription is null
                || HabbitThreeDescription is null)
            {
                DataValidationMessage = "You have to complete all habbits in order to continue";
                return false;
            }

            return true;

        }

        public List<HabbitModel> CreateHabbitList()
        {
            List<HabbitModel> habbitList = new List<HabbitModel>();

            habbitList.Add(new HabbitModel
            {
                Description = HabbitOneDescription!,
                InputDate = DateTime.Now,
                LevelOfWaste = HabbitLevelOfWaste!.Value
            });
            habbitList.Add(new HabbitModel
            {
                Description = HabbitTwoDescription!,
                InputDate = DateTime.Now,
                LevelOfWaste = HabbitLevelOfWaste!.Value
            });
            habbitList.Add(new HabbitModel
            {
                Description = HabbitThreeDescription!,
                InputDate = DateTime.Now,
                LevelOfWaste = HabbitLevelOfWaste!.Value
            });

            return habbitList;
        }

        public async Task<Dictionary<string, dynamic>> UpdateUserDetailsTask()
        {
            if (!ValidateHabbitCompletion())
            {
                IsOkToContinueRegistration = false;
                IsOkToContinueRegistration = false;
                return new Dictionary<string, dynamic>
                {
                    { "statusCode", (int)Enums.StatusCodes.InternalServerError },
                    { "message", "The registration process could not be completed" }
                };

            }

            PersonDto personToUpdate = new PersonDto
            {
                LastName = _globalContext.UserLastName,
                FirstName = _globalContext.UserFirstName,
                Email = _globalContext.Email,
                Id = _globalContext.PersonId

            };

            if (Age is not null)
            {
                personToUpdate.Age = Age;
            }

            AddressModel userAddress = new AddressModel();
            if (Street is not null)
            {
                userAddress.StreetName = Street;
            }

            if (County is not null)
            {
                userAddress.County = County;
            }

            if (City is not null)
            {
                userAddress.City = City;
            }

            personToUpdate.Address = userAddress;
            var x = _profileService.GetProfileInfo("YzMhKByE6ZhRCAHUw3Cx", _globalContext.UserToken);

            HttpCallResponseData updatePersonResponse =
                await _personService.UpdateAPerson(personToUpdate, _globalContext.UserToken);

            if (updatePersonResponse.StatusCode == (int)Enums.StatusCodes.Success)
            {
                IsOkToContinueRegistration = true;
                return new Dictionary<string, dynamic>
                {
                    { "statusCode", updatePersonResponse.StatusCode },
                    { "message", "The person was updated successfully" }
                };

            }

            IsOkToContinueRegistration = false;
            return new Dictionary<string, dynamic>
            {
                { "statusCode", (int)Enums.StatusCodes.InternalServerError },
                { "message", "The registration process could not be completed" }
            };
        }

        public async Task<Dictionary<string, dynamic>> CreateUserProfile(List<HabbitModel>? habbits)
        {
            ProfileDto profileToCreate = new ProfileDto
            {
                JoinDate = DateTime.Now,
                Level = (int)Enums.Level.One,
                Xp = 0,
                PersonId = _globalContext.PersonId,
            };
            if (habbits is not null)
            {
                profileToCreate.Habbits = habbits;
            }

            HttpCallResponseData createProfileResponse =
                await _profileService.CreateProfile(profileToCreate, _globalContext.UserToken);
            if (createProfileResponse.StatusCode == (int)Enums.StatusCodes.Success)
            {
                return new Dictionary<string, dynamic>
                {
                    { "statusCode", createProfileResponse.StatusCode },
                    { "globalContext", _globalContext }
                };
            }

            return new Dictionary<string, dynamic>
            {
                { "statusCode", (int)Enums.StatusCodes.InternalServerError },
                { "message", "The creation of your profile could not be completed. Please try again" }
            };
        }

        [RelayCommand]
        public async Task UpdatePersonData()
        {
            var updateUserResponse = await UpdateUserDetailsTask();
            if (updateUserResponse.TryGetValue("statusCode", out var value))
            {
                if (value == (int)Enums.StatusCodes.Success)
                {
                    var habbits = CreateHabbitList();

                    var createProfileResponse = await CreateUserProfile(habbits);
                    if (createProfileResponse.TryGetValue("statusCode", out var val))
                    {
                        if (val == (int)Enums.StatusCodes.Success)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(TasksView)}", true);
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("The creation of your profile failed. Please try again", DataValidationMessage, "OK");
                        }
                    }

                }
                else
                {
                    await Shell.Current.DisplayAlert("The registration process failed. Please try again", DataValidationMessage, "OK");
                }

            }
        }
    }
}
