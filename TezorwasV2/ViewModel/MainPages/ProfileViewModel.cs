
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
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
        [ObservableProperty] private int? age;

        public ObservableCollection<TaskModel> AllTaks { get; } = new ObservableCollection<TaskModel>();
        public ObservableCollection<HabbitModel> AllHabits { get; } = new ObservableCollection<HabbitModel>();


        public ProfileViewModel(IGlobalContext globalContext, IPersonService personService, IProfileService profileService)
        {
            _globalContext = globalContext;
            _personService = personService;
            _profileService = profileService;
            PersonFirstName = _globalContext.UserFirstName;

        }

    }
}
