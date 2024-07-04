using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TezorwasV2.Services;
using TezorwasV2.View;

namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ForgotPasswordViewModel : ObservableObject
    {
        private readonly IForgotPasswordService _forgotPasswordService;
        private readonly ILoadingSpinnerPopupService _loadingSpinnerPopupService;
        private LoadingSpinnerPopup _loadingPopup = new LoadingSpinnerPopup();

        [ObservableProperty] private string userEmail;

        public ForgotPasswordViewModel(IForgotPasswordService forgotPasswordService, ILoadingSpinnerPopupService loadingSpinnerPopupService)
        {
            _forgotPasswordService = forgotPasswordService;
            _loadingSpinnerPopupService = loadingSpinnerPopupService;
        }

        [RelayCommand]
        public async Task InitiateForgotPasswordProcess()
        {
            _loadingPopup = new LoadingSpinnerPopup();
            _loadingSpinnerPopupService.ShowPopup(_loadingPopup);
            await _forgotPasswordService.SendEmailNewPassword(UserEmail);
            _loadingSpinnerPopupService.ClosePopup(_loadingPopup);
        }
    }
}
