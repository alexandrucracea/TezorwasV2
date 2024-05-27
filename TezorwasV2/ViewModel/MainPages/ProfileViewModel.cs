using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        private readonly IPersonService _personService;
        private readonly IProfileService _profileService;

        [ObservableProperty] private string? personFirstName;
        [ObservableProperty] private string? personLastName;
        [ObservableProperty] private int? age;
        [ObservableProperty] private string city;
        [ObservableProperty] private string county;
        [ObservableProperty] private string streetName;
        [ObservableProperty] private string fullAddress;
        [ObservableProperty] private string fullName;
        [ObservableProperty] private int xp;
        [ObservableProperty] private string lvl;
        [ObservableProperty] private int habbitNo;
        [ObservableProperty] private int tasksDone;
        [ObservableProperty] private decimal xpBarValue;

        public ObservableCollection<TaskModel> AllTaks { get; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<HabbitModel> AllHabits { get; } = new ObservableCollection<HabbitModel>();


        public ProfileViewModel(IGlobalContext globalContext, IPersonService personService, IProfileService profileService)
        {
            _globalContext = globalContext;
            _personService = personService;
            _profileService = profileService;
            PersonFirstName = _globalContext.UserFirstName;

        }
        public async Task InitializeProfile()
        {
            PersonDto personToRead = await _personService.GetPersonInfo(_globalContext.PersonId, _globalContext.UserToken);
            ProfileDto profileToRead = await _profileService.GetProfileInfo(_globalContext.ProfileId,_globalContext.UserToken);
            if(personToRead != null)
            {
                PersonFirstName = personToRead.FirstName;
                PersonLastName = personToRead.LastName;
                Age = personToRead.Age;
                StreetName = personToRead.Address.StreetName;
                City = personToRead.Address.City;
                County = personToRead.Address.County;
                FullAddress = City +" "+ County + " " + StreetName;
                FullName = PersonFirstName + " " + PersonLastName;
            }
            if(profileToRead != null)
            {
                Xp = profileToRead.Xp;
                Lvl = "Lvl. " + profileToRead.Level.ToString();
                HabbitNo = profileToRead.Habbits.Count;
                TasksDone = profileToRead.Tasks.Where(task => task.IsCompleted == true).Count();
                foreach (Enums.Level level in Enum.GetValues(typeof(Enums.Level)))
                {
                    if(Xp < (int)level)
                    {
                        XpBarValue = (decimal)((Xp * 1.0)/(int)level);
                        break;
                    }
                }
            }
        }
    }
}
