using TezorwasV2.Helpers;

namespace TezorwasV2.Services
{
    public interface IForgotPasswordService
    {
        TezorwasApiHelper TezorwasApiHelper { get; set; }

        Task<HttpCallResponseData> SendEmailNewPassword(string email);
    }
}