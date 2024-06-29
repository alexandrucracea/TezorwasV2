

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class HabbitViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        private readonly IProfileService _profileService;


        public ObservableCollection<HabbitModel> ProfileHabbits = new ObservableCollection<HabbitModel>();

        public HabbitViewModel(IGlobalContext globalContext, IProfileService profileService)
        {
            _globalContext = globalContext;
            _profileService = profileService;
        }
        public async Task GetAllProfileHabbits()
        {
            ProfileHabbits.Clear();
            var profile = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);

            foreach(var habbit in profile.Habbits)
            {
                ProfileHabbits.Add(new HabbitModel
                {
                    Description = habbit.Description,
                    InputDate = habbit.InputDate,
                    LevelOfWaste = habbit.LevelOfWaste
                });
            }
        }
    }
}
