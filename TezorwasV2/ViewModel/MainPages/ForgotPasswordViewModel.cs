using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TezorwasV2.Services;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ForgotPasswordViewModel : ObservableObject
    {
        private readonly IForgotPasswordService _forgotPasswordService;

        [ObservableProperty] private string userEmail;

        public ForgotPasswordViewModel(IForgotPasswordService forgotPasswordService)
        {
            _forgotPasswordService = forgotPasswordService;
        }

        [RelayCommand]
        public async Task InitiateForgotPasswordProcess()
        {
            await _forgotPasswordService.SendEmailNewPassword(UserEmail);
        }
    }
}
