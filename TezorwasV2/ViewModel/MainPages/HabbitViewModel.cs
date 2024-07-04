

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TezorwasV2.DTO;
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

        public async Task AddHabbitToProfile(HabbitModel habbitToAdd)
        {
            var profile = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            profile.Habbits.Add(habbitToAdd);

            ProfileHabbits.Add(habbitToAdd);
            

            await _profileService.UpdateAProfile(profile, _globalContext.UserToken);
        }

        public async Task DeleteHabbit(HabbitModel habbitToDelete)
        {

            var itemToRemove = ProfileHabbits.FirstOrDefault(h => h.Description.Equals(habbitToDelete.Description));
            if (itemToRemove != null)
            {
                ProfileHabbits.Remove(itemToRemove);
            }
            ProfileDto profile = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            var habbitToDel = profile.Habbits.FirstOrDefault(h => h.Description.Equals(habbitToDelete.Description));
            if (habbitToDelete != null)
            {
                profile.Habbits.Remove(habbitToDel);
            }

            await _profileService.UpdateAProfile(profile as dynamic, _globalContext.UserToken);
        }

    }
}
