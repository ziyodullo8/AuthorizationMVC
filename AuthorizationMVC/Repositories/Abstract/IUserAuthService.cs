using AuthorizationMVC.Models.DTO;

namespace AuthorizationMVC.Repositories.Abstract
{
    public interface IUserAuthService
    {
        Task<Status> LOginAsync(LoginModel model);
        Task<Status> RegistrationAsynnc(RegistrationModel model);
        Task LogoutAsync();
    }
}
