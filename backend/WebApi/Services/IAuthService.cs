using WebApi.Models;

namespace WebApi.Services
{
    public interface IAuthService
    {
        string RequestPin(string email);

        Identity ValidatePin(string token, string pin);
    }
}